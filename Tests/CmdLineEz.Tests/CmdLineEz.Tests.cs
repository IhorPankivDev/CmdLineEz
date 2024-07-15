namespace CmdLineEz.Tests
{
    public class Tests
    {
        [OneTimeSetUp]
        public void RunOnceBeforeAnyTests() { }

        [SetUp]
        public void RunOnceBeforeAllTests() { }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}