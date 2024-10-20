from flask import Flask, jsonify
import json
import os
import datetime

# this file path
here = os.path.dirname(os.path.abspath(__file__))

app = Flask(__name__)

import routes.map







if __name__ == "__main__":
    app.run(debug=True)
