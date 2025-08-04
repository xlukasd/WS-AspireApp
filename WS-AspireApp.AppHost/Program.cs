var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.WS_AspireApp_ApiService>("apiservice");

builder.AddProject<Projects.WS_AspireApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
