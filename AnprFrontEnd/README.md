# ANPR Front End
AnprFrontEnd is Python web app for serving the GUI of the PolAssessment solution. It uses `Flask` but especially `Flask-Minify` and `Frozen-Flask` to generate very static html files. As the front end bit I used the [`htmx` framework](https://htmx.org/).

Other docs:
- [main README](../README.md)
- [Architecture README](../DOCS/ARCHITECTURE/README.md)

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

### Configure: Set Google Maps API Key
- Skip to the last bullet if you already possess a valid Google Maps API Key
- Go to Google Maps Platform and log in or sign up
- Choose or create a project
- Choose "Create Credentials" -> "API Key"
- Add or Edit your Api Key, it is sufficient to restrict this key:
  - Maps Embed API
  - Maps JavaScript API
- Paste the obtained API Key in a file named `.env.secrets` directly under the AnprFrontEnd folder like this:

```bash
GOOGLE_MAP_API_KEY="PASTE YOUR API KEY HERE"
```

### Realtime run
```bash
python ./app.py
```
Read the output of the app in order to browse to the correct port number.

### build static files
```bash
python ./freeze.py
```
It will (re)create a `build` folder and fills it with static html, js, css and misc. assets files. Serve these files e.g. with an NGINX daemon.

## Docker
In the main root of the solution, simply invoke
either
```bash
docker build  -f Dockerfile.AnprFrontEnd -t anpr-frontend-image .
```
or, for the entire solution:
```bash
./rebuild_docker_images.sh
```
for building the Docker image(s).

Finally either manially create a Docker container or run:
```bash
docker-compose up -d
```
in order to containerize the solution. Use the `-d` option to run it as a daemon.
