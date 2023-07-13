This project is based on ASP.NET Core Web API Version 7.0

It uses a InMemory database to extract list of To Dd Items. Application comprises of two controllers - one is ToDo controller which has all the APIs related to CRUD operations and other is the Auth controller which comprises of APIs related to user login and registration.

On successful registration, a valid JWT token is generated which can be used to access various CRUD APIs in ToDo controller based on user roles.

APIs description -

ToDo Controller :
GetAllItems() : Gets list of all ToDo items based on User Roles.

GetItemsById() : Takes id as input parameter and fetches only that record which matches the input id provided. (Based on user Roles)

UpdateSingleToDoItem() : Takes id as input parameter, fetches the corresponding record from DB and updates the record based on changes supplied by user. (Based on user Roles)

CreateToDoItems() : Creates a new record with the details supplied by user and adding the record in InMemory Database. (Based on user Roles)

DeleteItemsById() : Deletes a record based on id supplied by user if fpound in InMemory DB.(Based on user Roles)

AuthController :
Register() : This API is responsible for registering a new employee with all required details supplied by user and verifying those details as well.

Login() : This API is responsible for logging in the user if the credentials supplied by user matches with the credentials present in our DB. On successfull login, a JwT token is generated which has details of user logged in.
