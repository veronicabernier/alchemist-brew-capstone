from sqlalchemy import create_engine, ForeignKey, Column, Text, Integer, Boolean, Date, Float, text
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


def prebrew_dict(row):
    result = {}
    result['recipeid'] = row[0]
    result['brew_method'] = row[1]
    result['grind_setting'] = row[2]
    result['brand'] = row[3]
    result['roast'] = row[4]
    result['bean_type'] = row[5]
    result['coffee_weight'] = row[6]
    result['userid'] = row[7]
    return result

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
    username = Column("username",Text)
    email = Column("email", Text)
    password = Column("password", Text)
    private_profile = Column("private_profile", Boolean)
    birth_date = Column("birth_date", Date)
    gender = Column("gender", Text)
    location = Column("location", Text)

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
    brew_method = Column("brew_method", Text)
    grind_setting = Column("grind_setting", Integer)
    brand = Column("brand", Text)
    roast = Column("roast", Text)
    bean_type = Column("bean_type", Text)
    coffee_weight = Column("coffee_weight", Float)
    userid = Column("userid", Integer)

    def __init__(self, brew_method, grind_setting, brand, roast, bean_type, coffee_weight, userid):
        self.brew_method = brew_method
        self.grind_setting = grind_setting
        self.brand = brand
        self.roast = roast
        self.bean_type = bean_type
        self.coffee_weight = coffee_weight
        self.userid = userid

    def __repr__(self):
        return f"({self.recipeid} {self.brew_method} {self.grind_setting} {self.brand}" \
               f" {self.roast} {self.bean_type} {self.coffee_weight} {self.userid})"

Base.metadata.create_all(engine)
# Session = get_session()

engine = Session.get_bind()


@app.route("/signup", methods=['POST', 'GET'])
def register():
    if request.method == "POST":
        username = request.form.get('username'),
        email = request.form.get('email'),
        password = request.form.get("password"),
        confirm = request.form.get("confirm"),
        birth_date = request.form.get("birth_date"),
        gender = request.form.get("gender"),
        location = request.form.get("location")

        emaildata = Session.query(users).filter_by(email=email).first()
        print(emaildata)
        print(username, email, password, confirm, birth_date, gender, location)
        if emaildata == None:
            if password == confirm:
                Session.add(users(username=username, email=email, password=password, private_profile=True, birth_date=birth_date, gender=gender, location=location))
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

# def get_user_password(email):
#     passworddata = Session.query(Users.password).filter_by(email=email).first()
#     return passworddata[0]
#
# result=get_user_password("roberto.agosto@upr.edu")
# print(result)

@app.route("/login", methods=["POST", "GET"])
def login():
    if request.method == "POST":
        email = request.args.get('email')
        password = request.args.get("password")

        emaildata = Session.query(users.email).filter_by(email=email).first()
        passworddata = Session.query(users.password).filter_by(email=email).first()

        print(emaildata)
        print(passworddata)
        print(email, password)

        if emaildata is None:

            return jsonify(Error="User does not exist"), 400
        else:
            if (passworddata is None):
                return jsonify(Error="Incorrect password"), 400
            elif (passworddata[0] == password):
                return jsonify(Error="Succesfull login"), 400  # to be edited from here do redict to either svm or home

    return jsonify(Error="User or password is incorrect"), 400

def encoder_recipes(recipe):
    if isinstance(recipe, recipes):
        return {'recipeid': recipe.recipeid, 'brew_method': recipe.brew_method, 'grind_setting': recipe.grind_setting, 'brand': recipe.brand,
            'roast': recipe.roast, 'bean_type': recipe.bean_type, 'coffee_weight': recipe.coffee_weight, 'userid': recipe.userid}
    raise TypeError(f'Object{recipe} is not of type recipes.')

@app.route("/email/recipe list", methods=["POST", "GET"])
def get_recipes():
    result_list = []
    if request.method == "GET":
        userid = request.form.get('userid')
        prebrew = Session.query(recipes).filter_by(userid=userid).all()

        for i in prebrew:
            result=encoder_recipes(i)
            result_list.append(result)
        return jsonify(Recepies=result_list), 200

@app.route("/email/recipe list/add", methods=["POST", "GET"])
def add_recipe():
    if request.method == "POST":
        brew_method = request.form.get('brew_method')
        grind_setting = request.form.get('grind_setting')
        brand = request.form.get('brand')
        roast = request.form.get('roast')
        bean_type = request.form.get('bean_type')
        coffee_weight = request.form.get('coffee_weight')
        userid = request.form.get('userid')

        Session.add(
            recipes(brew_method=brew_method, grind_setting=grind_setting, brand=brand, roast=roast,
                  bean_type=bean_type, coffee_weight=coffee_weight, userid=userid, ))
        Session.commit()
        Session.flush()
        return jsonify("Recipe created"), 201

if __name__ == '__main__':
    app.run(debug=True)

engine.dispose()
Session.close()