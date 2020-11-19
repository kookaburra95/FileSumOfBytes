using System.IO;

namespace FileSumOfBytes.Logic.Model
{
    interface IFileOptions
    {
        public string ComputeMd5Checksum(FileInfo file);
        public void AnalyzeFiles(DirectoryInfo directory);
        public void WriteToXml(DirectoryInfo directory);
    }
}
