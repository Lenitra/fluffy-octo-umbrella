import json
import os 

# this file path
here = os.path.dirname(os.path.abspath(__file__))


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