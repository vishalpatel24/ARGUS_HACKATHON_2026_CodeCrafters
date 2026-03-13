using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// SQL Server resource and database for the backend API.
var sql = builder.AddSqlServer("sql", port: 1433)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);
var db = sql.AddDatabase("codecraftersdb");

// Backend API: build from Dockerfile using solution root as build context.
var api = builder.AddContainer("api", "codecrafters-api")
    .WithDockerfile("..", "CodeCrafters.BackEnd/CodeCrafters.Api/Dockerfile")
    .WithHttpEndpoint(name: "api-http", port: 6700, targetPort: 5000)
    .WithReference(db)
    .WaitFor(db);

// Frontend Angular: build from Dockerfile using WithDockerfile.
var frontend = builder.AddContainer("frontend", "codecrafters-frontend")
    .WithDockerfile("../CodeCrafters.FrontEnd/codecrafters-ui")
    .WithHttpEndpoint(name: "web", port: 4300, targetPort: 4200)
    .WithExternalHttpEndpoints()
    .WaitFor(api);

builder.Build().Run();
