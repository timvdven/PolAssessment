# Database: MariaDb

[Main README](../README.md)
[Architecture README](../DOCS/ARCHITECTURE/README.md)

Either install MariaDB either locally or on a server, or use the [MariaDB](https://hub.docker.com/_/mariadb) (image: mariadb:lts) image from the Docker image repositories.

After properly installing MariaDB, run the SQL files in the following order:
- [01-create-tables.sql](./db-docker/init-sql/01-create-tables.sql)
- [02-insert-data.sql](./db-docker/init-sql//02-insert-data.sql)
- [03-setup-indexes](./db-docker/init-sql/03-setup-indexes.sql)

The indexes are set on the fields `LicensePlate` and `ExactDateTime` because the high probably that those fields will be used in query where-clauses.

## Optionally: phpMyAdmin

Optionally, you can install phpMyAdmin in order to use a web based GUI for managing the MariaDB. To do so, either download and install phpMyAdmin or obtain the Docker image: [phpMyAdmin](https://hub.docker.com/_/phpmyadmin): (image: phpmyadmin:latest).

## Docker Compose

Both MariaDB and phpMyAdmin are included in the main `docker-compose.yml` file in the root of this project.