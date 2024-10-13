from flask import Flask, jsonify
import json
import os

# this file path
here = os.path.dirname(os.path.abspath(__file__))

app = Flask(__name__)


@app.route("/get_player_map")
def game_state():
    # Retourne l'entièreté de la map
    with open(here+"/data/map.json") as f:
        game_data = json.load(f)
    game_data = {
        "hexes": game_data,
        "centerTile": {
            "x": 50,
            "z": 50
        }
    }
    return jsonify(game_data)


if __name__ == "__main__":
    app.run(debug=True)
