using NSubstitute;

namespace CmdLineEz.Tests.TestData
{
    public class DataInitializer
    {
        public static IEnumerable<TestCaseData> GetNullInsteadOfDeleteCommandLine()
        {
            yield return new TestCaseData(null, null);
            yield return new TestCaseData(null, new string[] { });
            yield return new TestCaseData(null, new string[3] { "Example", null!, string.Empty });
            yield return new TestCaseData(null, new string[5] { "", "", "", "", "" });

        }
    }
}
