using MediatR;
using System.Collections.Generic;

namespace Testure.Mediator.ListBlobs;

/// <summary>
/// List blobs Request.
/// </summary>
public class ListBlobsRequest : IRequest<List<string>>
{
}
