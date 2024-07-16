using CmdLineEz.Tests.TestData;
using FluentAssertions;

namespace CmdLineEz.Tests
{
    public class Tests
    {
        [OneTimeSetUp]
        public void RunOnceBeforeAnyTests() { }

        [SetUp]
        public void RunOnceBeforeAllTests() { }


        [TestCaseSource(typeof(DataInitializer), nameof(DataInitializer.GetNullInsteadOfDeleteCommandLine))]
        public void Process_WithEmptyTResult_ShouldThrowAnException(DeleteCommandLine result, string[] args)
        {
            // Act
            Action act = () => CmdLineEz<DeleteCommandLine>.Process(result, args);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("No parameter specification loaded.");
        }


        [TestCaseSource(typeof(DataInitializer), nameof(DataInitializer.GetDeleteCommandLineWithNotSupportedType))]
        public void Process_TemplateClassWithNotSupportedType_ShouldAddInvalidParamNameExceptionToErrors(DeleteCommandLineWithNotSupportedType result, string[] args)
        {
            // Act
            List<string> errors = CmdLineEz<DeleteCommandLineWithNotSupportedType>.Process(result, args);

            // Assert
            errors.Should().NotBeNull("Errors list should not be null.")
                .And.ContainSingle(e => e == "invalid type of char parameter", "Error message should be 'invalid type of char parameter'.");
        }


        [TestCaseSource(typeof(DataInitializer), nameof(DataInitializer.GetDeleteCommandLineWithMissingRequiredParameter))]
        public void Process_TemplateClassWithMissingRequiredParameter_ShouldAddMissingParamExceptionToErrors(DeleteCommandLine result, string[] args)
        {
            // Act
            List<string> errors = CmdLineEz<DeleteCommandLine>.Process(result, args);

            // Assert
            errors.Should().NotBeNull("Errors list should not be null.")
                .And.ContainSingle(e => e == "missing confirm", "Error message should be 'missing confirm'.");
        }


        [TestCaseSource(typeof(DataInitializer), nameof(DataInitializer.GetDeleteCommandLineWithTwoRemainings))]
        public void Process_TemplateClassWithTwoRemainings_ShouldAddRemainingExceptionToErrors(DeleteCommandLineWithTwoRemainings result, string[] args)
        {
            // Act
            List<string> errors = CmdLineEz<DeleteCommandLineWithTwoRemainings>.Process(result, args);

            // Assert
            errors.Should().NotBeNull("Errors list should not be null.")
                .And.ContainSingle(e => e == "There are not allowed more then 1 remaining properties", 
                "Error message should be 'There are not allowed more then 1 remaining properties'.");
        }


        [TestCaseSource(typeof(DataInitializer), nameof(DataInitializer.GetValidDeleteCommandLine))]
        public void Process_TemplateClassWithOneRemainings_ShouldNotAddRemainingExceptionToErrors(DeleteCommandLine result, string[] args)
        {
            // Act
            List<string> errors = CmdLineEz<DeleteCommandLine>.Process(result, args);

            // Assert
            errors?.Should().NotContain(e => e == "There are not allowed more then 1 remaining properties",
                "Error list should NOT contain the message 'There are not allowed more then 1 remaining properties'.");
        }

        [TestCaseSource(typeof(DataInitializer), nameof(DataInitializer.GetArgsWithDublicates))]
        public void Process_ArgsHasDublicateParameters_ShouldAddExceptionToErrors(DeleteCommandLine result, string[] args)
        {
            // Act
            List<string> errors = CmdLineEz<DeleteCommandLine>.Process(result, args);

            // Assert
            errors.Should().Contain(e => e.StartsWith("Ambiguous parameter "),
                "Error list should contain ambiguous parameter error.");
        }
    }
}