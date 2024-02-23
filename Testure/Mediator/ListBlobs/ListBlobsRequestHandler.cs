using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Testure.Services;

namespace Testure.Mediator.ListBlobs;

/// <summary>
/// List blobs Request Handler.
/// </summary>
public class ListBlobsRequestHandler : IRequestHandler<ListBlobsRequest, List<string>>
{
    /// <summary>
    /// Container Service.
    /// </summary>
    private readonly IContainerService _containerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListBlobsRequestHandler"/> class.
    /// </summary>
    public ListBlobsRequestHandler(IContainerService containerService)
    {
        ArgumentNullException.ThrowIfNull(containerService);

        _containerService = containerService;
    }

    /// <summary>
    /// Handles listing blobs.
    /// </summary>
    public async Task<List<string>> Handle(ListBlobsRequest request, CancellationToken cancellationToken)
    {
        return await _containerService.GetFilesNamesAsync(cancellationToken);
    }
}
