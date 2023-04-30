# Assessment 2
In light of the feedback received from the initial assignment and the scaffolding project, I have thoroughly restructured the whole project to ensure it aligns more closely with the specified requirements.

# Refactor

### Environments
The project was renamed from Library WebAPI to Bookstore-API. There are three appsettings.json files to ensure that all critical variables are in enviromental files. Available environments are: Development, Production and Test. The project now, finds the connection with the PostgreSQL database from the appsettings of the running environment. 

### Exception Handling
All controller actions now include exception handling with printing the error in the console and returning 500 Internal Error to the user.

### DB Repositories
Restructured the way of accessing postgres from datalayer to repositories and fixed a bug where the connection could not close and with this new structure we avoid locks. Also the connection handling now happens in the repositories. 

### RESTful Approach
Authors controller now returns only data for authors and BooksController for books. This ensures that we follow the RESTful approach and we dont return any unnessesary data to the payload.

### Common Format
All responses to the user, even for the new features that have been added to the system, now return the responses in JSON format for better communication between systems.

# Token-Based Authentication
Bookstore application supports JWT tokens to authenticate the users. Some endpoints of the API (based on the provided use cases) now need to authenticate with a role as a Developer or a User with the bearer jwt token. Some sample of these are the following:
```
Developer:
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL2dpdGh1Yi5jb20vYWxleHBvdXJuYXJhcyIsImF1ZCI6ImJvb2tzdG9yZSIsInJvbGVzIjoiRGV2ZWxvcGVyIiwibmFtZSI6IlRha2lzIEdvbmlhcyIsImV4cCI6MjYwNzk5MDQwMH0.fgQfeFyVTDLcUeWZ4hqYhKV75TtK7Hqd6dVOLtfQGZI

User:
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL2dpdGh1Yi5jb20vYWxleHBvdXJuYXJhcyIsImF1ZCI6ImJvb2tzdG9yZSIsInJvbGVzIjoiVXNlciIsIm5hbWUiOiJUYWtpcyBHb25pYXMiLCJleHAiOjI2MDc5OTA0MDB9.bQhq_WYmn4zfay4BgKarp023OymzI2PvWqBIyvxb5Nw
```

# Throttling
All endpoints now support rate limiting. Application can now accept till 20 requests per 2 seconds. Otherwise, the result will be 429 Too Many Requests.

# IP Lookup
Using an external API (https://ipapi.co/), all the requests for create and delete methods are denied if the country of the user is not allowed. The allowed country can be edited from the enviroment file. Also the appsettings.json has a property "CustomLookupIP" that can set a custom IP address, and this is because of the closed network of the docker, that doesnt allow us to get the real IP. When this variable is null (see production environment), the real IP address from the user is being looked up.

# Redis
Bookstore application has been improved with the implementation of a caching layer with RedisDB. Instead of using in-app cache, the Redis from the docker compose file can now host the caching mechanism of the application for better performance and speed. All actors are being saved in redis with the key `author:{id}` and all books with the key `book:{id}`. With this way we can avoid stressing our postgres database.

# Tests

### Safe Testing
Testing was a difficult part to be developed. Not for the tests themselfs, but for the structure of the testing. When tests begin, a new database `bookstore_test` is being created with tables authors and books and all with inital data that could later be processed only for tests. The same happens with Redis. For testing purposes only, the redis uses the second of the 16 databases that it can use (`defaultDatabase=1`).

With this structure, both the postgres and redis production databases are safe from the testing part. After the tests finish, both databases are being deleted/cleared.

### Test Coverage
The tests developed for testing the application can cover up to 86% of the codebase. If we could exclude the exception handling that its not required to test, the percentage could go above 95%.

![Test Coverage Percentage](img/tests_coverage.jpg)

The created tests are for Authors, Books and of course the bulk endpoint of parsing multiple books in jobs. For this, there are the `AuthorsTests`, the `BooksTests` and the `JobsTests` methods. There is also a commented region of standalone tests but there are commented because its not recommented to run all of them together. The non-standalone tests can use the async/await pattern to follow the actions from one API call to another.

![Standalone Tests](img/standalone_tests.jpg)