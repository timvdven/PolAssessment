"""Context processors for Flask templates."""

from time import time
from datetime import datetime

def register(app, settings):
    @app.context_processor
    def register_context_processor():
        return { 
            'cachebuster': time(),
            'version': datetime.now().strftime('%Y-%m-%d.%H:%M:%S'),
            'nonce': settings.nonce,
            'webapi_url': settings.webapi_url,
            'google_map_api_key': settings.google_map_api_key,
        }
