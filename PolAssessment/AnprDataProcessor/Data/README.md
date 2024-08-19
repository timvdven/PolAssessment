docker-compose up -d

docker network create pol-assessment-network

docker exec -it mysql-container mysql -uuser -puserpassword anprdata
