# POL Assessment

Please read the [Todos](DOCS/TODO.md)

Projects:
- [ANPR Enricher](AnprEnricher/README.md)
- [ANPR Data Processor](AnprDataProcessor/README.md)

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
- 2 GB free on your disk (if you'll be using the `docker-compose.yml` solution)
- Modern day CPU power
- Modern day amount of internal memory (times 2 if you'll be using `docker-compose.yml` solution)

## Installation
This documentation describes two ways of installing the solution:
- [Using Docker](DOCS/INSTALLATION/DOCKER.md)
- [Install on local machine](DOCS/INSTALLATION/LOCAL.md)

### Prerequisites
Make sure you have installed on your machine:
- .NET 8
- Docker (if you choose to run the solution on Docker containers)

### Docker


## Usage
After defining a hot hotfolder, just drop a tgz file containing a valid JSON in this folder and browse in de web app for the results. This solution also imports some pre-defined data in the database for testing purposes. A more detailed description of the usage with examples can be found [here](DOCS/USAGE/README.md).

Prior to using this (or any other) solution in a production environment, please consult the recommendations [here](DOCS/RECOMMENDATIONS.md).
