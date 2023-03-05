#### NOTE
---
This is not the coursework handout. This is just a quick reference project readme.  
Please refer to the official handout from google classroom for more details.


#### The task
---
Taking the role of a backend engineer, you are requested to design and develop a system that will manage books.

A book entity must have a title, a publication date, a category, an author and the number of pages in it.

An author entity must have a first name, last name, country and number of books published.

Your system should fulfil all of the following requirements:

- Using the provided postgres (docker image) design your database tables that will store your books
- Support all CRUD (Create, Read, Update, Delete) operations
- Provide a search endpoint that will allow the API caller to search based on all book attributes (title, author, etc)
- Provide a bulk insert endpoint. This endpoint should support an operation for inserting and updating book details in big batches. First, it must expose a POST method that will allow the caller to provide a JSON array of items that should be inserted/updated. The caller should get a unique identifier for the job as a response indicating that the job is queued for processing. Posted items should be put into a buffer for parallel processing. These will be processed in batches of 10. (Find a reasonable size of parallel jobs that fits your hardware limitations)
- Provide an endpoint for checking the status of the bulk operation / job. Example of potential status of your job can be: Queued, In-progress, Completed, etc
- All endpoints provided should respect the RESTful principles with appropriate HTTP codes for relevant errors, etc

#### Don't
*Do not* use entity framework or any other ORM, use only raw sql  
*Do not* make views, there is no need for UI  
