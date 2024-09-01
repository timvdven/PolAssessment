from flask_frozen import Freezer
from flask_minify import Minify

from utils.flask import freezer_generators
from app import build

from utils.attr_dict import AttrDict

from os import environ
from dotenv import load_dotenv

if __name__ == '__main__':
    load_dotenv()
    nonce = environ.get('NONCE')

    settings = AttrDict({
        'nonce': nonce or 'nonsense',
    })

    app = build(settings)
    freezer = Freezer(app)
    Minify(app=app)
    freezer_generators.register(freezer)

    freezer.freeze()
    #freezer.run(debug=True)
