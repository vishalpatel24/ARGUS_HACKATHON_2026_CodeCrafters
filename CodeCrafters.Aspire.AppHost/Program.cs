using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// SQL Server resource and database for the backend API.
var sql = builder.AddSqlServer("sql", port: 1433);
var db = sql.AddDatabase("codecraftersdb");

// Backend API project orchestrated by Aspire with database reference.
// Uses the generated Projects.CodeCrafters_Api metadata type from the AppHost SDK.
// Configure the existing "http" endpoint on a fixed port so the Angular proxy (proxy.conf.json → localhost:5000) can target it.
var api = builder.AddProject<Projects.CodeCrafters_Api>("api")
    .WithHttpEndpoint(name: "api-http", port: 5000)
    .WithReference(db)
    .WaitFor(db);

// Frontend Angular app: runs "npm start" (ng serve --proxy-config proxy.conf.json --open).
// Use distinct endpoint name to avoid conflict with any default "http" endpoint.
var frontend = builder.AddJavaScriptApp("frontend", "../CodeCrafters.FrontEnd/codecrafters-ui", "start")
    .WithHttpEndpoint(name: "web")
    .WithExternalHttpEndpoints()
    .WaitFor(api);

builder.Build().Run();
