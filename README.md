# WeatherWebAPI


## Set up

Run Docker Compose up

PgAdmin for manage postgreSQL:
http://localhost:16543/

![image](https://github.com/RobertoFreireFerrazPassos/WeatherWebAPI/assets/41349878/fb834a71-ba81-4a49-87fc-eb45ace34ca7)

Login

```
Email: roberto@yahoo.com.br
Password: PgAdmin2019!
```

Click in Add new server

```
Server name: postgres-db
Host Name: postgres-db
Maintenance database: weather
Port: 5432
Username: simha
Password: Postgres2019!
```

Right click on 'weather' database and select "Query tool"

Create Users table

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

- Endpoint /api/registration

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


## Resiliency tests

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

- Added Cache for restcountries Api for extra resiliency in case this api is down.

- Added GlobalErrorHandlerMiddleware. If there is no try catch to handle an exception, the global error handler will handle the issue. 

Example below when the application tried to insert a new row in Users table.

```json
{
  "message": "23505: duplicate key value violates unique constraint \"users_email_key\"\n\nDETAIL: Detail redacted as it may contain sensitive data. Specify 'Include Error Detail' in the connection string to include this information."
}
```
