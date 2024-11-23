from general import HERE
from __main__ import app
import json
from flask import jsonify, request, session
import datetime


@app.route("/leaderboard", methods=["GET"])
def leaderboard():
    with open(HERE + "/data/users.json") as f:
        users = json.load(f)

    data = []
    for k, v in users.items():
        data.append({"username": k, "money": v["money"]})

    data = sorted(data, key=lambda x: x["money"], reverse=True)
    return jsonify(data)