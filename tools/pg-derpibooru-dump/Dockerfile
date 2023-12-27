FROM postgres:bullseye

RUN apt-get update && apt-get install -y \
	wget

RUN wget -q -O dump.pgdump https://files.derpicdn.net/derpibooru_public_dump_$(date +"%Y_%m_%d").pgdump

COPY restore.sh /docker-entrypoint-initdb.d/

ADD ./add-wilson-score.sql /scripts/