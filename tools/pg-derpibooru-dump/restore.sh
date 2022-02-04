#!/bin/bash
GREEN='\033[0;32m'
ORANGE='\033[0;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

set -e

echo "Dropping database if exists"
dropdb --username "$POSTGRES_USER" --if-exists derpibooru

echo "Creating database derpibooru"
createdb --username "$POSTGRES_USER" derpibooru

echo "Restoring database"
pg_restore --username "$POSTGRES_USER" -e -O -d derpibooru dump.pgdump

echo "Calculating Wilson Score"
pgsql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "derpibooru" -f /scripts/add-wilson-score.sql

echo -e "${GREEN}Initialization of derpibooru database completed${NC}"