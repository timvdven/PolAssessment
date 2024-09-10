# POL Assessment

## Table of Contents
- [Description](#description)
- [Installation](#installation)
- [Requirements](#requirements)
- [Usage](#usage)

## Description
- This solution is the result of the assessment described [here](DOCS/ASSESSMENT.md).
- In short: it is a system that reads ANPR data from a hot folder and enriches this data using online APIs. Furthermore this data can be viewed in a web app.
- The architecture can be found [here](DOCS/ARCHITECTURE/README.md).

## Requirements
If you want to host this entire solution on your machine, you should at least have:
- Docker (`docker` & `docker-compose`)
- 2 GB free on your disk (if you'll be using the `docker-compose.yml` solution)
- Modern day CPU power
- Modern day amount of internal memory (times 2 if you'll be using `docker-compose.yml` solution)
  - At least 650 MB

Please read the [Todos](DOCS/TODO.md)

## Installation
This documentation describes two ways of installing the solution:
- [Using Docker](DOCS/INSTALLATION/DOCKER.md)
- [Install on local machine](DOCS/INSTALLATION/LOCAL.md)

### Prerequisites
Make sure you have installed on your machine:
- .NET 8
- Python 3
- Docker (if you choose to run the solution on Docker containers)

Furthermore, you'll need:
- [Geoapify API Key](#set-geoapify-api-key)
- [Google Maps API Key](#set-google-maps-api-key)
- [Test Data](./DataGenerator/README.md)

#### Set Geoapify API Key
- Skip to the last bullet if you already possess a valid Geoapify API Key
- Go to the [Geoapify page](https://www.geoapify.com/reverse-geocoding-api/) and log in or sign up
- Add a new project or choose an existing one
- Obtain your API Key
- Paste the obtained API Key in a file named `appsettingsSecret.json` directly under the [AnprEnricher.App](./PolAssessment/AnprEnricher.App/) folder like this:
```json
{ 
  "LocationEnricher": {
    "ApiKey": "PASTE YOUR API KEY HERE"
  }
}
```

#### Set Google Maps API Key
- Skip to the last bullet if you already possess a valid Google Maps API Key
- Go to [Google Maps Platform](https://console.cloud.google.com/projectselector2/google/maps-apis/credentials) and log in or sign up
- Choose or create a project
- Choose "Create Credentials" -> "API Key"
- Add or Edit your Api Key, it is sufficient to restrict this key:
  - Maps Embed API
  - Maps JavaScript API
- Paste the obtained API Key in a file named `.env.secrets` directly under the [AnprFrontEnd](./AnprFrontEnd/) folder like this:
```bash
GOOGLE_MAP_API_KEY="PASTE YOUR API KEY HERE"
```

## Docker
If you meet all the requirements and prerequisites, you could simply run:
```bash
./rebuild_docker_images.sh
```
Which will:
- Build all docker images
  - 3 .NET projects
  - 1 Python project
- Create/Recreate a docker-compose project named polassessment

After successfully running the script, browse to the [ANPR Front End](http://localhost:5300/) page.
Unless you changed the yml file, the Hotfolder should be found [here](/HotFolders/HotFolderTgz/) after initiation of the anpr-enricher-app.

## Usage
If the anpr-enricher-app is running and bound to the correct hotfolder, just drop a tgz file containing a valid JSON in this folder and browse in de web app for the results. This solution also imports some pre-defined data in the database for testing purposes. A more detailed description of the usage with examples can be found [here](DOCS/USAGE/README.md).

Prior to using this (or any other) solution in a production environment, please consult the recommendations [here](DOCS/RECOMMENDATIONS.md).

### Creation of test data
The documentation on creating test data can be found in the separate Data Generator project, described [here](../DataGenerator/README.md).
