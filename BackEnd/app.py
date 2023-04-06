import datetime
import bcrypt
from sqlalchemy import create_engine, ForeignKey, Column, Text, Integer, Boolean, Date, Float, DateTime, String, DateTime
from sqlalchemy_utils import database_exists, create_database
from sqlalchemy.orm import sessionmaker, declarative_base
from BackEnd.config.dbconfig import pg_config as settings
from flask import Flask, request
from flask_cors import CORS
from flask import jsonify
import json



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
class users(Base):
    __tablename__ = "users"
    userid = Column("userid", Integer, autoincrement=True, primary_key=True)
    username = Column("username", String)
    email = Column("email", String)
    password = Column("password", String)
    private_profile = Column("private_profile", Boolean)
    birth_date = Column("birth_date", Date)
    gender = Column("gender", String)
    location = Column("location", String)

    def __init__(self, username, email, password, private_profile, birth_date, gender, location):
        self.username = username
        self.email = email
        self.password = password
        self.private_profile = private_profile
        self.birth_date = birth_date
        self.gender = gender
        self.location = location

    def __repr__(self):
        return f"({self.username} {self.email} {self.password}" \
               f" {self.private_profile} {self.birth_date} {self.gender} {self.location})"

class recipes(Base):
    __tablename__ = "recipes"
    recipeid = Column("recipeid", Integer, autoincrement=True, primary_key=True)
    brew_method = Column("brew_method", String)
    grind_setting = Column("grind_setting", Float)
    brand = Column("brand", String)
    roast = Column("roast", String)
    bean_type = Column("bean_type", String)
    coffee_weight = Column("coffee_weight", Float)
    userid = Column("userid", Integer, ForeignKey("users.userid"), nullable=False)
    recipe_visibility = Column("recipe_visibility", Boolean)

    def __init__(self, brew_method, grind_setting, brand, roast, bean_type, coffee_weight, userid, recipe_visibility):
        self.brew_method = brew_method
        self.grind_setting = grind_setting
        self.brand = brand
        self.roast = roast
        self.bean_type = bean_type
        self.coffee_weight = coffee_weight
        self.userid = userid
        self.recipe_visibility = recipe_visibility

    def __repr__(self):
        return f"({self.recipeid} {self.brew_method} {self.grind_setting} {self.brand}" \
               f" {self.roast} {self.bean_type} {self.coffee_weight} {self.userid} {self.recipe_visibility})"

class brews(Base):
    __tablename__ = "brews"
    brewid = Column("brewid", Integer, autoincrement=True, primary_key=True)
    ext_time = Column("ext_time", Integer)
    ext_weight = Column("ext_weight", Float)
    flavor = Column("flavor", String)
    date = Column("date", DateTime)
    recipeid = Column("recipeid", Integer, ForeignKey("recipes.recipeid"), nullable=False)
    brew_visibility = Column("brew_visibility", Boolean)

    def __init__(self, ext_time, ext_weight, flavor, date, recipeid, brew_visibility):
        self.ext_time = ext_time
        self.ext_weight = ext_weight
        self.flavor = flavor
        self.date = date
        self.recipeid = recipeid
        self.brew_visibility = brew_visibility

    def __repr__(self):
        return f"({self.ext_time} {self.ext_weight} {self.flavor} {self.date} {self.recipeid} {self.brew_visibility})"

Base.metadata.create_all(engine)
# Session = get_session()

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

        emaildata = Session.query(users).filter_by(email=email).first()
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
                Session.add(users(username=username, email=email, password=storedpass, private_profile=True, birth_date=birth_date, gender=gender, location=location))
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

        emaildata = Session.query(users.email).filter_by(email=email).first()
        passworddata = Session.query(users.password).filter_by(email=email).first()

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

def encoder_recipes(recipe):
    if isinstance(recipe, recipes):
        return {'recipeid': recipe.recipeid, 'brew_method': recipe.brew_method, 'grind_setting': recipe.grind_setting,
                'brand': recipe.brand, 'roast': recipe.roast, 'bean_type': recipe.bean_type,
                'coffee_weight': recipe.coffee_weight, 'userid': recipe.userid, 'recipe_visibility': recipe.recipe_visibility}
    raise TypeError(f'Object{recipe} is not of type recipes.')

def encoder_brews(brew):
    if isinstance(brew, brews):
        return {'brewid': brew.brewid, 'ext_time': brew.ext_time, 'ext_weight': brew.ext_weight, 'flavor': brew.flavor,
            'date': brew.date, 'recipeid': brew.recipeid, 'brew_visibility': brew.brew_visibility}
    raise TypeError(f'Object{brew} is not of type recipes.')

@app.route("/<userid>/recipe list", methods=["POST", "GET"])
def get_all_recipes(userid):
    result_list = []
    if request.method == "GET":
        prebrew = Session.query(recipes).filter_by(userid=userid).all()

        for i in prebrew:
            result=encoder_recipes(i)
            result_list.append(result)
        return jsonify(Recepies=result_list), 200
@app.route("/<userid>/recipe list/<recipeid>", methods=["POST", "GET"])
def get_recipe(userid, recipeid):
    result_list = []
    if request.method == "GET":
        prebrew = Session.query(recipes).filter_by(userid=userid, recipeid=recipeid).all()

        for i in prebrew:
            result=encoder_recipes(i)
            result_list.append(result)
        return jsonify(Recipe=result_list), 200
@app.route("/<userid>/recipe list/<recipeid>", methods=["POST", "GET"])
def edit_recipe(userid, recipeid):
    result_list = []
    if request.method == "GET":
        prebrew = Session.query(recipes).filter_by(userid=userid, recipeid=recipeid).all()

        for i in prebrew:
            result=encoder_recipes(i)
            result_list.append(result)
        return jsonify(Recipe=result_list), 200

@app.route("/<userid>/recipe list/add", methods=["POST", "GET"])
def add_recipe(userid):
    if request.method == "POST":
        brew_method = request.form.get('brew_method')
        grind_setting = request.form.get('grind_setting')
        brand = request.form.get('brand')
        roast = request.form.get('roast')
        bean_type = request.form.get('bean_type')
        coffee_weight = request.form.get('coffee_weight')

        Session.add(
            recipes(brew_method=brew_method, grind_setting=grind_setting, brand=brand, roast=roast,
                  bean_type=bean_type, coffee_weight=coffee_weight, userid=userid, recipe_visibility=True))
        Session.commit()
        Session.flush()
        return jsonify("Recipe created"), 201

@app.route("/<userid>/<recipeid>/add", methods=["POST", "GET"])
def new_brew(userid, recipeid):
    if request.method == "POST":
        ext_time = request.form.get('ext_time')
        ext_weight = request.form.get('ext_weight')
        flavor = request.form.get('flavor')

        Session.add(
            brews(ext_time=ext_time, ext_weight=ext_weight, flavor=flavor,
                  date=datetime.datetime.now(), recipeid=recipeid, brew_visibility=True))
        Session.commit()
        Session.flush()
        return jsonify("Recipe created"), 201

@app.route("/<userid>/brew list", methods=["POST", "GET"])
def get_brew_attempt(userid):
    result_list = []
    if request.method == "GET":
        attempt = Session.query(recipes, brews).join(recipes, recipes.recipeid == brews.recipeid).filter_by(userid=userid).all()

        #i iterates over the whole attempts
        #[0] indicate values from the recipes table
        #[1} indicate values from the brews table
        for i in attempt:
            result = {'recipeid': i[0].recipeid, 'brew_method': i[0].brew_method, 'grind_setting': i[0].grind_setting,
            'brand': i[0].brand, 'roast': i[0].roast, 'bean_type': i[0].bean_type, 'coffee_weight': i[0].coffee_weight,
            'userid': i[0].userid, 'recipe_visibility': i[0].recipe_visibility, 'brewid': i[1].brewid,
            'ext_time': i[1].ext_time, 'ext_weight': i[1].ext_weight, 'flavor': i[1].flavor, 'date': i[1].date,
            'brew_visibility': i[1].brew_visibility}
            result_list.append(result)
        return jsonify(Attempts=result_list), 200


if __name__ == '__main__':
    app.run(debug=True)

engine.dispose()
Session.close()