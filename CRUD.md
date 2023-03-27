# Authors
GET `/authors`  
This endpoint retrieves a list of all authors with their books.

GET `/authors/:id`  
This endpoint retrieves an author with his/her books by ID.

POST `/authors`  
This endpoint creates a new author in the database with a json body.
```
{
  "first_name": "Takis",
  "last_name": "Gonias",
  "country": "Greece"
}
```

PUT `/authors/:id`  
This endpoint updates an existing author by ID with a json body.
```
{
  "first_name": "Takis",
  "last_name": "Gonias",
  "country": "Greece"
}
```

DELETE `/authors/:id`  
This endpoint deletes an author by ID.

GET `/authors/init/:numberOfAuthors`  
This endpoint creates a numerous authors based on the numberOfAuthors parameter.

# Books
GET `/books`  
This endpoint retrieves a list of all books with their authors.

GET `/books/:id`  
This endpoint retrieves a book with it's author by ID.

POST `/books`  
This endpoint creates a new book in the database with a json body.
```
{
  "date": "2023-03-12",
  "author_id": 10,
  "title": "Lord Of The Rings",
  "category": "Fantasy",
  "pages": 469
}
```

PUT `/books/:id`  
This endpoint updates an existing book by ID with a json body.
```
{
  "date": "2023-03-12",
  "author_id": 10,
  "title": "Lord Of The Rings",
  "category": "Fantasy",
  "pages": 469
}
```

DELETE `/books/:id`  
This endpoint deletes a book by ID.

GET `/books/init/:numberOfBooks`  
This endpoint creates a numerous books based on the numberOfBooks parameter.

GET `/books/search?searchTerm={searchTerm}`  
This endpoint searches all books to find relative title, category or author details

# Parse
POST `/parse`  
This endpoint creates a job for bulk insert or update of books.
If a book in JSON has an id property, it will update the book. If not, it will insert it into the database
```
[
  {
    "id": 4,
    "date": "2023-03-23",
    "author_id": 1,
    "title": "Harry Potter",
    "category": "Adventure",
    "pages": 694
  },
  {
    "date": "2023-03-12",
    "author_id": 1,
    "title": "Lord Of The Rings",
    "category": "Fantasy",
    "pages": 469
  }
]
```

# Jobs
GET `/jobs/:id`  
This endpoint retrieves a job and prints the status of it.
```
Queued: The job is queued and it will be process when a worker is available.
In-progress: The job is currently inserting or updating books.
Failed: If a book failed to insert or update the whole job will be marked as failed.
Completed: All books has been processed correctly. 
```