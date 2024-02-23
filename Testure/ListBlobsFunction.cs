using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Testure.Mediator.ListBlobs;

namespace Testure;

/// <summary>
/// List blobs Function.
/// </summary>
public class ListBlobsFunction
{
    /// <summary>
    /// Mediator.
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Logger.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListBlobsFunction"/> class.
    /// </summary>
    public ListBlobsFunction(IMediator mediator, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        ArgumentNullException.ThrowIfNull(loggerFactory);

        _mediator = mediator;
        _logger = loggerFactory.CreateLogger<ListBlobsFunction>();
    }

    /// <summary>
    /// Lists blobs.
    /// </summary>
    [Function("ListBlobs")]
    public async Task<List<string>> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData httpRequestData)
    {
        try
        {
            return await _mediator.Send(new ListBlobsRequest());
        }

        catch (Exception exception)
        {
            _logger.LogError(exception, "Error listing blobs");
            throw;
        }
    }
}
