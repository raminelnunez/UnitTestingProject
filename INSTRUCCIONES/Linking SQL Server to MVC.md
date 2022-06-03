1. Installing Packages:
   * In Visual Studio we will need to install three packages in NuGet Package Manager (Tools > Nuget Package Manager > Manage Nuget Packages For Solution) or use the Package Manager Console.
Packages to Install:
   * Microsoft.EntityFrameworkCore.SqlServer
   * Microsoft.EntityFrameworkCore.Design
   * Microsoft.EntityFrameworkCore.Tools
                If you use  Package Manager Console to Install:
   * In the console panel, you will need to enter the following commands:
   * Install-Package Microsoft.EntityFrameworkCore.SqlServer
   * Install-Package Microsoft.EntityFrameworkCore.Design
   * Install-Package Microsoft.EntityFrameworkCore.Tools
                
2. Find your Server and Database names
   * Open SMSS and in the Object Explorer panel navigate to your database
   * Right-click on the database name and select properties
   * Within the window that pops up click the “View connection properties” link in the bottom left of the window. 
   * Take note of the “Database” name as well as the “Server Name” we will need these exact names multiple times in later steps


3. Connecting our SMSS Database to Visual Studio
   * In Visual Studio open your Server Explorer (View > Server Explorer or Ctrl  + Alt + S). 
   * Along the top of this panel, there will be a button with a plug icon next to a cylinder that will say “connect to database” when you hover over it. Click this.
   * The first time you click this a “Choose Data Source” window will pop up, go ahead and select “Microsoft SQL Server” and hit continue.  
   * Next an “Add Connection Window” will come up, we will need to enter our Server Name from Step 2
   * After you enter a correct server name the dropdown in the Database section of this window will take a moment to populate, next you will select the Database name from step 2


4. Scaffolding your Project using the Database
   * In the Package Manager Console panel you will need to enter the following command being sure to fill in the blanks with your exact Server Name and exact Database Name”


   * Scaffold-DbContext "Data Source=SERVERNAME;Initial Catalog=DATABASENAME;Integrated Security=True;" Microsoft.EntityFrameworkCore.SQLServer -OutputDir Models


   * This command will "scaffold" a set of Models and a DbContext class in the directory parameter given at the -OutputDir flag.
   * It creates these models from the Server to the provided location, from the specified database.


5. Adding our Connection String to the .json file
   * After our project has been scaffolded an “appsettings.json” file will have been created, find, and open it within your Solution Explorer
   * Our scaffolding will have also created a file within our Models folder thats name will start with our database name followed by the word “Context” we will need to take note of this name.
   * We will need to add the following to the top of this .json file:


   * "ConnectionStrings": {
   "xxxCONTEXT": ""Data Source=SERVERNAME;Initial Catalog=DATABASENAME;Integrated Security=True;"  }


   * This lets our application know where our server is, so when we start reading and writing data it’s going to the server and database we want it to.


   6. Updating our Program with the Connection String
   * This last step is to tell our program where the connection string from our last step is located and that we should use it
   * Find and open the Program.cs file from the solution explorer
   * Find the lines that say:
   * builder.Services.AddControllersWithViews();  and  var app = builder.Build();
   * We will want to input some data between these 2 lines being sure to plug in the exact name of our Context folder that we used in step 5.


// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<xxxCONTEXT>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("xxxCONTEXT")));


var app = builder.Build();


   * Lastly, you will need to be sure you have added the following to the top of this page (being sure to fill in your specific project file name):
   * using Microsoft.EntityFrameworkCore;
   * using PROJECT_FILE_NAME.Models