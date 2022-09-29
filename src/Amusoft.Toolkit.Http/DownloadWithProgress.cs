using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Threading;
using Amusoft.Toolkit.Http.Delegates;

namespace Amusoft.Toolkit.Http;

public static class DownloadWithProgress
{

	/// <summary>
	/// Downloads from a given url to a destination while providing information about the download progress
	/// </summary>
	/// <param name="httpClient">HttpClient instance</param>
	/// <param name="downloadUrl">remote url to start the download from</param>
	/// <param name="destinationPath">path to store the incomming response stream</param>
	/// <param name="progress">handler event to report progress</param>
	/// <param name="requestMessageBuilder">configuration of the outgoing http request to potentially add headers</param>
	/// <param name="cancellationToken">cancellation token to abort the download process</param>
	/// <returns>Once the download is complete this task will return</returns>
	public static async Task ExecuteAsync(HttpClient httpClient, string downloadUrl, string destinationPath, DownloadProgressHandler progress, Func<HttpRequestMessage>? requestMessageBuilder = null, CancellationToken cancellationToken = default)
	{
		requestMessageBuilder ??= GetDefaultRequestBuilder(downloadUrl);
		var download = new DownloadContainer(httpClient, destinationPath, requestMessageBuilder);
		download.ProgressChanged += progress;
		await download.StartDownload(cancellationToken).ConfigureAwait(false);
		download.ProgressChanged -= progress;
	}

	private static Func<HttpRequestMessage> GetDefaultRequestBuilder(string downloadPath)
	{
		return () => new HttpRequestMessage(HttpMethod.Get, downloadPath);
	}
}