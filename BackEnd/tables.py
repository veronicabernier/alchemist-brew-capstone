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
import app

Base = declarative_base()

# def get_engine(user, passwd, host, port, db):
#     url = f"postgresql://{user}:{passwd}@{host}:{port}/{db}"
#     if not database_exists(url):
#         create_database(url)
#     engine = create_engine(url, pool_size=50, echo=False)
#     return engine
#
# engine = get_engine(settings['User'],
#           settings['Password'],
#           settings['Host'],
#           settings['Port'],
#           settings['Database'])
#
#
# def get_engine_from_settings():
#     keys = ['User', 'Password', 'Host', 'Port', 'Database']
#     if not all(key in keys for key in settings.keys()):
#         raise Exception('Bad config file')
#
#     return get_engine(settings['User'],
#           settings['Password'],
#           settings['Host'],
#           settings['Port'],
#           settings['Database'])
#
# def get_session():
#     engine = get_engine_from_settings()
#     session = sessionmaker(bind=engine)()
#     return session
#
# Session = get_session()
#
# engine = Session.get_bind()
# # Base.metadata.create_all(engine)
appEngine = app.engine



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
    recipeid = Column("recipeid", Integer, ForeignKey("recipes.recipeid"), nullable=False)
    userid = Column("userid", Integer, nullable=False)
    brew_method = Column("brew_method", String)
    grind_setting = Column("grind_setting", Float)
    brand = Column("brand", String)
    roast = Column("roast", String)
    bean_type = Column("bean_type", String)
    coffee_weight = Column("coffee_weight", Float)
    # recipe_visibility = Column("recipe_visibility", Boolean)
    ext_time = Column("ext_time", Integer)
    ext_weight = Column("ext_weight", Float)
    notes = Column("notes", String)
    date = Column("date", DateTime)
    tagid = Column("tagid", Integer)
    inner_section = Column("inner_section", String)
    middle_section = Column("middle_section", String)
    outer_section = Column("outer_section", String)
    brew_visibility = Column("brew_visibility", Boolean)

    def __init__(self, recipeid, userid, brew_method, grind_setting, brand, roast, bean_type, coffee_weight,
                 ext_time, ext_weight, notes, date, tagid,  inner_section, middle_section,
                 outer_section, brew_visibility):
        self.recipeid = recipeid
        self.userid = userid
        self.brew_method = brew_method
        self.grind_setting = grind_setting
        self.brand = brand
        self.roast = roast
        self.bean_type = bean_type
        self.coffee_weight = coffee_weight
        # self.recipe_visibility = recipe_visibility
        self.ext_time = ext_time
        self.ext_weight = ext_weight
        self.notes = notes
        self.date = date
        self.tagid = tagid
        self.inner_section = inner_section
        self.middle_section = middle_section
        self.outer_section = outer_section
        self.brew_visibility = brew_visibility

    def __repr__(self):
        return f"({self.recipeid} {self.userid}  {self.brew_method} {self.grind_setting} {self.brand}" \
               f" {self.roast} {self.bean_type} {self.coffee_weight} {self.ext_time} {self.ext_weight} " \
               f"{self.notes} {self.date} {self.tagid} {self.brew_visibility})"

class espresso_scores(Base):
    __tablename__ = "espresso_scores"
    espresso_scoreid = Column("espresso_scoreid", Integer, autoincrement=True, primary_key=True)
    userid = Column("userid", Integer)
    weightScore = Column("weightScore", Integer)
    weightScoreTotal = Column("weightScoreTotal", Integer)
    reservoirScore = Column("reservoirScore", Integer)
    reservoirScoreTotal = Column("reservoirScoreTotal", Integer)
    powerOnScore = Column("powerOnScore", Integer)
    powerOnScoreTotal = Column("powerOnScoreTotal", Integer)
    grindScore = Column("grindScore", Integer)
    grindScoreTotal = Column("grindScoreTotal", Integer)
    tampScore = Column("tampScore", Integer)
    tampScoreTotal = Column("tampScoreTotal", Integer)
    brewScore = Column("brewScore", Integer)
    brewScoreTotal = Column("brewScoreTotal", Integer)
    serveScore = Column("serveScore", Integer)
    serveScoreTotal = Column("serveScoreTotal", Integer)
    scoreTotal = Column("scoreTotal", Integer)
    evalTotal = Column("evalTotal", Integer)
    grade = Column("grade", Integer)
    dateObtained = Column("dateObtained", DateTime)


    def __init__(self, userid, weightScore, weightScoreTotal, reservoirScore, reservoirScoreTotal,
                 powerOnScore, powerOnScoreTotal, grindScore, grindScoreTotal, tampScore, tampScoreTotal,
                 brewScore, brewScoreTotal, serveScore, serveScoreTotal, scoreTotal, evalTotal, grade, dateObtained):
        self.userid = userid
        self.weightScore = weightScore
        self.weightScoreTotal = weightScoreTotal
        self.reservoirScore = reservoirScore
        self.reservoirScoreTotal = reservoirScoreTotal
        self.powerOnScore = powerOnScore
        self.powerOnScoreTotal = powerOnScoreTotal
        self.grindScore = grindScore
        self.grindScoreTotal = grindScoreTotal
        self.tampScore = tampScore
        self.tampScoreTotal = tampScoreTotal
        self.brewScore = brewScore
        self.brewScoreTotal = brewScoreTotal
        self.serveScore = serveScore
        self.serveScoreTotal = serveScoreTotal
        self.scoreTotal = scoreTotal
        self.evalTotal = evalTotal
        self.grade = grade
        self.dateObtained = dateObtained

    def __repr__(self):
        return f"({self.userid} {self.weightScore} {self.weightScoreTotal} {self.reservoirScore} {self.reservoirScoreTotal}" \
               f" {self.powerOnScore} {self.powerOnScoreTotal} {self.grindScore} {self.grindScoreTotal} " \
               f"{self.tampScore} {self.tampScoreTotal} {self.brewScore} {self.brewScoreTotal} " \
               f"{self.serveScore} {self.serveScoreTotal} {self.scoreTotal} {self.evalTotal} {self.grade} {self.dateObtained})"

class drip_scores(Base):
    __tablename__ = "drip_scores"
    drip_scoreid = Column("drip_scoreid", Integer, autoincrement=True, primary_key=True)
    userid = Column("userid", Integer)
    weightScore = Column("weightScore", Integer)
    weightScoreTotal = Column("weightScoreTotal", Integer)
    reservoirScore = Column("reservoirScore", Integer)
    reservoirScoreTotal = Column("reservoirScoreTotal", Integer)
    grindScore = Column("grindScore", Integer)
    grindScoreTotal = Column("grindScoreTotal", Integer)
    chooseFilterScore = Column("chooseFilterScore", Integer)
    chooseFilterScoreTotal = Column("chooseFilterScoreTotal", Integer)
    refillReservoirScore = Column("refillReservoirScore", Integer)
    refillReservoirScoreTotal = Column("refillReservoirScoreTotal", Integer)
    brewScore = Column("brewScore", Integer)
    brewScoreTotal = Column("brewScoreTotal", Integer)
    serveScore = Column("serveScore", Integer)
    serveScoreTotal = Column("serveScoreTotal", Integer)
    scoreTotal = Column("scoreTotal", Integer)
    evalTotal = Column("evalTotal", Integer)
    grade = Column("grade", Integer)
    dateObtained = Column("dateObtained", DateTime)


    def __init__(self, userid, weightScore, weightScoreTotal, reservoirScore, reservoirScoreTotal,
                 grindScore, grindScoreTotal, chooseFilterScore, chooseFilterScoreTotal,
                 refillReservoirScore, refillReservoirScoreTotal, brewScore, brewScoreTotal,
                 serveScore, serveScoreTotal, scoreTotal, evalTotal, grade, dateObtained):
        self.userid = userid
        self.weightScore = weightScore
        self.weightScoreTotal = weightScoreTotal
        self.reservoirScore = reservoirScore
        self.reservoirScoreTotal = reservoirScoreTotal
        self.grindScore = grindScore
        self.grindScoreTotal = grindScoreTotal
        self.chooseFilterScore = chooseFilterScore
        self.chooseFilterScoreTotal = chooseFilterScoreTotal
        self.refillReservoirScore = refillReservoirScore
        self.refillReservoirScoreTotal = refillReservoirScoreTotal
        self.brewScore = brewScore
        self.brewScoreTotal = brewScoreTotal
        self.serveScore = serveScore
        self.serveScoreTotal = serveScoreTotal
        self.scoreTotal = scoreTotal
        self.evalTotal = evalTotal
        self.grade = grade
        self.dateObtained = dateObtained

    def __repr__(self):
        return f"({self.userid} {self.weightScore} {self.weightScoreTotal} {self.reservoirScore} {self.reservoirScoreTotal}" \
               f"{self.grindScore} {self.grindScoreTotal} {self.chooseFilterScore} {self.chooseFilterScoreTotal}" \
               f"{self.refillReservoirScore} {self.refillReservoirScoreTotal} {self.brewScore} {self.brewScoreTotal} " \
               f"{self.serveScore} {self.serveScoreTotal} {self.scoreTotal} {self.evalTotal} {self.grade} {self.dateObtained})"

class mokapot_scores(Base):
    __tablename__ = "mokapot_scores"
    mokapot_scoreid = Column("mokapot_scoreid", Integer, autoincrement=True, primary_key=True)
    userid = Column("userid", Integer)
    weightScore = Column("weightScore", Integer)
    weightScoreTotal = Column("weightScoreTotal", Integer)
    grindScore = Column("grindScore", Integer)
    grindScoreTotal = Column("grindScoreTotal", Integer)
    chooseWaterScore = Column("chooseWaterScore", Integer)
    chooseWaterScoreTotal = Column("chooseWaterScoreTotal", Integer)
    addCoffeeScore = Column("addCoffeeScore", Integer)
    addCoffeeScoreTotal = Column("addCoffeeScoreTotal", Integer)
    putTogetherScore = Column("putTogetherScore", Integer)
    putTogetherScoreTotal = Column("putTogetherScoreTotal", Integer)
    stoveScore = Column("stoveScore", Integer)
    stoveScoreTotal = Column("stoveScoreTotal", Integer)
    serveScore = Column("serveScore", Integer)
    serveScoreTotal = Column("serveScoreTotal", Integer)
    scoreTotal = Column("scoreTotal", Integer)
    evalTotal = Column("evalTotal", Integer)
    grade = Column("grade", Integer)
    dateObtained = Column("dateObtained", DateTime)


    def __init__(self, userid, weightScore, weightScoreTotal,grindScore, grindScoreTotal,
                 chooseWaterScore, chooseWaterScoreTotal, addCoffeeScore, addCoffeeScoreTotal,
                 putTogetherScore, putTogetherScoreTotal, stoveScore, stoveScoreTotal,
                 serveScore, serveScoreTotal, scoreTotal, evalTotal, grade, dateObtained):
        self.userid = userid
        self.weightScore = weightScore
        self.weightScoreTotal = weightScoreTotal
        self.grindScore = grindScore
        self.grindScoreTotal = grindScoreTotal
        self.chooseWaterScore = chooseWaterScore
        self.chooseWaterScoreTotal = chooseWaterScoreTotal
        self.addCoffeeScore = addCoffeeScore
        self.addCoffeeScoreTotal = addCoffeeScoreTotal
        self.putTogetherScore = putTogetherScore
        self.putTogetherScoreTotal = putTogetherScoreTotal
        self.stoveScore = stoveScore
        self.stoveScoreTotal = stoveScoreTotal
        self.serveScore = serveScore
        self.serveScoreTotal = serveScoreTotal
        self.scoreTotal = scoreTotal
        self.evalTotal = evalTotal
        self.grade = grade
        self.dateObtained = dateObtained

    def __repr__(self):
        return f"({self.userid} {self.weightScore} {self.weightScoreTotal} {self.grindScore} {self.grindScoreTotal}"\
               f"{self.chooseWaterScore} {self.chooseWaterScoreTotal} {self.addCoffeeScore} {self.addCoffeeScoreTotal}"\
               f"{self.putTogetherScore} {self.putTogetherScoreTotal} {self.stoveScore} {self.stoveScoreTotal}"\
               f"{self.serveScore} {self.serveScoreTotal} {self.scoreTotal} {self.evalTotal} {self.grade} {self.dateObtained})"

class chemex_scores(Base):
    __tablename__ = "chemex_scores"
    chemex_scoreid = Column("chemex_scoreid", Integer, autoincrement=True, primary_key=True)
    userid = Column("userid", Integer)
    weightScore = Column("weightScore", Integer)
    weightScoreTotal = Column("weightScoreTotal", Integer)
    grindScore = Column("grindScore", Integer)
    grindScoreTotal = Column("grindScoreTotal", Integer)
    wetGroundsScore = Column("wetGroundsScore", Integer)
    wetGroundsScoreTotal = Column("wetGroundsScoreTotal", Integer)
    addWaterScore = Column("addWaterScore", Integer)
    addWaterScoreTotal = Column("addWaterScoreTotal", Integer)
    serveScore = Column("serveScore", Integer)
    serveScoreTotal = Column("serveScoreTotal", Integer)
    scoreTotal = Column("scoreTotal", Integer)
    evalTotal = Column("evalTotal", Integer)
    grade = Column("grade", Integer)
    dateObtained = Column("dateObtained", DateTime)


    def __init__(self, userid, weightScore, weightScoreTotal,grindScore, grindScoreTotal,
                 wetGroundsScore, wetGroundsScoreTotal, addWaterScore, addWaterScoreTotal,
                 serveScore, serveScoreTotal, scoreTotal, evalTotal, grade, dateObtained):
        self.userid = userid
        self.weightScore = weightScore
        self.weightScoreTotal = weightScoreTotal
        self.grindScore = grindScore
        self.grindScoreTotal = grindScoreTotal
        self.wetGroundsScore = wetGroundsScore
        self.wetGroundsScoreTotal = wetGroundsScoreTotal
        self.addWaterScore = addWaterScore
        self.addWaterScoreTotal = addWaterScoreTotal
        self.serveScore = serveScore
        self.serveScoreTotal = serveScoreTotal
        self.scoreTotal = scoreTotal
        self.evalTotal = evalTotal
        self.grade = grade
        self.dateObtained = dateObtained

    def __repr__(self):
        return f"({self.userid} {self.weightScore} {self.weightScoreTotal} {self.grindScore} {self.grindScoreTotal}"\
               f"{self.wetGroundsScore} {self.wetGroundsScoreTotal} {self.addWaterScore} {self.addWaterScoreTotal}"\
               f"{self.serveScore} {self.serveScoreTotal} {self.scoreTotal} {self.evalTotal} {self.grade} {self.dateObtained})"

class tags(Base):
    __tablename__ = "tags"
    tagid = Column("tagid", Integer, autoincrement=True, primary_key=True)
    inner_section = Column("inner_section", String)
    middle_section = Column("middle_section", String)
    outer_section = Column("outer_section", String)
    def __init__(self, inner_section, middle_section, outer_section):
        self.inner_section = inner_section
        self.middle_section = middle_section
        self.outer_section = outer_section
    def __repr__(self):
        return f"({self.inner_section} {self.middle_section} {self.outer_section})"

class tagsbackup(Base):
    __tablename__ = "tagsbackup"
    tagid = Column("tagid", Integer, autoincrement=True, primary_key=True)
    inner_section = Column("inner_section", String)
    middle_section = Column("middle_section", String)
    outer_section = Column("outer_section", String)
    def __init__(self, inner_section, middle_section, outer_section):
        self.inner_section = inner_section
        self.middle_section = middle_section
        self.outer_section = outer_section
    def __repr__(self):
        return f"({self.inner_section} {self.middle_section} {self.outer_section})"

class confirmations(Base):
    __tablename__ = "confirmations"
    userid = Column("userid", Integer, primary_key=True)
    email = Column("email", String)
    confirmation_code = Column("confirmation_code", String)
    confirmation = Column("confirmation", Boolean)

    def __init__(self, userid, email, confirmation_code, confirmation):
        self.userid = userid
        self.email = email
        self.confirmation_code = confirmation_code
        self.confirmation = confirmation

    def __repr__(self):
        return f"({self.userid} {self.email} {self.confirmation_code} {self.confirmation} )"

Base.metadata.create_all(appEngine)
