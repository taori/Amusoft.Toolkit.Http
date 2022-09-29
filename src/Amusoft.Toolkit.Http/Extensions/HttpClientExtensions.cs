using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.Toolkit.Http.Delegates;

namespace Amusoft.Toolkit.Http.Extensions;

public static class HttpClientExtensions
{
	/// <summary>
	/// Downloads from a given url to a destination while providing information about the download progress
	/// </summary>
	/// <param name="source">HttpClient instance</param>
	/// <param name="downloadUrl">remote url to start the download from</param>
	/// <param name="destinationPath">path to store the incomming response stream</param>
	/// <param name="progress">handler event to report progress</param>
	/// <param name="requestMessageBuilder">configuration of the outgoing http request to potentially add headers</param>
	/// <param name="cancellationToken">cancellation token to abort the download process</param>
	/// <returns>Once the download is complete this task will return</returns>
	public static async Task DownloadWithProgressAsync(this HttpClient source, string downloadUrl, string destinationPath, DownloadProgressHandler progress, Func<HttpRequestMessage>? requestMessageBuilder = default, CancellationToken cancellationToken = default)
	{
		await DownloadWithProgress.ExecuteAsync(source, downloadUrl, destinationPath, progress, requestMessageBuilder, cancellationToken)
			.ConfigureAwait(false);
	}
}