using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Amusoft.Toolkit.Http.Extensions;
using Amusoft.Toolkit.Http.UnitTests.Toolkit;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.Toolkit.Http.UnitTests
{
	public class DownloadWithProgressTests : TestBase
	{
		private static readonly NLog.ILogger Log = NLog.LogManager.GetLogger(nameof(DownloadWithProgressTests));

		public DownloadWithProgressTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper, data)
		{
		}

		[Fact]
		public async Task VerifyProgressReportingHappens()
		{
			using var client = new HttpClient();
			var downloadUrl = "https://download.visualstudio.microsoft.com/download/pr/a0832b5a-6900-442b-af79-6ffddddd6ba4/e2df0b25dd851ee0b38a86947dd0e42e/dotnet-runtime-5.0.17-win-x64.exe";
			var tempFileName = Path.GetTempFileName();
			var progressMax100 = new List<double?>();
			var progressList = new List<double?>();
			try
			{
				await client.DownloadWithProgressAsync(downloadUrl, tempFileName, progress =>
				{
					progressList.Add(progress.Progress);
					progressMax100.Add(progress.ProgressMax100);
				});
				progressMax100.Count.ShouldBeGreaterThan(0);
				progressList.Count.ShouldBe(progressMax100.Count);
			}
			finally
			{
				File.Delete(tempFileName);
			}
		}
	}
}