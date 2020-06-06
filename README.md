# Auth Server

> Simple Authentication Server who serves access tokens

A simple authentication server, just for demonstration, has 2 users in the repository. Each user has their own role, employee or manager, and can access their respective routes only with an access token. The employee can only access the employee route but not the management route and the manager can access both.

---
## Version 1

### Users
 - batman
 - robin
 
Password is respective for each username.

### Routes:

- /v1/account/login
- /v1/account/anonymous
- /v1/account/employee
- /v1/account/manager

## Version 2

### Users
You will need to register a user.

### Routes:
- /v2/register
- /v2/login
- /v2/refresh
- /v2/user

