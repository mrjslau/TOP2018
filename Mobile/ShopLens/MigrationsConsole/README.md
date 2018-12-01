## Working with DB

The *ShopLensWeb* project contains all the models and db contexts that you need.

The *MigrationsConsole* project has links to those models. This way
migrations can be produced from a simple console application. From this 
project you can also write queries, fill the DB with data, etc.

### Using the local DB

Set the "UseLocalDb" flag in the App.config of *MigrationsConsole* to "FALSE".
Pass *true* to the ShopLensContext's *useLocalDb* parameter.

The database named "ShopLens.db" will be created in your Documents folder.

### Using the azure DB

Check and see if your DB connection string is correct. Pass the connection string
to the db context. If you're using *MigrationsConsole* project, set the 
"UseLocalDb" flag in App.config to "TRUE".