#!/bin/bash

## This script will return all data from "Customers" collection in "transaction_database" and export it as `transactions.json` file.

# Define the output filename as a variable
output_file="transactions.json"

# Execute the mongo command and filter out the unwanted line
mongo --host localhost --port 27017 --authenticationDatabase admin --quiet <<EOF | grep -v "switched to db TransactionDb" > "$output_file"
use TransactionDb 
db.Transactions.find().forEach(doc => printjson(doc))
EOF

# Format the JSON and replace BinData and ISODate
sed -i -E 's/BinData\([0-9]+,"(.*?)"\)/"\1"/g; s/ISODate\("(.*?)"\)/"\1"/g;' "$output_file"

# Wrap the output in square brackets to create a JSON array
sed -i -e '1s/^/[/' -e '$s/$/]/' "$output_file"

# Add a comma after each closing brace '}' except the last one
sed -i '$!s/}/},/' "$output_file"

# Check if the export was successful
if [ -f "$output_file" ]; then
  echo "Export successfully completed."
else
  echo "Export failed. Error occurred."
fi
