using System;

namespace Amusoft.Toolkit.Http;

/// <summary>
/// Data holder EventArgs for download progress
/// </summary>
public readonly ref struct DownloadProgressEventArgs
{
	/// <summary>
	/// Total download size
	/// </summary>
	public long TotalFileSize { get; init; }

	/// <summary>
	/// Currently downloaded bytes
	/// </summary>
	public long Downloaded { get; init; }

	/// <summary>
	/// Progress as 0.0 - 1.0
	/// </summary>
	public double Progress { get; init; }

	/// <summary>
	/// Progress as 0.0 - 100.0
	/// </summary>
	public double ProgressMax100 { get; init; }
}