The System has two parts UI and Backend

UI Part is developed using ANGULAR:
It requires Node and angular CLI to build and run.

CURRENT VERISON USED
Angular CLI: 17.3.0
Node: 20.11.1
Package Manager: npm 10.5.0


You may also need to install angular material module from npm (but wait not now, will only use if required)
ng add @angular/material


For backend I have used VS 2019 (Version 16.10.1) it has .NET 5 (CORE) so please make sure to install it.

Steps to run the system:

Unzip the folder Schenider it contains 2 sub folders 

UI (contains the Angular code)
Backend (contains .net code solution)

To build back end

First execute the CreateDatabase.sql to populate the desired schema. You can find it in solution diretory under Database folder.
Then using VS, open the solution and update the connection string in appsettings.json.

now press F5 it should show Swagger page with all the end points.

for front end navigate to ThesaurusUI using npm console. And run the below on npm command promp
ng serve
it will compile and run the angular on local host
It will start the Angular UI and host it on local host. you can accs it at http://localhost:4200/
