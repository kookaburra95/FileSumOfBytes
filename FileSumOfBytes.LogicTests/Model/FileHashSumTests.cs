using FileSumOfBytes.Logic.Model;
using NUnit.Framework;

namespace FileSumOfBytes.LogicTests.Model
{
    [TestFixture()]
    public class FileHashSumTests
    {
        [Test]
        [TestCase("abc.bin", "33345F90A97D51507CBC27CCEDA8D8AC")]
        public void FileHashSumTest(string name, string hashSum)
        {
            var file = new FileHashSum("abc.bin", "33345F90A97D51507CBC27CCEDA8D8AC");
            Assert.AreEqual(name, file.Name);
            Assert.AreEqual(hashSum, file.HashSum);
        }
    }
}
