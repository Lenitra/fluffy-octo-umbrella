import json
import os 
import datetime

# this file path
here = os.path.dirname(os.path.abspath(__file__))


# WHAT TO REGENERATE
DEBUG = "users" # "users" or "map" or "both"

if DEBUG == "users" or DEBUG == "both":
    map = {}




    for i in range (32):
        for j in range(32):
            map[f"{i}:{j}"] = {
                "owner": "",
                "type": "",
                "units": 0
            }

    # register map into data/map.json
    with open(here+"/data/map.json", "w") as f:
        json.dump(map, f, indent=4)

now = datetime.datetime.now()
print(now.strftime("%Y-%m-%d %H:%M:%S"))

if DEBUG == "users" or DEBUG == "both":
    users = {
        "Lenitra": {
            "password": "password",
            "recdatetime": now.strftime("%Y-%m-%d %H:%M:%S"),
        },
    }

    # register users into data/users.json
    with open(here+"/data/users.json", "w") as f:
        json.dump(users, f, indent=4)
