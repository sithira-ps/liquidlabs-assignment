# Liquid Labs Assignment

## Project Progress:

- Project creation:
  `dotnet new webapi --use-controllers -o liquidlabs-assignment`
- Git Initialization:
  `git init`
  `dotnet new gitignore`
- Add SQL package
  `dotnet add package Microsoft.Data.SqlClient`
- Add Tests project
  `dotnet new xunit -0 liquidlabs-assignment.Tests`
- Add Moq package (for tests)
  `dotnet add package Moq`
- Add FluentAssertions package (for tests)
  `dotnet add package FluentAssertions`

## Public API

- Im using RestCountries API from https://restcountries.com/
- The dataset covers 90+ normalized fields per country

## Project Structure/Architecture:

- Project consists of 5 main layers
  1. Middleware
  2. Controller
  3. Service
  4. Repository
  5. Database

- Please refer the below diagram

![alt text](architecture-diagram.png)

- When the request comes, it first go through the Error Handling Middleware
- Then the controller takes over, handles the http request and pass the request to the service layer.
- Service layer includes the business logic. It check if there is a local cache or not by trying to retrieve the data from the the database, through the repository layer. (Service -> Repository -> Database)
- If there is data, it send that retrieved data to the client.
- If there is no data, it call the external api endpoint to retrieve data, and then save it to the database, and then share it with the client.

## Caching Logic

### Get By Country Name

- Initially database it empty, or not available the country that is searching.
- So it has the retrieve the data from the external API
- Then it save that record to the database. When it does it use sync_level as 1. This is important to validate the cache.
- If there is another request come asking for same country, now it can extract it from the db. (a record that match the country name without considering the sync_level. Because any sync_level works fine for this type of request)

### Get By Continent Name

- Here it could be few scenarios
  1. Scenario 1: Database it empty
  2. Scenario 2: Database doesn't has any records with that continent
  3. Scenario 3: Database has one or more records with that continent, but sync_level = 1.
  4. Scenario 4: Database has all the records with for continent, and sync_level = 2 or sync_level = 3.

- In Scenario 1, Scenario 2: we retrieve the data from the API and save it in the database as sync_level = 2.
- In Scenario 3: Here we have few records for that continent, but those are records saved when call GetByCountry method. We cannot grantee these records are the all the records for this continent. There could be some other countries that are not in the database. So we retrieve the data from the API, delete all the records with that continent and sync_level = 1. Only after that, we save the extracted data to the database. This delete step is important. Because there could be records with sync_level = 1 for that continent. (Eg: when search ac country by name) We cant add our new record while these records still in the database, due to duplicate records.
- In Scenario 4: Unlike in scenario 3, we can grantee that available records are the all the records for that continent. So we don't call the API. Simply return the data from the DB.

### Get All

- Here it could be few scenarios
  1. Scenario 1: Database it empty
  2. Scenario 2: Database has one or more records, but sync_level = 1 or sync_level = 2.
  3. Scenario 3: Database has all the records, and sync_level = 3.

- In Scenario 1: we retrieve the data from the API and save it in the database as sync_level = 3.
- In Scenario 2: Similar to the Scenario 3 in GetByContinentName. So we retrieve the data from the API and delete all the records available. Only after that, we save the extracted data to the database.
- In Scenario 3: Unlike in scenario 2, we can grantee that available records are the all the records needed. So we don't call the API. Simply return the data from the DB.

## Setting up the Project

### Clone the project

- Clone the repository: `git clone https://github.com/sithira-ps/liquidlabs-assignment.git`

### Setting up the Public API

- Go to https://restcountries.com/sign-up
- Create a new account by adding the email and other necessary details
- Confirm the email by clicking the link that has sent to your email (this email might be in the spam folder)
- Once confirmed, you will navigate to the Welcome page, which has mentioned your API key.
- Copy that API key and paste it in the appsettings.json file -> ExternalApi -> ApiKey section

### Setting up the Database

- Create a new SQL Server database and paste its connection string in the appsettings.json file -> ConnectionStrings -> DefaultConnection (or fill the placeholders in the dummy connection string available there)
- Run the sql-schema.sql file in that database, to create the necessary table.

### Run the project

- Run the project:
  - Go the the main project : `cd liquidlabs-assignment`
  - Run the project using: `dotnet run`
- Run tests of this project:
  - Go the the test project : `cd liquidlabs-assignment.Tests`
  - Run the project using: `dotnet test`

- Open Swagger: http://localhost:5011/swagger/
- Get all countries: http://localhost:5011/api/v1/Countries
- Get all countries by continent: http://localhost:5011/api/v1/countries/continent/{continent}
- Get all countries by name: http://localhost:5011/api/v1/countries/{name}
