#!/bin/bash

## This script will delete all data from "Customers" collection in "transaction_database" 

mongo --host localhost --port 27017 --authenticationDatabase admin --quiet <<EOF
use TransactionDb
db.Transactions.deleteMany({})
EOF

# Check if deletion was successful
if [ $? -eq 0 ]; then
  echo "All data was deleted successfully."
else
  echo "Error occured."
fi
