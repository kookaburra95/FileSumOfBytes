using System;
using System.IO;
using FileSumOfBytes.Logic.Model;
using FileSumOfBytes.LogicTests.Properties;
using NUnit.Framework;

namespace FileSumOfBytes.LogicTests.Model
{
    [TestFixture()]
    public class FileOptionsTests
    {
        [TestCase("33345F90A97D51507CBC27CCEDA8D8AC")]
        public void ComputeMd5ChecksumTest(string hashSum)
        {
            var file = new FileInfo("Resources\\ABC.bin");
            var fileOptions = new Logic.Model.FileOptions();
            var result = fileOptions.ComputeMd5Checksum(file);
            
            Assert.AreEqual(hashSum, result);
        }

        [Test]
        public void ComputeMd5ChecksumTestFileNotFoundException()
        {
            var file = new FileInfo("exception.txt");
            var fileOptions = new Logic.Model.FileOptions();

            Assert.Throws<FileNotFoundException>(() => fileOptions.ComputeMd5Checksum(file), "File not found.");
        }

        [Test]
        public void AnalyzeFilesTestDirectoryNotFoundException()    
        {
            var directory = new DirectoryInfo("exception");
            var fileOptions = new Logic.Model.FileOptions();

            Assert.Throws<DirectoryNotFoundException>(() => fileOptions.AnalyzeFiles(directory), "Directory not found.");
        }

        [Test]
        public void WriteToXmlTestDirectoryNotFoundException()
        {
            var directory = new DirectoryInfo("exception");
            var fileOptions = new Logic.Model.FileOptions();

            Assert.Throws<DirectoryNotFoundException>(() => fileOptions.WriteToXml(directory), "Directory not found.");
        }
    }
}   