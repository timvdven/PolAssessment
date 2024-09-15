"""Routes for Flask app."""

from flask import render_template, send_from_directory, make_response

def register(app):
    @app.route("/", methods=['GET'])
    def home_route():
        return render_template('pages/app.jinja')
    
    @app.route("/app/", methods=['GET'])
    @app.route("/app/<string:page>/", methods=['GET'])
    def app_route(page:str = None):
        if page and page in ['login', 'dashboard', 'map']:
            return render_template(f'app/{page}.jinja')
        return render_template('pages/app.jinja')

    # @app.route('/favicon.ico', methods=['GET'])
    # def favicon_route():
    #     return send_from_directory('statics', 'favicon.ico', mimetype='image/x-icon')
