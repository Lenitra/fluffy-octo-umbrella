from general import HERE  # pylint: disable=import-error
from __main__ import app
import json
from flask import jsonify
from os import path
import datetime


@app.route("/get_all_map")
def game_state():
    # Retourne l'entièreté de la map
    with open(HERE + "/data/map.json") as f:
        game_data = json.load(f)
    game_data = {"hexes": game_data}
    return jsonify(game_data)


# bouger les unités d'un hexagone à un autre
# @param origin: hexagone d'origine sous forme de string "x:z"
# @param destination: hexagone de destination sous forme de string "x:z"
# @param units: nombre d'unités à déplacer
# @return: True si le déplacement a réussi, False sinon
@app.route("/move_units/<string:origin>/<string:destination>/<int:units>")
def move_units(origin, destination, units):
    with open(HERE + "/data/map.json", "r") as f:
        hexes = json.load(f)

    if origin not in hexes or destination not in hexes:
        return "NOPE"

    if hexes[origin]["units"] < units:
        return "NOPE"

    if find_path(origin, destination, hexes[origin]["owner"]) == []:
        return "NOPE"

    # si les owner sont différents, on attaque
    if (
        hexes[origin]["owner"] != hexes[destination]["owner"]
        and hexes[destination]["type"].split(":")[0] != "HQ"
    ):
        if hexes[origin]["units"] > hexes[destination]["units"]:
            hexes[origin]["units"] -= hexes[destination]["units"]
            hexes[destination]["units"] = 0
            hexes[destination]["owner"] = hexes[origin]["owner"]
        else:
            hexes[destination]["units"] -= hexes[origin]["units"]
            hexes[origin]["units"] = 0

    # sinon on déplace les unités
    else:
        hexes[destination]["units"] += units
        hexes[origin]["units"] -= units

    with open(HERE + "/data/map.json", "w") as f:
        json.dump(hexes, f, indent=4)

    return jsonify(find_path(origin, destination, hexes[origin]["owner"]))


# Pemret de récupérer les hexagones d'un joueur et ses adjacents en fonction de ses batiments
# ROUTE APPELEE PAR LE CLIENT REGULIEREMENT
@app.route("/get_hex/<string:player>")
def get_hex(player):
    radius = 2
    update_resources(player)

    # Charger les hexagones
    with open(HERE + "/data/map.json", "r") as f:
        hexes = json.load(f)

    # Créer une liste pour les hexagones du joueur
    result = {}

    # Parcourir tous les hexagones et enregistrer ceux du joueur
    for k, v in hexes.items():
        if v["owner"] == player:
            result[k] = v

    # Parcourir les hexagones du joueur et ajouter les hexagones adjacents
    for _ in range(radius):
        tmp = result.copy()
        for k, v in tmp.items():
            x, z = map(int, k.split(":"))
            for adj in get_adjacent_hexes(x, z):
                key = f"{adj['x']}:{adj['z']}"
                if key not in result:
                    result[key] = hexes[key]

    return jsonify(format_json_hexes(result))


# Fonction permettant de trouver un chemin vers une tile en bfs
# @param origin: hexagone d'origine sous forme de string "x:z"
# @param destination: hexagone de destination sous forme de string "x:z"
# @param player: nom du joueur
# @return: liste des hexagones à traverser pour atteindre la destination
def find_path(origin, destination, player=""):
    with open(HERE + "/data/map.json", "r") as f:
        hexes = json.load(f)

    if origin not in hexes or destination not in hexes:
        return []

    # Créer une liste pour les hexagones visités
    visited = []
    # Créer une liste pour les hexagones à visiter
    queue = [[origin]]

    # Tant qu'il reste des hexagones à visiter
    while queue:
        path = queue.pop(0)
        node = path[-1]

        # Si on a atteint la destination
        if node == destination:
            return path

        # Si on n'a pas encore visité cet hexagone
        if node not in visited:
            # Ajouter cet hexagone à la liste des visités
            visited.append(node)
            if hexes[node]["owner"] != player:
                continue
            # Ajouter les hexagones adjacents à la liste des hexagones à visiter
            for k in get_adjacent_hexes(*map(int, node.split(":"))):
                if k not in visited:
                    new_path = list(path)
                    new_path.append(f"{k['x']}:{k['z']}")
                    queue.append(new_path)

    return []


# Permet de formater les hexagones en JSON pour les envoyer au client
def format_json_hexes(hexes):
    return {
        "hexes": [
            {"key": k, "owner": v["owner"], "type": v["type"], "units": v["units"]}
            for k, v in hexes.items()
        ]
    }


# update les ressources de toutes les tiles d'un joueur
# @param player: nom du joueur
def update_resources(player):

    max_accumulated_hours = 6

    with open(HERE + "/data/users.json", "r") as f:
        users = json.load(f)

    with open(HERE + "/data/map.json", "r") as f:
        hexes = json.load(f)

    deltatime = datetime.datetime.now() - datetime.datetime.strptime(
        users[player]["recdatetime"], "%Y-%m-%d %H:%M:%S"
    )
    deltatimehours = deltatime.total_seconds() / 3600
    if deltatimehours > max_accumulated_hours:
        deltatimehours = max_accumulated_hours
    if deltatimehours > 1:
        for k, v in hexes.items():
            if v["owner"] == player:

                # si c'est une HQ, on ajoute des unités
                if v["type"].split(":")[0] == "HQ":
                    v["units"] += int(deltatimehours)
                    print("added", int(deltatimehours), "units to", k)

        # on laisse les minutes restantes (en gros division euclidienne)
        deltatime = deltatimehours - int(deltatimehours)
        users[player]["recdatetime"] = (
            datetime.datetime.now() - datetime.timedelta(hours=deltatime)
        ).strftime("%Y-%m-%d %H:%M:%S")

        with open(HERE + "/data/users.json", "w") as f:
            json.dump(users, f, indent=4)
        with open(HERE + "/data/map.json", "w") as f:
            json.dump(hexes, f, indent=4)
        return True


# Fonction auxiliaire pour obtenir les hexagones adjacents
def get_adjacent_hexes(x, z):
    toret = []
    toret.append({"x": x + 1, "z": z})
    toret.append({"x": x - 1, "z": z})
    toret.append({"x": x, "z": z + 1})
    toret.append({"x": x, "z": z - 1})
    if x % 2 != 0:
        toret.append({"x": x + 1, "z": z + 1})
        toret.append({"x": x - 1, "z": z + 1})
    else:
        toret.append({"x": x + 1, "z": z - 1})
        toret.append({"x": x - 1, "z": z - 1})

    # Filtrer les hexagones hors de la map
    toret = list(
        filter(
            lambda x: x["x"] >= 0 and x["z"] >= 0 and x["x"] < 32 and x["z"] < 32, toret
        )
    )

    return toret
