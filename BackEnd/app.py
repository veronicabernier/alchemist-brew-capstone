import datetime
import bcrypt
from sqlalchemy import create_engine, ForeignKey, Column, Text, Integer, Boolean, Date, Float, DateTime, String, \
    DateTime, update, Null, null
from sqlalchemy_utils import database_exists, create_database
from sqlalchemy.orm import sessionmaker, declarative_base
from BackEnd.config.dbconfig import pg_config as settings
from flask import Flask, request
from flask_cors import CORS
from flask import jsonify
import json
import tables
import encoders

app = Flask(__name__)
#db = SQLAlchemy(app)
CORS(app)

salt = bcrypt.gensalt()
def get_engine(user, passwd, host, port, db):
    url = f"postgresql://{user}:{passwd}@{host}:{port}/{db}"
    if not database_exists(url):
        create_database(url)
    engine = create_engine(url, pool_size=50, echo=False)
    return engine

engine = get_engine(settings['User'],
          settings['Password'],
          settings['Host'],
          settings['Port'],
          settings['Database'])

def get_engine_from_settings():
    keys = ['User', 'Password', 'Host', 'Port', 'Database']
    if not all(key in keys for key in settings.keys()):
        raise Exception('Bad config file')

    return get_engine(settings['User'],
          settings['Password'],
          settings['Host'],
          settings['Port'],
          settings['Database'])

def get_session():
    engine = get_engine_from_settings()
    session = sessionmaker(bind=engine)()
    return session

Session = get_session()

engine = Session.get_bind()

Base = declarative_base()

Base.metadata.create_all(engine)

engine = Session.get_bind()


@app.route("/signup", methods=['POST', 'GET'])
def register():
    if request.method == "POST":
        username = request.form.get('username')
        email = request.form.get('email')
        password = request.form.get("password")
        confirm = request.form.get("confirm")
        birth_date = request.form.get("birth_date")
        gender = request.form.get("gender")
        location = request.form.get("location")

        emaildata = Session.query(tables.users).filter_by(email=email).first()
        t=datetime.datetime.now().strftime("%I:%M%p")
        d=datetime.datetime.now().date()

        print(d, t)
        print(username, email, password, confirm, birth_date, gender, location)
        if emaildata == None:
            if password == confirm:
                passencode = password.encode(encoding='UTF-8', errors='strict')
                print(passencode)
                hashpass = bcrypt.hashpw(passencode, salt)
                print(hashpass)
                storedpass = hashpass.decode(encoding='UTF-8', errors='strict')
                Session.add(tables.users(username=username, email=email, password=storedpass, private_profile=True, birth_date=birth_date, gender=gender, location=location))
                Session.commit()
                Session.flush()
                return jsonify("Singup complete"), 201
            else:
                Session.flush()
                return jsonify(Error="Password does not match"), 404
        else:
            Session.flush()
            return jsonify(Error="User can't register, user already exists"), 403

    return jsonify(Error="Missing Information"), 400

@app.route("/login", methods=["POST", "GET"])
def login():
    if request.method == "POST":
        email = request.form.get('email')
        password = request.form.get("password")

        emaildata = Session.query(tables.users.email).filter_by(email=email).first()
        passworddata = Session.query(tables.users.password).filter_by(email=email).first()

        print(emaildata)
        print(passworddata[0])
        print(email, password)

        passencode = password.encode(encoding='UTF-8', errors='strict')
        print(passencode)
        hashpass = bcrypt.hashpw(passencode, salt)
        print(hashpass)

        if emaildata is None:

            return jsonify(Error="User does not exist"), 400
        else:
            if (passworddata is None):
                return jsonify(Error="Incorrect password"), 400
            else:
               if (bcrypt.checkpw(password.encode(encoding='UTF-8', errors='strict'), passworddata[0].encode(encoding='UTF-8', errors='strict'))):
                return jsonify("Succesfull login"), 400  # to be edited from here do redict to either svm or home

    return jsonify(Error="User or password is incorrect"), 400

@app.route("/<userid>/recipe list", methods=["POST", "GET"])
def get_all_recipes(userid):
    result_list = []
    if request.method == "GET":
        prebrew = Session.query(tables.recipes).filter_by(userid=userid, recipe_visibility=True).all()

        for i in prebrew:
            result = encoders.encoder_recipes(i)
            result_list.append(result)
        return jsonify(Recepies=result_list), 200
@app.route("/<userid>/recipe list/<recipeid>", methods=["POST", "GET"])
def get_recipe(userid, recipeid):
    result_list = []
    if request.method == "GET":
        prebrew = Session.query(tables.recipes).filter_by(userid=userid, recipeid=recipeid, recipe_visibility=True).all()

        for i in prebrew:
            result = encoders.encoder_recipes(i)
            result_list.append(result)
        return jsonify(Recipe=result_list), 200
@app.route("/<userid>/<recipeid>/edit", methods=["PUT"])
def edit_recipe(userid, recipeid):
    result_list = []
    new_brew_method = request.form.get('brew_method')
    new_grind_setting = request.form.get('grind_setting')
    new_brand = request.form.get('brand')
    new_roast = request.form.get('roast')
    new_bean_type = request.form.get('bean_type')
    new_coffee_weight = request.form.get('coffee_weight')
    if request.method == "PUT":
        prebrew = Session.query(tables.recipes).filter_by(userid=userid, recipeid=recipeid).all()

        for i in prebrew:
            result = encoders.encoder_recipes(i)
            result_list.append(result)
            print(result.get('brew_method'))
        if(new_brew_method == None or len(new_brew_method) == 0):
            new_brew_method = result_list[0].get('brew_method')
        if (new_grind_setting == None or len(new_grind_setting) == 0):
            new_grind_setting = result_list[0].get('grind_setting')
        if(new_brand == None or len(new_brand) == 0):
            new_brand = result_list[0].get('brand')
        if(new_roast == None or len(new_roast) == 0):
            new_roast = result_list[0].get('roast')
        if(new_bean_type == None or len(new_bean_type) == 0):
            new_bean_type = result_list[0].get('bean_type')
        if(new_coffee_weight == None or len(new_coffee_weight) == 0):
            new_coffee_weight = result_list[0].get('coffee_weight')

        Session.execute(update(tables.recipes).filter_by(recipeid=recipeid).values(
            brew_method=new_brew_method, grind_setting=new_grind_setting, brand=new_brand, roast=new_roast,
            bean_type=new_bean_type, coffee_weight=new_coffee_weight))

        Session.commit()
        Session.flush()

        return jsonify(Edited=result_list), 200

@app.route("/<userid>/recipe list/add", methods=["POST", "GET"])
def add_recipe(userid):
    if request.method == "POST":
        brew_method = request.form.get('brew_method')
        grind_setting = request.form.get('grind_setting')
        brand = request.form.get('brand')
        roast = request.form.get('roast')
        bean_type = request.form.get('bean_type')
        coffee_weight = request.form.get('coffee_weight')

        if (brew_method == None or len(brew_method) == 0):
            return jsonify(Error="Please enter brew method")
        if(brand == None or len(brand)==0):
            return jsonify(Error="Please enter coffee brand")
        if (grind_setting == None or len(grind_setting) == 0):
            return jsonify(Error="Please enter coffee grind setting")
        if (not grind_setting.isnumeric()):
            return jsonify(Error="Please enter a number for the coffee grind setting")
        if(roast == None or len(roast)==0):
            roast = 'Not specified'
        if (bean_type == None or len(bean_type) == 0):
            bean_type = 'Not specified'
        if (coffee_weight == None or len(coffee_weight) == 0):
            return jsonify(Error="Please enter coffee weight")

        Session.add(
            tables.recipes(brew_method=brew_method.lower(), grind_setting=grind_setting, brand=brand.lower(), roast=roast.lower(),
                  bean_type=bean_type.lower(), coffee_weight=coffee_weight, userid=userid, recipe_visibility=True))
        Session.commit()
        Session.flush()
        return jsonify("Recipe created"), 201

@app.route("/<userid>/<recipeid>/add", methods=["POST", "GET"])
def new_brew(userid, recipeid):
    result_list = []
    if request.method == "POST":
        ext_time = request.form.get('ext_time')
        ext_weight = request.form.get('ext_weight')
        flavor = request.form.get('flavor')

        prebrew = Session.query(tables.recipes).filter_by(userid=userid, recipeid=recipeid, recipe_visibility=True).all()

        for i in prebrew:
            result = encoders.encoder_recipes(i)
            result_list.append(result)

        brew_method = result_list[0].get('brew_method')
        grind_setting = result_list[0].get('grind_setting')
        brand = result_list[0].get('brand')
        roast = result_list[0].get('roast')
        bean_type = result_list[0].get('bean_type')
        coffee_weight = result_list[0].get('coffee_weight')

        Session.add(
            tables.brews( recipeid=recipeid, brew_method=brew_method, grind_setting=grind_setting, brand=brand, roast=roast,
                  bean_type=bean_type, coffee_weight=coffee_weight, userid=userid, ext_time=ext_time,
                   ext_weight=ext_weight, flavor=flavor, date=datetime.datetime.now(), brew_visibility=True))
        Session.commit()
        Session.flush()
        return jsonify("Recipe created"), 201

@app.route("/<userid>/brew list", methods=["POST", "GET"])
def get_brew_attempt(userid):
    result_list = []
    if request.method == "GET":
        attempt = Session.query(tables.brews).filter_by(userid=userid).all()

        for i in attempt:
            result = encoders.encoder_brews(i)
            result_list.append(result)
        return jsonify(Attempts=result_list), 200

@app.route("/<userid>/espresso_scores", methods=["POST", "GET"])
def get_espresso_scores(userid):
    result_list = []
    if request.method == "GET":
        espresso = Session.query(tables.espresso_scores).filter_by(userid=userid).all()
        # drip = Session.query(tables.drip_scores).filter_by(userid=userid).all()

        for i in espresso:
            result = encoders.encoder_score(i)
            result_list.append(result)
        return jsonify(Attempts=result_list), 200
@app.route("/<userid>/drip_scores", methods=["POST", "GET"])
def get_drip_scores(userid):
    result_list = []
    if request.method == "GET":
        drip = Session.query(tables.drip_scores).filter_by(userid=userid).all()

        for i in drip:
            result = encoders.encoder_score(i)
            result_list.append(result)
        return jsonify(Attempts=result_list), 200

@app.route("/<userid>/espresso_simulation", methods=["POST", "GET"])
def new_espresso_score(userid):
    if request.method == "POST":
        weightScore = request.form.get('weightScore')
        weightScoreTotal = request.form.get('weightScoreTotal')
        reservoirScore = request.form.get('reservoirScore')
        reservoirScoreTotal = request.form.get('reservoirScoreTotal')
        powerOnScore = request.form.get('powerOnScore')
        powerOnScoreTotal = request.form.get('powerOnScoreTotal')
        grindScore = request.form.get('grindScore')
        grindScoreTotal = request.form.get('grindScoreTotal')
        tampScore = request.form.get('tampScore')
        tampScoreTotal = request.form.get('tampScoreTotal')
        brewScore = request.form.get('brewScore')
        brewScoreTotal = request.form.get('brewScoreTotal')
        serveScore = request.form.get('serveScore')
        serveScoreTotal = request.form.get('serveScoreTotal')
        scoreTotal = int(weightScore) + int(reservoirScore) + int(powerOnScore) + int(grindScore) + \
                     int(tampScore) + int(brewScore) + int(serveScore)
        evalTotal = int(weightScoreTotal) + int(reservoirScoreTotal) + int(powerOnScoreTotal) + \
                    int(grindScoreTotal) + int(tampScoreTotal) + int(brewScoreTotal) + int(serveScoreTotal)
        grade = (scoreTotal/evalTotal)*100

        Session.add(
            tables.espresso_scores( userid=userid, weightScore=weightScore, weightScoreTotal=weightScoreTotal,
            reservoirScore=reservoirScore, reservoirScoreTotal=reservoirScoreTotal, powerOnScore=powerOnScore,
            powerOnScoreTotal=powerOnScoreTotal, grindScore=grindScore, grindScoreTotal=grindScoreTotal, tampScore=tampScore,
            tampScoreTotal=tampScoreTotal, brewScore=brewScore, brewScoreTotal=brewScoreTotal,
            serveScore=serveScore, serveScoreTotal=serveScoreTotal, scoreTotal=scoreTotal,
            evalTotal=evalTotal, grade=grade, dateObtained=datetime.datetime.now()))
        Session.commit()
        Session.flush()
        return jsonify("Scores recorded"), 201

@app.route("/<userid>/drip_simulation", methods=["POST", "GET"])
def new_drip_score(userid):
    if request.method == "POST":
        weightScore = request.form.get('weightScore')
        weightScoreTotal = request.form.get('weightScoreTotal')
        reservoirScore = request.form.get('reservoirScore')
        reservoirScoreTotal = request.form.get('reservoirScoreTotal')
        grindScore = request.form.get('grindScore')
        grindScoreTotal = request.form.get('grindScoreTotal')
        chooseFilterScore = request.form.get('chooseFilterScore')
        chooseFilterScoreTotal = request.form.get('chooseFilterScoreTotal')
        refillReservoirScore = request.form.get('chooseFilterScore')
        refillReservoirScoreTotal = request.form.get('refillReservoirScoreTotal')
        brewScore = request.form.get('brewScore')
        brewScoreTotal = request.form.get('brewScoreTotal')
        serveScore = request.form.get('serveScore')
        serveScoreTotal = request.form.get('serveScoreTotal')
        scoreTotal = int(weightScore) + int(reservoirScore) + int(chooseFilterScore) + int(grindScore) + \
                     int(refillReservoirScore) + int(brewScore) + int(serveScore)
        evalTotal = int(weightScoreTotal) + int(reservoirScoreTotal) + int(chooseFilterScoreTotal) + \
                    int(grindScoreTotal) + int(refillReservoirScoreTotal) + int(brewScoreTotal) + int(serveScoreTotal)
        grade = (scoreTotal/evalTotal)*100

        Session.add(
            tables.drip_scores( userid=userid, weightScore=weightScore, weightScoreTotal=weightScoreTotal,
            reservoirScore=reservoirScore, reservoirScoreTotal=reservoirScoreTotal, chooseFilterScore=chooseFilterScore,
            chooseFilterScoreTotal=chooseFilterScoreTotal, grindScore=grindScore, grindScoreTotal=grindScoreTotal,
            refillReservoirScore=refillReservoirScore, refillReservoirScoreTotal=refillReservoirScoreTotal,
            brewScore=brewScore, brewScoreTotal=brewScoreTotal,serveScore=serveScore, serveScoreTotal=serveScoreTotal,
            scoreTotal=scoreTotal, evalTotal=evalTotal, grade=grade, dateObtained=datetime.datetime.now()))
        Session.commit()
        Session.flush()
        return jsonify("Scores recorded"), 201


if __name__ == '__main__':
    app.run(debug=True)

engine.dispose()
Session.close()