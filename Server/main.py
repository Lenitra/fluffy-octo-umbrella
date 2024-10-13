from flask import Flask, jsonify
import json
import os

# this file path
here = os.path.dirname(os.path.abspath(__file__))

app = Flask(__name__)


def format_json_hexes(hexes):
    return { "hexes": [{"key": k, "owner": v["owner"], "type": v["type"], "units": v["units"]} for k, v in hexes.items()] }


@app.route("/get_all_map")
def game_state():
    # Retourne l'entièreté de la map
    with open(here+"/data/map.json") as f:
        game_data = json.load(f)
    game_data = {
        "hexes": game_data
    }
    return jsonify(game_data)


# Fonction auxiliaire pour obtenir les hexagones adjacents
def get_adjacent_hexes(x, z):
    if z % 2 == 0:
        return [
            {"x": x + 1, "z": z},
            {"x": x - 1, "z": z},
            {"x": x, "z": z + 1},
            {"x": x, "z": z - 1},
            {"x": x - 1, "z": z - 1},
            {"x": x + 1, "z": z - 1}
        ]

    else:
        return [
            {"x": x + 1, "z": z},
            {"x": x - 1, "z": z},
            {"x": x, "z": z + 1},
            {"x": x, "z": z - 1},
            {"x": x - 1, "z": z + 1},
            {"x": x + 1, "z": z + 1}
        ]
    


# Route Flask pour obtenir les hexagones d'un joueur dans un certain rayon
@app.route("/get_hex/<string:player>/<int:radius>")
def get_hex(player, radius):
    print("Searching for hexes of player", player, "in radius", radius)

    # Charger les hexagones
    with open(here+"/data/map.json", "r") as f:
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
    



    # Formater les hexagones en JSON
    result = format_json_hexes(result)

    # Retourner le résultat sous forme de JSON
    return jsonify(result)


if __name__ == "__main__":
    app.run(debug=True)
