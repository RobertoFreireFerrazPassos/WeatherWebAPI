# WeatherWebAPI


## Set up

Run Docker Compose up

PgAdmin for manage postgreSQL:
http://localhost:16543/

look at docker compose file for PgAdmin authentication

Click in Add new server

```
Server name: postgres-db
Host Name: postgres-db
Maintenance database: weather
Port: 5432
Username: simha
Password: Postgres2019!
```

## swagger test

Endpoint /api/registration

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

Replace appSettings url for testing CircuitBreaker

```json
"RestCountriesApi": {
   "Url": "https://httpstat.us/500"
}
```

Stop only 'redisdb' container to test resiliency of the system

## Notes

- Added Cache for restcountries Api for extra resiliency in case this api is down.