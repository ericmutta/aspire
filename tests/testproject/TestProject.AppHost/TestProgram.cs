// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;

public class TestProgram
{
    private TestProgram(string[] args, Assembly assembly, bool includeIntegrationServices = false, bool disableDashboard = true, bool includeNodeApp = false)
    {
        AppBuilder = DistributedApplication.CreateBuilder(new DistributedApplicationOptions { Args = args, DisableDashboard = disableDashboard, AssemblyName = assembly.FullName });

        var serviceAPath = Path.Combine(Projects.TestProject_AppHost.ProjectPath, @"..\TestProject.ServiceA\TestProject.ServiceA.csproj");

        ServiceABuilder = AppBuilder.AddProject("servicea", serviceAPath);
        ServiceBBuilder = AppBuilder.AddProject<Projects.ServiceB>("serviceb");
        ServiceCBuilder = AppBuilder.AddProject<Projects.ServiceC>("servicec");
        WorkerABuilder = AppBuilder.AddProject<Projects.WorkerA>("workera");

        if (includeNodeApp)
        {
            // Relative to this project so that it doesn't changed based on
            // where this code is referenced from.
            var path = Path.Combine(Projects.TestProject_AppHost.ProjectPath, @"..\nodeapp");
            var scriptPath = Path.Combine(path, "app.js");

            NodeAppBuilder = AppBuilder.AddNodeApp("nodeapp", scriptPath)
                .WithServiceBinding(hostPort: 5031, scheme: "http", env: "PORT");

            NpmAppBuilder = AppBuilder.AddNpmApp("npmapp", path)
                .WithServiceBinding(hostPort: 5032, scheme: "http", env: "PORT");
        }

        if (includeIntegrationServices)
        {
            var sqlserverContainer = AppBuilder.AddSqlServerContainer("sqlservercontainer");
            var mysqlContainer = AppBuilder.AddMySqlContainer("mysqlcontainer");
            var redisContainer = AppBuilder.AddRedisContainer("rediscontainer");
            var postgresContainer = AppBuilder.AddPostgresContainer("postgrescontainer");
            var rabbitmqContainer = AppBuilder.AddRabbitMQContainer("rabbitmqcontainer");
            var mongodbContainer = AppBuilder.AddMongoDBContainer("mongodbcontainer");
            var sqlserverAbstract = AppBuilder.AddSqlServerContainer("sqlserverabstract");
            var mysqlAbstract = AppBuilder.AddMySqlContainer("mysqlabstract");
            var redisAbstract = AppBuilder.AddRedisContainer("redisabstract");
            var postgresAbstract = AppBuilder.AddPostgresContainer("postgresabstract");
            var rabbitmqAbstract = AppBuilder.AddRabbitMQContainer("rabbitmqabstract");
            var mongodbAbstract = AppBuilder.AddMongoDB("mongodbabstract");

            IntegrationServiceABuilder = AppBuilder.AddProject<Projects.IntegrationServiceA>("integrationservicea")
                .WithReference(sqlserverContainer)
                .WithReference(mysqlContainer)
                .WithReference(redisContainer)
                .WithReference(postgresContainer)
                .WithReference(rabbitmqContainer)
                .WithReference(mongodbContainer)
                .WithReference(sqlserverAbstract)
                .WithReference(mysqlAbstract)
                .WithReference(redisAbstract)
                .WithReference(postgresAbstract)
                .WithReference(rabbitmqAbstract)
                .WithReference(mongodbAbstract);
        }
    }

    public static TestProgram Create<T>(string[]? args = null, bool includeIntegrationServices = false, bool includeNodeApp = false, bool disableDashboard = true) =>
        new TestProgram(args ?? [], typeof(T).Assembly, includeIntegrationServices, disableDashboard, includeNodeApp: includeNodeApp);

    public IDistributedApplicationBuilder AppBuilder { get; private set; }
    public IResourceBuilder<ProjectResource> ServiceABuilder { get; private set; }
    public IResourceBuilder<ProjectResource> ServiceBBuilder { get; private set; }
    public IResourceBuilder<ProjectResource> ServiceCBuilder { get; private set; }
    public IResourceBuilder<ProjectResource> WorkerABuilder { get; private set; }
    public IResourceBuilder<ProjectResource>? IntegrationServiceABuilder { get; private set; }
    public IResourceBuilder<NodeAppResource>? NodeAppBuilder { get; private set; }
    public IResourceBuilder<NodeAppResource>? NpmAppBuilder { get; private set; }
    public DistributedApplication? App { get; private set; }

    public List<IResourceBuilder<ProjectResource>> ServiceProjectBuilders => [ServiceABuilder, ServiceBBuilder, ServiceCBuilder];

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        App = AppBuilder.Build();
        await App.RunAsync(cancellationToken);
    }

    public DistributedApplication Build()
    {
        if (App == null)
        {
            App = AppBuilder.Build();
        }
        return App;
    }

    public void Run()
    {
        Build();
        App!.Run();
    }
}

