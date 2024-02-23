using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Testure.Services;

namespace Testure;

/// <summary>
/// Program.
/// </summary>
public static class Program
{
    /// <summary>
    /// Initializes static members of the <see cref="Program"/> class.
    /// </summary>
    static Program()
    {
        _configuration = new ConfigurationBuilder().AddJsonFile("local.settings.json", false, true).AddEnvironmentVariables().Build();
        _host = new HostBuilder().ConfigureFunctionsWorkerDefaults().ConfigureServices(ConfigureServices).ConfigureStorageAccount().Build();
    }

    /// <summary>
    /// Host.
    /// </summary>
    private static readonly IHost _host;

    /// <summary>
    /// Configuration.
    /// </summary>
    private static IConfiguration _configuration;

    /// <summary>
    /// Main entry point.
    /// </summary>
    public static async Task Main()
    {
        await _host.RunAsync();
    }

    /// <summary>
    /// Configures services.
    /// </summary>
    /// <param name="serviceCollection">Service collection.</param>
    private static void ConfigureServices(IServiceCollection serviceCollection)
    {
        var queueName = _configuration.GetValue<string>("QueueName") ?? throw new InvalidOperationException("QueueName is required.");
        var containerName = _configuration.GetValue<string>("ContainerName") ?? throw new InvalidOperationException("ContainerName is required.");
        serviceCollection.AddOptions<TestureOptions>().Configure(options => { options.QueueName = queueName; options.ContainerName = containerName; });

        serviceCollection.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(typeof(Program).Assembly));

        serviceCollection.AddScoped<IContainerService, ContainerService>();

        var azureStorageAccountConnectionString = _configuration.GetValue<string>("AzureStorageAccountConnectionString") ?? throw new InvalidOperationException("AzureStorageAccountConnectionString is required.");
        serviceCollection.AddAzureClients(builder => { builder.AddQueueServiceClient(azureStorageAccountConnectionString); builder.AddBlobServiceClient(azureStorageAccountConnectionString); });
    }

    private static IHostBuilder ConfigureStorageAccount(this IHostBuilder hostBuilder)
    {
        var azureStorageAccountConnectionString = _configuration.GetValue<string>("AzureStorageAccountConnectionString") ?? throw new InvalidOperationException("AzureStorageAccountConnectionString is required.");

        hostBuilder.ConfigureServices(services =>
        {
            var serviceProvider = services.BuildServiceProvider();

            var queueClient = serviceProvider.GetRequiredService<QueueServiceClient>().GetQueueClient(_configuration.GetValue<string>("QueueName") ?? throw new InvalidOperationException("QueueName is required."));
            queueClient.CreateIfNotExists();

            var blobContainerClient = serviceProvider.GetRequiredService<BlobServiceClient>().GetBlobContainerClient(_configuration.GetValue<string>("ContainerName") ?? throw new InvalidOperationException("ContainerName is required."));
            blobContainerClient.CreateIfNotExists();
        });

        return hostBuilder;
    }
}
