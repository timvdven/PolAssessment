# ANPR Front End
AnprFrontEnd is Python web app for serving the GUI of the PolAssessment solution. It uses `Flask` but especially `Flask-Minify` and `Frozen-Flask` to generate very static html files. As the front end bit I used the [`htmx` framework](https://htmx.org/).

Other docs:
- [Back to main README](../README.md)

## Local installation

### Prep

Create a virtual env (venv):
```bash
python -m venv ~/venv_anpr
```

Activate the venv:
```bash
source ~/venv_anpr/bin/activate
```
or in Windows:
```
~\venv_anpr\Scripts\Activate.ps1
```

Update `pip`:
```bash
pip install --upgrade pip
```

Install required modules:
```bash
pip install -r requirements.txt 
```
