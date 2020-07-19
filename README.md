# Exchangerat

Simple payments platform, that allows the users to transfer money between their own accounts or to transfer money to other users' accounts.

### The application consists of the following features:
**1. Administration Client app (admin docker container)**
 - The administrator can login from the admin client and can Approve/Cancel clients' requests.
 
**2. Regular Client app (frontend docker container)**
 - Annonymous users can login or register in the platform.
 - Authenticated users can send requests for new accounts, for blocking and closing existing account.
 - Authenticated users can see their own accounts, can see account details.
 - Authenticated users can create transaction between own and other users' accounts.
 - Authenticated users can add funds to their accounts via credit/debit cards.

### Hints:
To login as an administrator (because the administrator is seeded and is only one user) use the following credentials:
 - **Username**: exchangerat 
 - **Password**: asddsa
