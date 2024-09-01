docker build  -f Dockerfile.AnprFrontEnd -t anpr-frontend-image .
docker build  -f Dockerfile.AnprDataProcessor -t anpr-dataprocessor-image .
docker build  -f Dockerfile.AnprEnricher -t anpr-enricher-image .
docker build  -f Dockerfile.AnprWebApi -t anpr-webapi-image .
