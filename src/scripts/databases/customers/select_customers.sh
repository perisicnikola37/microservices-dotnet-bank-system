#!/bin/bash

# Credentials are read from src/.env (see src/.env.example)
set -a; source "$(dirname "$0")/../../../.env"; set +a

DB_HOST="localhost"
DB_PORT="5434"
DB_NAME="${CUSTOMER_DB_NAME}"
DB_USER="${CUSTOMER_DB_USER}"
DB_PASSWORD="${CUSTOMER_DB_PASSWORD}"

# Execute the query
PGPASSWORD=$DB_PASSWORD psql -h $DB_HOST -p $DB_PORT -U $DB_USER -d $DB_NAME -c "SELECT * FROM \"Customers\";"
