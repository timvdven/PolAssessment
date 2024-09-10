from flask import Flask
from flask_minify import Minify

from utils.attr_dict import AttrDict
from utils.flask import context_processors, routes

from os import environ
from dotenv import load_dotenv

def build(settings:AttrDict):
    app = Flask(__name__, template_folder='templates', static_folder='statics', static_url_path='/statics')

    app.status = AttrDict({})

    routes.register(app)

    context_processors.register(app, settings)

    return app

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

    if environ.get('NO_MINIFY') not in ['True', 'true', '1']:
        Minify(app=app)

    debug = environ.get('DEBUG') in ['True', 'true', '1']
    port = environ.get('PORT')
    host = environ.get('HOST')

    app.run(debug=debug, port=port, host=host)
