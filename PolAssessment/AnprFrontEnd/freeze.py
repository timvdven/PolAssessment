from flask_frozen import Freezer
from flask_minify import Minify

from utils.flask import freezer_generators
from app import build

from utils.attr_dict import AttrDict

from os import environ
from dotenv import load_dotenv

if __name__ == '__main__':
    load_dotenv('.env')
    load_dotenv('.env.secrets')
    
    nonce = environ.get('NONCE')
    webapi_url = environ.get('WEBAPI_URL')
    google_map_api_key = environ.get('GOOGLE_MAP_API_KEY')

    settings = AttrDict({
        'nonce': nonce or 'nonsense',
        'webapi_url': webapi_url,
        'google_map_api_key': google_map_api_key,
    })

    app = build(settings)
    freezer = Freezer(app)
    Minify(app=app)
    freezer_generators.register(freezer)

    freezer.freeze()
    #freezer.run(debug=True)
