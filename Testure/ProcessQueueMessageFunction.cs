using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Testure.Mediator.ProcessQueueMessage;

namespace Testure;

/// <summary>
/// Process queue message Function.
/// </summary>
public class ProcessQueueMessageFunction
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
    /// Initializes a new instance of the <see cref="ProcessQueueMessageFunction"/> class.
    /// </summary>
    public ProcessQueueMessageFunction(IMediator mediator, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        ArgumentNullException.ThrowIfNull(loggerFactory);

        _mediator = mediator;
        _logger = loggerFactory.CreateLogger<ProcessQueueMessageFunction>();
    }

    /// <summary>
    /// Processes queue message.
    /// </summary>
    [Function("ProcessQueueMessage")]
    public async Task RunAsync([QueueTrigger("%QueueName%", Connection = "AzureStorageAccountConnectionString")] string item)
    {
        try
        {
            await _mediator.Send(new ProcessQueueMessageRequest { Content = item });
            _logger.LogInformation("Processed queue message: {item}", item);
        }

        catch (Exception exception)
        {
            _logger.LogError(exception, "Error processing queue message: {item}", item);
            throw;
        }
    }
}
