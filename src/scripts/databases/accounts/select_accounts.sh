#!/bin/bash

# Credentials are read from src/.env (see src/.env.example)
set -a; source "$(dirname "$0")/../../../.env"; set +a

DB_HOST="localhost"
DB_PORT="5433"
DB_NAME="${ACCOUNT_DB_NAME}"
DB_USER="${ACCOUNT_DB_USER}"
DB_PASSWORD="${ACCOUNT_DB_PASSWORD}"

# Execute the query with ordering by `CreatedDate` column
PGPASSWORD=$DB_PASSWORD psql -h $DB_HOST -p $DB_PORT -U $DB_USER -d $DB_NAME -c "SELECT * FROM \"Accounts\" ORDER BY \"CreatedDate\" ASC;" | awk '{
    if (NR==1) {
        print "\033[1;31m" $0 "\033[0m"  # Red for header
    } else {
        print $0 "\033[0m"  # White for rows
    }
}'

