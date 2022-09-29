using System.Runtime.Serialization;
using Amusoft.Toolkit.Http.UnitTests.Toolkit;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.Toolkit.Http.UnitTests
{
    public class DownloadContainerTests : TestBase
    {
        [Fact]
        public void DivisionByZeroDoesNotThrow()
        {
	        var instance = FormatterServices.GetUninitializedObject(typeof(DownloadContainer)) as DownloadContainer;
            instance.ReportProgress(0,5);

        }

        public DownloadContainerTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper, data)
        {
        }
    }
}
