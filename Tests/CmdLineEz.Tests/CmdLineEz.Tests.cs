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
        public void Process_DeleteCommandLineWithNotSupportedType_ShouldAddInvalidParamNameExceptionToErrors(DeleteCommandLineWithNotSupportedType result, string[] args)
        {
            // Act
            List<string> errors = CmdLineEz<DeleteCommandLineWithNotSupportedType>.Process(result, args);

            // Assert
            errors.Should().NotBeNull("Errors list should not be null.")
                .And.ContainSingle(e => e == "invalid char", "Error message should be 'invalid char'.");
        }

        [TestCaseSource(typeof(DataInitializer), nameof(DataInitializer.GetDeleteCommandLineWithMissingRequiredParameter))]
        public void Process_DeleteCommandLineWithMissingRequiredParameter_ShouldAddMissingParamExceptionToErrors(DeleteCommandLineWithNotSupportedType result, string[] args)
        {
            // Act
            List<string> errors = CmdLineEz<DeleteCommandLineWithNotSupportedType>.Process(result, args);

            // Assert
            errors.Should().NotBeNull("Errors list should not be null.")
                .And.ContainSingle(e => e == "missing confirm", "Error message should be 'missing confirm'.");
        }
    }
}