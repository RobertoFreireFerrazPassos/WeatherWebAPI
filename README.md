# WeatherWebAPI

## Required Tools

- Computer with Windows OS
- Docker Desktop application running Linux
- Visual Studio 2022

## Docker containers

- postgres-db (database)
- postgresit-db (integration tests database)
- pgadmin (database management tool)
- redisdb (distributed cache)
- weather.webapi (web api project using net8.0 and swagger)

## Set up

### Docker compose

Click on "Docker compose" in Visual Studio (useful for debugging web api in Visual Studio)

![image](https://github.com/RobertoFreireFerrazPassos/WeatherWebAPI/assets/41349878/532e903a-ac05-49c0-8fad-479f7d12c221)

Or run cmd "docker compose up" in a folder that contains "docker-compose.yml" file.

When containers are ready:

- Cmd console

![image](https://github.com/RobertoFreireFerrazPassos/WeatherWebAPI/assets/41349878/e81a31f2-f5e3-49b6-b649-905a63734ba0)

- Docker Desktop containers:

![image](https://github.com/RobertoFreireFerrazPassos/WeatherWebAPI/assets/41349878/ce485411-e014-49f0-a57e-b4b3fba07c59)

- Docker Desktop volumes:

![image](https://github.com/RobertoFreireFerrazPassos/WeatherWebAPI/assets/41349878/6b241dd1-99fc-4430-aa61-21ea691993d7)

- Windows folder for volumes:

![image](https://github.com/RobertoFreireFerrazPassos/WeatherWebAPI/assets/41349878/4544ff87-4064-467e-ab4a-de166a05e09a)

### Create tables for both databases

#### PgAdmin 

http://localhost:16543/

It might take some time to load the PgAdmin since it depends on "postgres-db" and "postgresit-db" containers to be ready.

Login

```
Email: simha@yahoo.com.br
Password: PgAdmin2019!
```

![image](https://github.com/RobertoFreireFerrazPassos/WeatherWebAPI/assets/41349878/2a9d5cc2-465b-46b9-9d84-01901f53605b)

Click on add new server

![image](https://github.com/RobertoFreireFerrazPassos/WeatherWebAPI/assets/41349878/2482ce6d-5016-4e45-9748-9d7d845719e0)


Use this information below for "general" and "connection" tabs in "Register - Server" modal:

![image](https://github.com/RobertoFreireFerrazPassos/WeatherWebAPI/assets/41349878/26862b88-f397-4f41-8ce1-c2d8756d3955)

For Weather database, use this:

```
Server name: postgres-db
Host Name: postgres-db
Maintenance database: weather
Port: 5432
Username: simha
Password: Postgres2019!
```

For Integration Test database, use this:

```
Server name: postgresit-db
Host Name: postgresit-db
Maintenance database: weatherit
Port: 5432
Username: simha
Password: Postgres2019!
```

Right click on 'weather' database, for example, and select "Query tool"

![image](https://github.com/RobertoFreireFerrazPassos/WeatherWebAPI/assets/41349878/fb834a71-ba81-4a49-87fc-eb45ace34ca7)

Create Users table for both databases

```sql
CREATE TABLE IF NOT EXISTS Users (
    Id UUID PRIMARY KEY,
    Firstname VARCHAR(100),
    Lastname VARCHAR(100),
    Username VARCHAR(100) UNIQUE,
    Email VARCHAR(100) UNIQUE,
    PasswordHash VARCHAR(255),
    Address TEXT,
    Birthdate DATE,
    PhoneNumber VARCHAR(20),
    LivingCountry VARCHAR(100),
    CitizenCountry VARCHAR(100)
);
```

## Swagger test

Swagger UI will open automatically if we are running docker compose from Visual Studio.

If not, validate the port "52691" as shown in previous "Docker Desktop containers" image for weather.webapi container.

http://localhost:52691/swagger/index.html

- Endpoint POST: /api/registration

```json
{
  "firstname": "Jack",
  "lastname": "Doe",
  "email": "user@example.com",
  "password": "123456",
  "address": "Bastions Valletta VLT 193",
  "birthdate": "2004-01-12",
  "phoneNumber": "+356 22915000",
  "livingCountry": "MLT",
  "citizenCountry": "MLT"
}
```
- Authorize registered user

The requirement is to use Basic Authorization. It is not to implement a bearer/token authentication.

We don't need to create an endpoint "/api/login" just to return a base64-encoded string username:password

Instead we are going to use the built-in UI modal in Swagger to automatically add the authorization header

Unfortunatelly, Swagger is adding the authorization header for all endpoints, even for those which authorization is not required.

![image](https://github.com/RobertoFreireFerrazPassos/WeatherWebAPI/assets/41349878/6395e800-c583-49cb-b262-3f46e4d33b02)

- Endpoint GET: /api/weather

The requirement is to get the weather conditions of the country for the user registered in our system.

Since this endpoint uses Basic Authorization, we don't need to pass /{username} in url.

/{username} in the url means that the user can add any username but this is not the requirement.

## Weather.Tests project

Run tests using Visual studio.

### Unit tests

Classes ending with "Tests"

Example: AuthServiceTests

### Integration tests

Classes ending with "IntegrationTests"

Example: RepositoryIntegrationTests

## Resilience/Fault tolerance tests

Stop only 'redisdb' container

Stop only 'postgres-db' container

Stop 'redisdb' and 'postgres-db' containers

Replace appSettings url for testing the circuit breaker

```json
"RestCountriesApi": {
   "Url": "https://httpstat.us/500"
}
```


## Notes

- Added also Cache for restcountries Api for extra resiliency in case this api is down.

- Added GlobalErrorHandlerMiddleware. If there is no try catch to handle an exception, the global error handler will handle the issue. 

Example below when the application tried to insert a new row in Users table.

```json
{
  "message": "23505: duplicate key value violates unique constraint \"users_email_key\"\n\nDETAIL: Detail redacted as it may contain sensitive data. Specify 'Include Error Detail' in the connection string to include this information."
}
```

- It is saving database docker volume in folder "C:\dockervolumes\postgresql". I didn't test in a different OS other than Windows.

- The documentation mentions no need to generate salt. "his implementation on hashing will generate a salt automatically for you"

https://github.com/BcryptNet/bcrypt.net

## Improvements

- Add a health check endpoint testing the health of all the dependencies (databases, external apis and redis).
