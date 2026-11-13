## **Bank Application - Part A**



##### Requirements



.NET 6.0 SDK

Visual Studio

SQLite database



##### How to run



Open the PartA\_BankDataWeb

* click the PartA\_BankDataWeb.slnx
* Then click the run button in the visul studio
* the API will be available at http://localhost:5084



##### Seed users login credentials



username: SeedUser1

password: Bank123



##### Run and check these in Postman after running the application



* method: GET / URL: http://localhost:5084/api/Users/username/SeedUser1 - retrieve SeedUser details



* method: POST / URL: http://localhost:5084/api/Users/login

&#x20;   Then go to Body -> raw -> JSON

&#x20;   Type this

&#x20;   {

&#x20;       "username": "SeedUser1",

&#x20;       "password": "Bank123"

&#x20;   }

&#x20;   This for login



* method: GET / URL: http://localhost:5084/api/Accounts/SEED0002 - To retrieve SEED02's account details

