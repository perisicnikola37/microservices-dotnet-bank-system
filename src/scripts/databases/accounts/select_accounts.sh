#!/bin/bash

# Credentials
DB_HOST="localhost" 
DB_PORT="5433" 
DB_NAME="_accountdatabase_gpQcTBtS3NE1S"
DB_USER="admin"
DB_PASSWORD="_accountdatabasep_wPRhN1iCfGsb"

# Execute the query
PGPASSWORD=$DB_PASSWORD psql -h $DB_HOST -p $DB_PORT -U $DB_USER -d $DB_NAME -c "SELECT * FROM \"Accounts\";"
