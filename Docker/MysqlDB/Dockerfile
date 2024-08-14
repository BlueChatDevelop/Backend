FROM mysql:latest

ENV MYSQL_ROOT_PASSWORD=root_password
ENV MYSQL_DATABASE=bluechat_db
ENV MYSQL_USER=user
ENV MYSQL_PASSWORD=user_password

COPY init.sql /docker-entrypoint-initdb.d/
