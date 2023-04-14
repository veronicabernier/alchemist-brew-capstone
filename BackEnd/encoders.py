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

def encoder_recipes(recipe):
    if isinstance(recipe, tables.recipes):
        return {'recipeid': recipe.recipeid, 'brew_method': recipe.brew_method, 'grind_setting': recipe.grind_setting,
                'brand': recipe.brand, 'roast': recipe.roast, 'bean_type': recipe.bean_type,
                'coffee_weight': recipe.coffee_weight, 'userid': recipe.userid, 'recipe_visibility': recipe.recipe_visibility}
    raise TypeError(f'Object{recipe} is not of type recipes.')

def encoder_brews(brew):
    if isinstance(brew, tables.brews):
        return {'brewid': brew.brewid, 'recipeid': brew.recipeid, 'brew_method': brew.brew_method,
                'grind_setting': brew.grind_setting, 'brand': brew.brand, 'roast': brew.roast,
                'bean_type': brew.bean_type, 'coffee_weight': brew.coffee_weight, 'userid': brew.userid,
                'ext_time': brew.ext_time, 'ext_weight': brew.ext_weight, 'flavor': brew.flavor, 'date': brew.date,
                'brew_visibility': brew.brew_visibility}
    raise TypeError(f'Object{brew} is not of type recipes.')

def encoder_score(record):
    if isinstance(record, tables.espresso_scores):
        return {'espreso_scoreid': record.espreso_scoreid, 'userid': record.userid, 'weightScore': record.weightScore,
                'weightScoreTotal': record.weightScoreTotal, 'reservoirScore': record.reservoirScore, 'reservoirScoreTotal': record.reservoirScoreTotal,
                'powerOnScore': record.powerOnScore, 'powerOnScoreTotal': record.powerOnScoreTotal, 'grindScore': record.grindScore,
                'grindScoreTotal': record.grindScoreTotal, 'tampScore': record.tampScore, 'tampScoreTotal': record.tampScoreTotal, 'brewScore': record.brewScore,
                'brewScoreTotal': record.brewScoreTotal, 'serveScore': record.serveScore, 'serveScoreTotal': record.serveScoreTotal,
                'scoreTotal': record.scoreTotal, 'evalTotal': record.evalTotal, 'grade': record.grade, 'dateObtained': record.dateObtained}

    if isinstance(record, tables.drip_scores):
        return {'drip_scoreid': record.drip_scoreid, 'userid': record.userid, 'weightScore': record.weightScore,
                'weightScoreTotal': record.weightScoreTotal, 'reservoirScore': record.reservoirScore,
                'reservoirScoreTotal': record.reservoirScoreTotal, 'grindScore': record.grindScore,
                'grindScoreTotal': record.grindScoreTotal,
                'chooseFilterScore': record.chooseFilterScore,
                'chooseFilterScoreTotal': record.chooseFilterScoreTotal,
                'refillReservoirScore': record.refillReservoirScore,
                'refillReservoirScoreTotal': record.refillReservoirScoreTotal,
                'brewScore': record.brewScore, 'brewScoreTotal': record.brewScoreTotal,
                'serveScore': record.serveScore, 'serveScoreTotal': record.serveScoreTotal,
                'scoreTotal': record.scoreTotal, 'evalTotal': record.evalTotal, 'grade': record.grade,
                'dateObtained': record.dateObtained}

    if isinstance(record, tables.mokapot_scores):
        return {'mokapot_scoreid': record.mokapot_scoreid, 'userid': record.userid, 'weightScore': record.weightScore,
                'weightScoreTotal': record.weightScoreTotal, 'grindScore': record.grindScore,
                'grindScoreTotal': record.grindScoreTotal,  'chooseWaterScore': record.chooseWaterScore,
                'chooseWaterScoreTotal': record.chooseWaterScoreTotal,
                'addCoffeeScore': record.addCoffeeScore,
                'addCoffeeScoreTotal': record.addCoffeeScoreTotal,
                'putTogetherScore': record.putTogetherScore,
                'putTogetherScoreTotal': record.putTogetherScoreTotal,
                'stoveScore': record.stoveScore, 'stoveScoreTotal': record.stoveScoreTotal,
                'serveScore': record.serveScore, 'serveScoreTotal': record.serveScoreTotal,
                'scoreTotal': record.scoreTotal, 'evalTotal': record.evalTotal, 'grade': record.grade,
                'dateObtained': record.dateObtained}

    if isinstance(record, tables.chemex_scores):
        return {'chemex_scoreid': record.chemex_scoreid, 'userid': record.userid, 'weightScore': record.weightScore,
                'weightScoreTotal': record.weightScoreTotal, 'grindScore': record.grindScore,
                'grindScoreTotal': record.grindScoreTotal,  'wetGroundsScore': record.wetGroundsScore,
                'wetGroundsScoreTotal': record.wetGroundsScoreTotal,
                'addWaterScore': record.addWaterScore,
                'addWaterScoreTotal': record.addWaterScoreTotal,
                'serveScore': record.serveScore, 'serveScoreTotal': record.serveScoreTotal,
                'scoreTotal': record.scoreTotal, 'evalTotal': record.evalTotal, 'grade': record.grade,
                'dateObtained': record.dateObtained}

    raise TypeError(f'Object{record} is not any type to score.')

# def encoder_drip_score(record):
#     if isinstance(record, tables.drip_scores):
#         return {'drip_scoreid': record.drip_scoreid, 'userid': record.userid, 'weightScore': record.weightScore,
#                 'weightScoreTotal': record.weightScoreTotal, 'reservoirScore': record.reservoirScore,
#                 'reservoirScoreTotal': record.reservoirScoreTotal, 'grindScore': record.grindScore,
#                 'grindScoreTotal': record.grindScoreTotal,
#                 'chooseFilterScore': record.chooseFilterScore,
#                 'chooseFilterScoreTotal': record.chooseFilterScoreTotal,
#                 'refillReservoirScore': record.refillReservoirScore,
#                 'refillReservoirScoreTotal': record.refillReservoirScoreTotal,
#                 'brewScore': record.brewScore, 'brewScoreTotal': record.brewScoreTotal,
#                 'serveScore': record.serveScore, 'serveScoreTotal': record.serveScoreTotal,
#                 'scoreTotal': record.scoreTotal, 'evalTotal': record.evalTotal, 'grade': record.grade,
#                 'dateObtained': record.dateObtained}
#     raise TypeError(f'Object{record} is not any type to score.')