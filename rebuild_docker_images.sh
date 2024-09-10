#!/bin/bash

network_name="pol-assessment-network"

if [ ! "$(docker network ls --filter name=^${network_name}$ --format="{{ .Name }}")" ]; then
  docker network create ${network_name}
  echo "Network '${network_name}' created."
else
  echo "Network '${network_name}' already exists."
fi

docker build  -f Dockerfile.AnprFrontEnd -t anpr-frontend-image .
docker build  -f Dockerfile.AnprDataProcessor.WebApi -t anpr-dataprocessor-webapi-image .
docker build  -f Dockerfile.AnprEnricher.App -t anpr-enricher-app-image .
docker build  -f Dockerfile.AnprFrontEnd.WebApi -t anpr-frontend-webapi-image .

docker-compose up -d --force-recreate
