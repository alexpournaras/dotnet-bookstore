# CW2 Expanding the system

#### NOTE
---
This is not the coursework handout. This is just a quick reference project readme.  
Please refer to the official handout from google classroom for more details.


#### The task
---
Extremely pleased from your performance and the previous deliverables, the company is now requesting you to extend upon the previously submitted work.

During the project briefing, the company is sharing some security concerns with you. They would like you to enhance the previous endpoints with a token based authentication system and incoming throttling (20 Requests per 2 Seconds) in order to avoid any surprises.


Also, they are providing you with an external service that they would like to consume. This is an External REST API that provides IP address lookup and related services. Using this api (https://ipapi.co/), you are requested to protect the creation and deletion operations and restrict the origin of the requests to Greece. Any request coming for create and delete from any other country should be denied. 

Other than that, you are requested to enhance all relevant services and/or repositories with in-app caching to optimise the usage of the persistent layer and external dependencies.


In order to keep up with the high quality of your previous work, you should focus on delivering fast, high quality code within specs. Proper HTTP responses/codes and good test coverage (above 80%)

To help you out with the requirement extraction, an in-house analyst is providing you with some user stories to include with your requirements.

Note: In order to use the third party api (https://ipapi.co/) you will have to create a free account and follow the API reference docs (https://ipapi.co/api/) in order to properly integrate the service.

--- 

#### User stories

As a Developer, i want to create an Author so that i can enhance the author dataset with more entries.  

As a Developer, i want to edit an Author so that i can update the Author’s information.  

As a Developer, i want to delete an Author so that i can remove the Author’s information from the dataset.  

As a Developer, i want to create a Book so that i can make new Books available for management.   

As a Developer, i want to edit a Book so that i can update the Book’s information.  

As a Developer, i want to delete a Book so that i can remove the Book’s information from the dataset.  

As a Developer, i want to link a Book to an Author so that i can associate the Author with the published Book.  

As a User, i want to view the full list of Authors so that i can keep track of the Author entries.  

As a User, i want to view the full list of Books so that i can keep track of the Books entries.  

As a User, i want to search the Books so that i can find Books based on the Publication date.  

As a User, i want to search the Books so that i can find Books based on the Title.  

As a User, i want to search the Books so that i can find Books based on the Category.  

As a User, i want to search the Books so that i can find Books based on the Author.  

As a Developer, i want to view the full list of Authors so that i can keep track of the Author entries.  

As a Developer, i want to view the full list of Books so that i can keep track of the Books entries.  

As a Developer, i want to search the Books so that i can find Books based on the Publication date.  

As a Developer, i want to search the Books so that i can find Books based on the Title.  

As a Developer, i want to search the Books so that i can find Books based on the Category.  

As a Developer, i want to search the Books so that i can find Books based on the Author.
