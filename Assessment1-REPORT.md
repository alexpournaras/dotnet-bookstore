# CRUD
You can find all the CRUD actions with examples in post/put requests in the CRUD.md file.

# Search Endpoint
The endpoint sends the searchTerm of the user to the LibraryService and from there to LibraryDatalayer. There, the datalayer perform a query on the database that searches for book title, book category, author name and country and returns to the user all the books that was found.

# Bulk Upsert
On the endpoint /parse the user can send through POST request a JSON of books and add them into the database or update them. This is being done in the background. When the user sends the JSON, the WorkerService creates a unique job that contains all the books. The job is being added to a queue and the Id of the job is returned to the user.

When a worker becomes available, it takes a job and creates batches of the job's books. Those batches can perform the upsert in parallel based on the number of available workers. For each book of the each batch, on the time of the upsert in the database, we lock the _dbInstance so we can avoid transactions mess up with each others. If all actions succeed, the job is silently finished in the background.

#### Example Request
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

#### Example Response
A job has been queued with ID: 687bb0e4-1713-472b-982a-7dfd7fa93284

# Job Status
The endpoint /parse returns to the user a unique Id of the created job. The user can find out in which stage the job is at /jobs/{id}

Possible responses: `Queued`, `In-progress`, `Completed`, `Failed`