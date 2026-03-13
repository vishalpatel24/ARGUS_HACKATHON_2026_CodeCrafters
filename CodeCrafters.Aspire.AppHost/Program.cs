using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// SQL Server resource and database for the backend API.
var sql = builder.AddSqlServer("sql", port: 1433);
var db = sql.AddDatabase("codecraftersdb");

// Backend API project orchestrated by Aspire with database reference.
// Uses the generated Projects.CodeCrafters_Api metadata type from the AppHost SDK.
var api = builder.AddProject<Projects.CodeCrafters_Api>("api")
    .WithReference(db)
    .WaitFor(db);

// Frontend Angular app placeholder orchestrated via NPM (when the app exists).
// This assumes an npm-based Angular app in CodeCrafters.FrontEnd\\codecrafters-angular.
// Uncomment and adjust when the Angular project is initialized with package.json:
//
// var frontend = builder.AddNpmApp(
//         name: "frontend",
//         workingDirectory: "..\\CodeCrafters.FrontEnd\\codecrafters-angular",
//         scriptName: "start")
//     .WithReference(api);

builder.Build().Run();
