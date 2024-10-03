#!/bin/bash

# Credentials
DB_HOST="localhost" 
DB_PORT="5434" 
DB_NAME="_customerdatabase_PORfERJCaF1p"
DB_USER="admin"
DB_PASSWORD="_customerdatabasep_4vvFpVXgTJd"

# Execute the query
PGPASSWORD=$DB_PASSWORD psql -h $DB_HOST -p $DB_PORT -U $DB_USER -d $DB_NAME -c "SELECT * FROM \"Customers\";"
