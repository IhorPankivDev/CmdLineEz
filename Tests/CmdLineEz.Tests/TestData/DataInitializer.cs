using NSubstitute;

namespace CmdLineEz.Tests.TestData
{
    public class DataInitializer
    {
        public static IEnumerable<TestCaseData> GetValidDeleteCommandLine()
        {
            yield return new TestCaseData(new DeleteCommandLine(),
            new string[7] { "/Confirm", "/Recursive", "/VeRbOsE", "/Prefix", "example", "example2", "example3" });

            yield return new TestCaseData(new DeleteCommandLine(),
            new string[5] { "/Confirm", "/VeRbOsE", "/Prefix=someValue", "example", "" });

            yield return new TestCaseData(new DeleteCommandLine(),
            new string[2] { "/Confirm", "/Recursive" });

            yield return new TestCaseData(new DeleteCommandLine(), 
            new string[6] { "/conFIrm", "/RecuRsive", "/VeRbOsE", "/PreFix=    ", "example", "" });
        }

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

        public static IEnumerable<TestCaseData> GetDeleteCommandLineWithMissingRequiredParameter()
        {
            yield return new TestCaseData(new DeleteCommandLine(),
            new string[4] { "/Recursive", "/VeRbOsE", "/Prefix", "example"});

            yield return new TestCaseData(new DeleteCommandLine(),
            new string[5] { "/Recursive=somevalue", "/VeRbOsE", "/Prefix=someValue", "example" , "" });

            yield return new TestCaseData(new DeleteCommandLine(),
            new string[1] { "/Recursive=" });

            yield return new TestCaseData(new DeleteCommandLine
            {
                ConfirmNeeded = true,
                Recursive = true,
                PrintDetails = true,
            }, new string[6] { "confirm", "/RecuRsive", "/VeRbOsE", "/PreFix=    ", "example", "" });
        }

        public static IEnumerable<TestCaseData> GetDeleteCommandLineWithTwoRemainings()
        {
            yield return new TestCaseData(new DeleteCommandLineWithTwoRemainings(),
            new string[3] { "/Recursive", "/VeRbOsE", "/Prefix" });

            yield return new TestCaseData(new DeleteCommandLineWithTwoRemainings(),
            new string[5] { "/Recursive=somevalue", "/VeRbOsE", "/Prefix=someValue", "example", "" });

            yield return new TestCaseData(new DeleteCommandLineWithTwoRemainings(),
            new string[1] { "/Recursive=   " });

            yield return new TestCaseData(new DeleteCommandLineWithTwoRemainings
            {
                ConfirmNeeded = true,
                Recursive = true,
                PrintDetails = true,
            }, new string[6] { "confirm", "/RecuRsive", "/VeRbOsE", "/PreFix=    ", "example", "" });
        }

        public static IEnumerable<TestCaseData> GetArgsWithDublicates()
        {
            yield return new TestCaseData(new DeleteCommandLine(),
            new string[8] { "/Confirm", "/Recursive", "/VeRbOsE", "/Prefix", "/Recursive", "example", "example2", "example3" });

            yield return new TestCaseData(new DeleteCommandLine(),
            new string[6] { "/VeRbOsE", "/Confirm", "/VeRbOsE", "/Prefix=someValue", "example", "" });

            yield return new TestCaseData(new DeleteCommandLine(),
            new string[3] { "/Confirm", "/Recursive", "/Confirm"});

            yield return new TestCaseData(new DeleteCommandLine(),
            new string[7] { "/conFIrm", "/RecuRsive", "/VeRbOsE", "/PreFix=    ", "/PreFix=SomeValue", "example", "" });
        }

        public static IEnumerable<TestCaseData> GetValidCommandLines()
        {
            yield return new TestCaseData(
                "delete /confirm file1.txt file2.txt", 
                new DeleteCommandLine 
                { 
                    ConfirmNeeded = true, 
                    Recursive = false, 
                    PrintDetails = false, 
                    Prefix = null!, 
                    Remaining = new List<string> { "file1.txt", "file2.txt" }
                });

            yield return new TestCaseData(
                "delete /confirm /recursive file1.txt file2.txt", 
                new DeleteCommandLine
                {
                    ConfirmNeeded = true, 
                    Recursive = true, 
                    PrintDetails = false, 
                    Prefix = null!, 
                    Remaining = new List<string>{"file1.txt", "file2.txt" }
                });

            yield return new TestCaseData(
                "delete /confirm /verbose file1.txt file2.txt", 
                new DeleteCommandLine
                {
                    ConfirmNeeded = true, 
                    Recursive = false, 
                    PrintDetails = true, 
                    Prefix = null!, 
                    Remaining = new List<string>{"file1.txt", "file2.txt" }
                });

            yield return new TestCaseData(
                "delete /verbose /confirm file1.txt file2.txt", 
                new DeleteCommandLine
                {
                    ConfirmNeeded = true, 
                    Recursive = false, 
                    PrintDetails = true, 
                    Prefix = null!, 
                    Remaining = new List<string>{"file1.txt", "file2.txt" }
                });

            yield return new TestCaseData(
                "delete /confirm /prefix=prefix /verbose file1.txt file2.txt", 
                new DeleteCommandLine
                {
                    ConfirmNeeded = true, 
                    Recursive = false, 
                    PrintDetails = true, 
                    Prefix = "prefix", 
                    Remaining = new List<string>{"file1.txt", "file2.txt" }
                });

            yield return new TestCaseData(
                "delete /VERBOSE /confirm file1.txt file2.txt file3.txt file4.txt file5.txt file6.txt -file7", 
                new DeleteCommandLine
                {
                    ConfirmNeeded = true, 
                    Recursive = false, 
                    PrintDetails = true, 
                    Prefix = null!, 
                    Remaining = new List<string>{"file1.txt", "file2.txt", "file3.txt", "file4.txt", "file5.txt", "file6.txt", "-file7" }
                });

            yield return new TestCaseData(
                "delete /confirm", 
                new DeleteCommandLine
                {
                    ConfirmNeeded = true, 
                    Recursive = false, 
                    PrintDetails = false, 
                    Prefix = null!,
                    Remaining = new List<string>()
                });

            yield return new TestCaseData(
                "someValue /confirm", 
                new DeleteCommandLine
                {
                    ConfirmNeeded = true,
                    Recursive = false,
                    PrintDetails = false,
                    Prefix = null!,
                    Remaining = new List<string>()
                });

            yield return new TestCaseData(
                "someValue /confirm /verbose", 
                new DeleteCommandLine
                {
                    ConfirmNeeded = true, 
                    Recursive = false, 
                    PrintDetails = true, 
                    Prefix = null!, 
                    Remaining = new List<string>()
                });

            yield return new TestCaseData(
                "someValue /confirm /RecurSivE file1.txt file2.txt", 
                new DeleteCommandLine
                {
                    ConfirmNeeded = true, 
                    Recursive = true, 
                    PrintDetails = false, 
                    Prefix = null!, 
                    Remaining = new List<string>{"file1.txt", "file2.txt" }
                });

        }
    }
}
