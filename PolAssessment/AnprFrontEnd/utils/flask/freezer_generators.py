"""Freezer generators for the website."""

def register(freezer):
    @freezer.register_generator
    def app_route():
        yield { 'page': 'login' }
        yield { 'page': 'dashboard' }
