import datetime
import bcrypt
import os
import math
import random
import smtplib
from sqlalchemy import create_engine, update, text
from sqlalchemy import create_engine, ForeignKey, Column, Text, Integer, Boolean, Date, Float, DateTime, String, \
    DateTime, update, null
from sqlalchemy_utils import database_exists, create_database
from sqlalchemy.orm import sessionmaker, declarative_base
from config.dbconfig import pg_config as settings
from flask import Flask, request
from flask_cors import CORS
from flask import jsonify

import tables
import encoders

app = Flask(__name__)
#db = SQLAlchemy(app)
CORS(app)

# salt = bcrypt.gensalt()
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
        salt = bcrypt.gensalt()
        username = request.form.get('username')
        email = request.form.get('email')
        password = request.form.get("password")
        confirm = request.form.get("confirm")
        birth_date = request.form.get("birth_date")
        gender = request.form.get("gender")
        location = request.form.get("location")

        emaildata = Session.query(tables.users).filter_by(email=email).first()
        t = datetime.datetime.now().strftime("%I:%M%p")
        d = datetime.datetime.now().date()

        if emaildata == None:
            if password == confirm:
                passencode = password.encode(encoding='UTF-8', errors='strict')
                hashpass = bcrypt.hashpw(passencode, salt)
                storedpass = hashpass.decode(encoding='UTF-8', errors='strict')
                server = smtplib.SMTP('smtp.gmail.com', 587)
                server.starttls()
                password = 'j x s y g g q w m f g n b f v h'
                server.login('alchemy.coffee.brew@gmail.com', password)
                OTP = ''.join([str(random.randint(0, 9)) for i in range(4)])
                OTPencode = OTP.encode(encoding='UTF-8', errors='strict')
                hashOTP = bcrypt.hashpw(OTPencode, salt)
                storedOTP = hashOTP.decode(encoding='UTF-8', errors='strict')
                msg = 'Hello, Your OTP is ' + str(OTP)
                sender = 'alchemy.coffee.brew@gmail.com'  # write email id of sender
                server.sendmail(sender, email, msg)
                Session.add(tables.users(username=username, email=email, password=storedpass,
                                              private_profile=True, birth_date=birth_date, gender=gender,
                                              location=location))
                Session.commit()
                userid=Session.query(tables.users.userid).filter_by(email=email).first()
                Session.add(tables.confirmations(userid=userid[0], email=email, confirmation_code=storedOTP, confirmation=False))
                server.quit()
                Session.commit()
                Session.flush()
                return jsonify("Signup complete"), 201
            else:
                Session.flush()
                return jsonify(Error="Password does not match"), 404
        else:
            Session.flush()
            return jsonify(Error="User can't register, user already exists"), 403

    return jsonify(Error="Missing Information"), 400

@app.route("/signup/<userid>/verify", methods=['POST', 'GET'])
def account_verification(userid):
    result_list = []
    if request.method == "POST":
        confirmation_code = request.form.get('confirmation_code')
        verification_code = Session.query(tables.confirmations.confirmation_code).filter_by(userid=userid).first()
        # for i in verification_code:
        #     result = encoders.encoder_temp_user(i)
        #     result_list.append(result)
        # print(result_list[0].get('confirmation_code'), confirmation_code)
        # print(result_list[0].get('confirmation_code') == int(confirmation_code))
        if(bcrypt.checkpw(confirmation_code.encode(encoding='UTF-8', errors='strict'),
                          verification_code[0].encode(encoding='UTF-8', errors='strict'))):
            Session.execute(update(tables.confirmations).filter_by(userid=userid).values(
                confirmation=True))
            Session.commit()
            Session.flush()
            return jsonify("Email Verified")
        return jsonify(Error="Wrong code")



@app.route("/login", methods=["POST", "GET"])
def login():
    if request.method == "POST":
        email = request.form.get('email')
        password = request.form.get("password")
        salt = bcrypt.gensalt()

        emaildata = Session.query(tables.users.email).filter_by(email=email).first()
        passworddata = Session.query(tables.users.password).filter_by(email=email).first()

        passencode = password.encode(encoding='UTF-8', errors='strict')
        # hashpass = bcrypt.hashpw(passencode, salt)

        if emaildata is None:

            return jsonify(Error="User does not exist"), 400
        else:
            if (passworddata is None):
                return jsonify(Error="Incorrect password"), 400
            else:
               if (bcrypt.checkpw(password.encode(encoding='UTF-8', errors='strict'), passworddata[0].encode(encoding='UTF-8', errors='strict'))):
                return jsonify("Succesfull login"), 400  # to be edited from here do redict to either svm or home
            

    return jsonify(Error="User or password is incorrect"), 400

@app.route("/<userid>/profile", methods=["POST", "GET"])
def get_user_profile(userid):
    result_list = []
    if request.method == "GET":
        profile = Session.query(tables.users).filter_by(userid=userid).all()

        for i in profile:
            result = encoders.encoder_user(i)
            result_list.append(result)
        return jsonify(Recepies=result_list), 200

@app.route("/<userid>/profile/edit", methods=["PUT"])
def edit_user_profile(userid):
    result_list = []
    username = request.form.get('username')
    email = request.form.get('email')
    private_profile = request.form.get('private_profile')
    birth_date = request.form.get('birth_date')
    gender = request.form.get('gender')
    location = request.form.get('location')
    if request.method == "PUT":
        profile = Session.query(tables.users).filter_by(userid=userid).all()

        for i in profile:
            result = encoders.encoder_user(i)
            result_list.append(result)

        if(username == None or len(username) == 0):
            username = result_list[0].get('username')
        if (email == None or len(email) == 0):
            email = result_list[0].get('email')
        if(private_profile == None or len(private_profile) == 0):
            private_profile = result_list[0].get('private_profile')
        if(birth_date == None or len(birth_date) == 0):
            birth_date = result_list[0].get('birth_date')
        if(gender == None or len(gender) == 0):
            gender = result_list[0].get('gender')
        if(location == None or len(location) == 0):
            location = result_list[0].get('location')

        Session.execute(update(tables.users).filter_by(userid=userid).values(
            username=username, email=email, private_profile=private_profile, birth_date=birth_date,
            gender=gender, location=location))

        Session.commit()
        Session.flush()
        return jsonify("Profile edited"), 200

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
    recipe_list = []
    tag_list = []
    if request.method == "POST":
        ext_time = request.form.get('ext_time')
        ext_weight = request.form.get('ext_weight')
        notes = request.form.get('notes')
        tagid = request.form.get('tagid')

        prebrew = Session.query(tables.recipes).filter_by(userid=userid, recipeid=recipeid, recipe_visibility=True).all()
        tag = Session.query(tables.tags).filter_by(tagid=tagid).all()

        for i in prebrew:
            result = encoders.encoder_recipes(i)
            recipe_list.append(result)
        for i in tag:
            result = encoders.encoder_tag(i)
            tag_list.append(result)

        brew_method = recipe_list[0].get('brew_method')
        grind_setting = recipe_list[0].get('grind_setting')
        brand = recipe_list[0].get('brand')
        roast = recipe_list[0].get('roast')
        bean_type = recipe_list[0].get('bean_type')
        coffee_weight = recipe_list[0].get('coffee_weight')

        inner_section = tag_list[0].get('inner_section')
        middle_section = tag_list[0].get('middle_section')
        outer_section = tag_list[0].get('outer_section')
        print(inner_section, middle_section, outer_section)

        Session.add(
            tables.brews(recipeid=recipeid, brew_method=brew_method, grind_setting=grind_setting, brand=brand, roast=roast,
                        bean_type=bean_type, coffee_weight=coffee_weight, userid=userid, ext_time=ext_time,
                        ext_weight=ext_weight, notes=notes, date=datetime.datetime.now(), tagid=tagid,
                        inner_section=inner_section, middle_section=middle_section, outer_section=outer_section,
                        brew_visibility=True))
        Session.commit()
        Session.flush()
        return jsonify("Recipe created"), 201

@app.route("/<userid>/brew list", methods=["POST", "GET"])
def get_brew_attempts(userid):
    result_list = []
    if request.method == "GET":
        attempt = Session.query(tables.brews).filter_by(userid=userid).all()

        for i in attempt:
            result = encoders.encoder_brews(i)
            result_list.append(result)
        return jsonify(Attempts=result_list), 200

@app.route("/<userid>/brew list/<brewid>", methods=["POST", "GET"])
def get_brew_attempt(userid, brewid):
    result_list = []
    if request.method == "GET":
        attempt = Session.query(tables.brews).filter_by(userid=userid, brewid=brewid).all()

        for i in attempt:
            result = encoders.encoder_brews(i)
            result_list.append(result)
        return jsonify(Attempts=result_list), 200

@app.route("/<userid>/search/<tagid>", methods=["POST", "GET"])
def search_brews_by_tag(userid, tagid):
    result_list = []
    if request.method == "GET":
        attempt = Session.query(tables.brews).filter_by(tagid=tagid).all()

        for i in attempt:
            result = encoders.encoder_brews(i)
            result_list.append(result)
        return jsonify(Attempts=result_list), 200

@app.route("/<userid>/search/flavor", methods=["POST", "GET"])
def search_brews_by_flavor(userid):
    flavor = request.form.get('flavor')
    result_list = []
    if request.method == "GET":
        inner = Session.query(tables.brews).order_by(tables.brews.date.desc()).filter_by(inner_section=flavor).\
            filter(tables.brews.userid != userid).all()
        middle = Session.query(tables.brews).order_by(tables.brews.date.desc()).filter_by(middle_section=flavor).\
            filter(tables.brews.userid != userid).all()
        outer = Session.query(tables.brews).order_by(tables.brews.date.desc()).filter_by(outer_section=flavor).\
            filter(tables.brews.userid != userid).all()

        for i in inner:
            result = encoders.encoder_brews(i)
            result_list.append(result)
        for i in middle:
            result = encoders.encoder_brews(i)
            result_list.append(result)
        for i in outer:
            result = encoders.encoder_brews(i)
            result_list.append(result)
        return jsonify(Attempts=result_list), 200

@app.route("/<userid>/search/<recipeid>/copy", methods=["POST", "GET"])
def copy_recipe(userid, recipeid):
    recipe_list = []
    if request.method == "POST":

        prebrew = Session.query(tables.recipes).filter_by(recipeid=recipeid,
                                                          recipe_visibility=True).all()
        for i in prebrew:
            result = encoders.encoder_recipes(i)
            recipe_list.append(result)
        brew_method = recipe_list[0].get('brew_method')
        grind_setting = recipe_list[0].get('grind_setting')
        brand = recipe_list[0].get('brand')
        roast = recipe_list[0].get('roast')
        bean_type = recipe_list[0].get('bean_type')
        coffee_weight = recipe_list[0].get('coffee_weight')

        Session.add(
            tables.recipes(brew_method=brew_method.lower(), grind_setting=grind_setting, brand=brand.lower(), roast=roast.lower(),
                  bean_type=bean_type.lower(), coffee_weight=coffee_weight, userid=userid, recipe_visibility=True))
        Session.commit()
        Session.flush()
        return jsonify("Recipe copied"), 201

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

@app.route("/<userid>/mokapot_scores", methods=["POST", "GET"])
def get_mokapot_scores(userid):
    result_list = []
    if request.method == "GET":
        mokapot = Session.query(tables.mokapot_scores).filter_by(userid=userid).all()

        for i in mokapot:
            result = encoders.encoder_score(i)
            result_list.append(result)
        return jsonify(Attempts=result_list), 200

@app.route("/<userid>/chemex_scores", methods=["POST", "GET"])
def get_chemex_scores(userid):
    result_list = []
    if request.method == "GET":
        chemex = Session.query(tables.chemex_scores).filter_by(userid=userid).all()

        for i in chemex:
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
        refillReservoirScore = request.form.get('refillReservoirScore')
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

@app.route("/<userid>/mokapot_simulation", methods=["POST", "GET"])
def new_mokapot_score(userid):
    if request.method == "POST":
        weightScore = request.form.get('weightScore')
        weightScoreTotal = request.form.get('weightScoreTotal')
        grindScore = request.form.get('grindScore')
        grindScoreTotal = request.form.get('grindScoreTotal')
        chooseWaterScore = request.form.get('chooseWaterScore')
        chooseWaterScoreTotal = request.form.get('chooseWaterScoreTotal')
        addCoffeeScore = request.form.get('addCoffeeScore')
        addCoffeeScoreTotal = request.form.get('addCoffeeScoreTotal')
        putTogetherScore = request.form.get('putTogetherScore')
        putTogetherScoreTotal = request.form.get('putTogetherScoreTotal')
        stoveScore = request.form.get('stoveScore')
        stoveScoreTotal = request.form.get('stoveScoreTotal')
        serveScore = request.form.get('serveScore')
        serveScoreTotal = request.form.get('serveScoreTotal')
        scoreTotal = int(weightScore) + int(chooseWaterScore) + int(addCoffeeScore) + int(grindScore) + \
                     int(putTogetherScore) + int(stoveScore) + int(serveScore)
        evalTotal = int(weightScoreTotal) + int(chooseWaterScoreTotal) + int(addCoffeeScoreTotal) + \
                    int(grindScoreTotal) + int(putTogetherScoreTotal) + int(stoveScoreTotal) + int(serveScoreTotal)
        grade = (scoreTotal/evalTotal)*100

        Session.add(
            tables.mokapot_scores(userid=userid, weightScore=weightScore, weightScoreTotal=weightScoreTotal,
            grindScore=grindScore, grindScoreTotal=grindScoreTotal, chooseWaterScore=chooseWaterScore,
            chooseWaterScoreTotal=chooseWaterScoreTotal, addCoffeeScore=addCoffeeScore,
            addCoffeeScoreTotal=addCoffeeScoreTotal, putTogetherScore=putTogetherScore,
            putTogetherScoreTotal=putTogetherScoreTotal, stoveScore=stoveScore, stoveScoreTotal=stoveScoreTotal,
            serveScore=serveScore, serveScoreTotal=serveScoreTotal, scoreTotal=scoreTotal, evalTotal=evalTotal,
            grade=grade, dateObtained=datetime.datetime.now()))
        Session.commit()
        Session.flush()
        return jsonify("Scores recorded"), 201

@app.route("/<userid>/chemex_simulation", methods=["POST", "GET"])
def new_chemex_score(userid):
    if request.method == "POST":
        weightScore = request.form.get('weightScore')
        weightScoreTotal = request.form.get('weightScoreTotal')
        grindScore = request.form.get('grindScore')
        grindScoreTotal = request.form.get('grindScoreTotal')
        wetGroundsScore = request.form.get('wetGroundsScore')
        wetGroundsScoreTotal = request.form.get('wetGroundsScoreTotal')
        addWaterScore = request.form.get('addWaterScore')
        addWaterScoreTotal = request.form.get('addWaterScoreTotal')
        serveScore = request.form.get('serveScore')
        serveScoreTotal = request.form.get('serveScoreTotal')
        scoreTotal = int(weightScore) + int(wetGroundsScore) + int(addWaterScore) + int(grindScore) + int(serveScore)
        evalTotal = int(weightScoreTotal) + int(wetGroundsScoreTotal) + int(addWaterScoreTotal) + \
                    int(grindScoreTotal) + int(serveScoreTotal)
        grade = (scoreTotal/evalTotal)*100

        Session.add(
            tables.chemex_scores(userid=userid, weightScore=weightScore, weightScoreTotal=weightScoreTotal,
            grindScore=grindScore, grindScoreTotal=grindScoreTotal, wetGroundsScore=wetGroundsScore,
            wetGroundsScoreTotal=wetGroundsScoreTotal, addWaterScore=addWaterScore, addWaterScoreTotal=addWaterScoreTotal,
            serveScore=serveScore, serveScoreTotal=serveScoreTotal, scoreTotal=scoreTotal, evalTotal=evalTotal,
            grade=grade, dateObtained=datetime.datetime.now()))
        Session.commit()
        Session.flush()
        return jsonify("Scores recorded"), 201

@app.route("/tags", methods=["POST", "GET"])
def get_tags():
    result_list = []
    if request.method == "GET":
        flavorWheel = Session.query(tables.tags).order_by(tables.tags.tagid).filter_by().all()

        for i in flavorWheel:
            result = encoders.encoder_tag(i)
            result_list.append(result)
        return jsonify(Attempts=result_list), 200

@app.route("/basic_tags", methods=["POST", "GET"])
def get_basic_tags():
    result_list = []
    if request.method == "GET":
        flavorWheel = Session.query(tables.tags).order_by(tables.tags.tagid).filter_by(middle_section='').all()

        for i in flavorWheel:
            result = encoders.encoder_tag(i)
            result_list.append(result)
        return jsonify(Attempts=result_list), 200

@app.route("/intermediate_tags", methods=["POST", "GET"])
def get_intermediate_tags():
    result_list = []
    if request.method == "GET":
        flavorWheel = Session.query(tables.tags).order_by(tables.tags.tagid).\
            filter(tables.tags.middle_section !='').\
            filter_by(outer_section='')
        for i in flavorWheel:
            result = encoders.encoder_tag(i)
            result_list.append(result)
        return jsonify(Attempts=result_list), 200

@app.route("/advanced_tags", methods=["POST", "GET"])
def get_advanced_tags():
    result_list = []
    if request.method == "GET":
        flavorWheel = Session.execute(text("select * "
                                      "from tags "
                                      "where outer_section != '' or middle_section in (select middle_section "
                                      "from tags "
                                      "group by middle_section "
                                      "having count(middle_section) = 1) "
                                      "order by tagid"))
        for i in flavorWheel:
            result = encoders.encoder_tag(i)
            print(result)
            result_list.append(result)
        return jsonify(Attempts=result_list), 200


if __name__ == '__main__':
    app.run(debug=True)

engine.dispose()
Session.close()