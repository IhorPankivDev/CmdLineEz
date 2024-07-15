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

        public static IEnumerable<TestCaseData> GetDeleteCommandLineWithNotSupportedType()
        {
            yield return new TestCaseData(new DeleteCommandLineWithNotSupportedType(),
            new string[6] { "/confirm", "/char=d", "/Recursive", "/VeRbOsE", "/Prefix", "example" });

            yield return new TestCaseData(new DeleteCommandLineWithNotSupportedType(),
            new string[5] { "/CHAR=somevalue", "/Recursive=somevalue", "/VeRbOsE", "/Prefix=someValue", "example" });

            yield return new TestCaseData(new DeleteCommandLineWithNotSupportedType(),
            new string[1] { "/char=5" });

            yield return new TestCaseData(new DeleteCommandLineWithNotSupportedType
            {
                ConfirmNeeded = true,
                Recursive = true,
                PrintDetails = true,
            }, new string[6] { "/confirm", "/cHar=somevalue", "/RecuRsive", "/VeRbOsE", "/PreFix=    ", "example" });
        }
    }
}
