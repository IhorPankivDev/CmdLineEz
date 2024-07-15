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
    }
}