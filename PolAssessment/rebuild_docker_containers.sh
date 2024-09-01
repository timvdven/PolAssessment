docker-compose down -v
docker network rm pol-assessment-network
docker network create pol-assessment-network
export HOTFOLDERS_PATH=${HOME}/git/PolAssessment/HotFolders
docker-compose up -d
docker image prune
