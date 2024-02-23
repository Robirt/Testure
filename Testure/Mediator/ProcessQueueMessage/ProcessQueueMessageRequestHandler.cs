using MediatR;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Testure.Services;

namespace Testure.Mediator.ProcessQueueMessage;

/// <summary>
/// Process queue message Request Handler.
/// </summary>
public class ProcessQueueMessageRequestHandler : IRequestHandler<ProcessQueueMessageRequest, Unit>
{
    /// <summary>
    /// Container Service.
    /// </summary>
    private readonly IContainerService _containerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessQueueMessageRequestHandler"/> class.
    /// </summary>
    public ProcessQueueMessageRequestHandler(IContainerService containerService)
    {
        ArgumentNullException.ThrowIfNull(containerService);

        _containerService = containerService;
    }

    /// <summary>
    /// Handles processing queue message.
    /// </summary>
    public async Task<Unit> Handle(ProcessQueueMessageRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Content)) throw new ArgumentNullException(nameof(request.Content));

        try
        {
            JsonDocument.Parse(request.Content);

            await _containerService.AddFileAsync(request.Content, cancellationToken);
        }

        catch (JsonException)
        {
            throw;
        }

        return Unit.Value;
    }
}
