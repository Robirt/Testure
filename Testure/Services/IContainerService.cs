using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Testure.Services;

/// <summary>
/// Container Service.
/// </summary>
public interface IContainerService
{
    /// <summary>
    /// Gets the names of the files in the container.
    /// </summary>
    public Task<List<string>> GetFilesNamesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Adds a file to the container.
    /// </summary>
    public Task AddFileAsync(string content, CancellationToken cancellationToken);
}
