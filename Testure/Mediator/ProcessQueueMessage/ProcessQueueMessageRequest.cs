using MediatR;

namespace Testure.Mediator.ProcessQueueMessage;

/// <summary>
/// Process queue message Request.
/// </summary>
public class ProcessQueueMessageRequest : IRequest<Unit>
{
    /// <summary>
    /// Content.
    /// </summary>
    public string Content { get; set; } = string.Empty;
}
