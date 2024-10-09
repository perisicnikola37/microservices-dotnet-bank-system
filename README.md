## [Installation guide](https://github.com/perisicnikola37/microservices-dotnet-bank-system/wiki/Installation-guide)

<hr/>

![68747470733a2f2f692e706f7374696d672e63632f51744234685768442f556e7469746c65642d323032342d30392d32332d313335372d312e706e67](https://github.com/user-attachments/assets/9f8673d8-8e2d-405e-84d3-fcbe3d100e0e)

![screencapture-localhost-8007-healthchecks-ui-2024-10-09-15_31_40](https://github.com/user-attachments/assets/cc2f7eea-649f-4fb2-809d-57dff2f013b6)

## Message broker

### RabbitMQ

URL: http://localhost:15672 

Credentials:

Username: admin@gmail.com

Password: _rabbitmq_3gVE_0c!BbUby+

<hr />

## Database and container management tools

### pgAdmin

URL: http://localhost:5050 

Credentials:

Username: admin@gmail.com

Password: _pgadmin_v-r6yVMv5G30V#2

<hr />

### mongo-express

URL: http://localhost:8081

Credentials:

Username: admin

Password: _transactiondatabase_9G2sTQ*HM5

<hr />

### Portainer

URL: http://localhost:9000 

<hr />

## Services

### Account API

URL: http://localhost:8001/swagger/index.html

### Account Database (PostgreSQL)

Credentials:

Host name: account_database

Database name: _accountdatabase_gpQcTBtS3NE1S

Username: admin

Password: _accountdatabasep_wPRhN1iCfGsb

<hr />

### Customer API

URL: http://localhost:8002/swagger/index.html

### Customer Database (PostgreSQL)

Credentials:

Host name: account_database

Database name: _customerdatabase_PORfERJCaF1p

Username: admin

Password: _customerdatabasep_4vvFpVXgTJd

<hr />

### Transaction API

URL: http://localhost:8003/swagger/index.html 

### Transaction Database (MongoDB)

Credentials:

Username: admin

Password: _transactiondatabase_9G2sTQ*HM5

<hr />

## Custom nuget package

### ApiVersioningLib

URL: https://www.nuget.org/packages/ApiVersioningLib

<hr />

## Data visualization 

### Elastic search: http://localhost:9200 

### Kibana: http://localhost:5601

<hr />

## Health checks

### By service

Account service: http://localhost:8001/health-check
Account service: http://localhost:8002/health-check
Account service: http://localhost:8003/health-check

### Web status application

URL: http://localhost:8007/healthchecks-ui

