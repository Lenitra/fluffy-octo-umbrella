from general import HERE
from __main__ import app
import json
from flask import jsonify, request, session
import datetime


@app.route("/leaderboard/money")
def leaderboard():
    with open(HERE + "/data/users.json") as f:
        users = json.load(f)

    data = []
    for k, v in users.items():
        data.append({"username": k, "money": v["money"]})

    data = sorted(data, key=lambda x: x["money"], reverse=True)
    return jsonify(data)

@app.route("/leaderboard/territory")
def leaderboard2():
    toret = {}
    with open(HERE + "/data/map.json") as f:
        map = json.load(f)

    for tile in map.values():
        if tile["owner"] == "":
            continue
        try:
            toret[tile["owner"]] += 1
        except:
            toret[tile["owner"]] = 1
        
    toret = sorted(toret.items(), key=lambda x: x[1], reverse=True)


    return jsonify(toret)


# récupère le nombre d'unités possédées par les 10 premiers
@app.route("/leaderboard/units")
def leaderboard3():
    toret = {}
    with open(HERE + "/data/map.json") as f:
        map = json.load(f)

    for tile in map.values():
        if tile["owner"] == "":
            continue
        try:
            toret[tile["owner"]] += tile["units"]
        except:
            toret[tile["owner"]] = tile["units"]

    toret = sorted(toret.items(), key=lambda x: x[1], reverse=True)
    return jsonify(toret)


# récupère les 10 premiers de chaque leaderboard
@app.route("/leaderboard/all")
def leaderboard4():
    with open(HERE + "/data/users.json") as f:
        users = json.load(f)

    data = {"money": {}, "territory": {}, "units": {}}
    for k, v in users.items():
        data["money"][k] = v["money"]
        data["territory"][k] = 0
        data["units"][k] = 0

    with open(HERE + "/data/map.json") as f:
        map = json.load(f)
    
    for tile in map.values():
        if tile["owner"] == "":
            continue
        data["territory"][tile["owner"]] += 1
        data["units"][tile["owner"]] += tile["units"]

    data["money"] = sorted(data["money"].items(), key=lambda x: x[1], reverse=True)[:10]
    data["territory"] = sorted(data["territory"].items(), key=lambda x: x[1], reverse=True)[:10]
    data["units"] = sorted(data["units"].items(), key=lambda x: x[1], reverse=True)[:10]

    return jsonify(data)