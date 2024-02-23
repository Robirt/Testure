using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Testure.Services;

/// <summary>
/// Implemention of the Container Service.
/// </summary>
public class ContainerService : IContainerService
{
    /// <summary>
    /// Blob Service Client.
    /// </summary>
    private readonly BlobContainerClient _blobContainerClient;

    /// <summary>
    /// Testure Options.
    /// </summary>
    private readonly TestureOptions _testureOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContainerService"/> class.
    /// </summary>
    public ContainerService(BlobServiceClient blobServiceClient, IOptions<TestureOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(blobServiceClient);

        _testureOptions = options.Value;
        _blobContainerClient = blobServiceClient.GetBlobContainerClient(_testureOptions.ContainerName);
    }

    /// <inheritdoc/>
    public async Task<List<string>> GetFilesNamesAsync(CancellationToken cancellationToken)
    {
        var filesNames = new List<string>();

        await foreach (var blobItem in _blobContainerClient.GetBlobsAsync(cancellationToken: cancellationToken))
        {
            filesNames.Add(blobItem.Name);
        }

        return filesNames;
    }

    /// <inheritdoc/>
    public async Task AddFileAsync(string content, CancellationToken cancellationToken)
    {
        var blobName = $"{DateTime.UtcNow.Year}/{DateTime.UtcNow.Month}/{DateTime.UtcNow.Day}/{DateTime.UtcNow.Hour}/{DateTime.UtcNow.Minute}/{Guid.NewGuid()}.json";

        using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
        {
            await _blobContainerClient.UploadBlobAsync(blobName, memoryStream, cancellationToken);
        }
    }
}
