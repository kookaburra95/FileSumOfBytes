using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml;

namespace FileSumOfBytes.Logic.Model
{
    /// <summary>
    /// Working with files.
    /// </summary>
    public class FileOptions
    {
        /// <summary>
        /// Collection of results(file name and hash).
        /// </summary>
        public ObservableCollection<FileHashSum> FilesInfo { get; } = new ObservableCollection<FileHashSum>();

        /// <summary>
        /// File analysis completion event.
        /// </summary>
        public event Action<string> IsEnded; 

        /// <summary>
        /// Thread synchronization (locker).
        /// </summary>
        private object _locker = new object();

        /// <summary>
        /// Compute file MD5 checksum.
        /// </summary>
        /// <param name="file">File to work with.</param>
        /// <returns>File Hash sum.</returns>
        public string ComputeMd5Checksum(FileInfo file)
        {
            if (!file.Exists)
            {
                throw new FileNotFoundException("File not found."); 
            }   

            using (Stream stream = File.OpenRead(file.FullName))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                var fileData = md5.ComputeHash(stream);

                var result = BitConverter.ToString(fileData).Replace("-", string.Empty);
                return result;
            }
        }

        /// <summary>
        /// Analyze all files in directory and sub directory.
        /// </summary>
        /// <param name="directory">User directory.</param>
        public void AnalyzeFiles(DirectoryInfo directory)
        {
            if (!directory.Exists)
            {
                throw new DirectoryNotFoundException("Directory not found.");
            }

            var files = directory.GetFiles();

            Parallel.ForEach(files, file =>
            {
                {
                    var hashSum = ComputeMd5Checksum(file);
                    var fileHashSum = new FileHashSum(file.Name, hashSum);

                    lock (_locker)
                    {
                        FilesInfo.Add(fileHashSum);
                    }
                }
            });

            var directories = directory.GetDirectories();
            if (directories.Length == 0) return;

            Parallel.ForEach(directories, AnalyzeFiles);
        }

        /// <summary>
        /// Async version AnalyzeFiles method.
        /// </summary>
        /// <param name="directory"></param>
        public async void AnalyzeFilesAsync(DirectoryInfo directory)
        {
            await Task.Run(() => AnalyzeFiles(directory));
            WriteToXml(directory);

            IsEnded?.Invoke("Анализ завершён.");
        }

        /// <summary>
        /// Writes the results to XML.
        /// </summary>
        /// <param name="directory">User directory.</param>
        public void WriteToXml(DirectoryInfo directory)
        {
            if (!directory.Exists)
            {
                throw new DirectoryNotFoundException("Directory not found.");
            }

            XmlDocument xml = new XmlDocument();
            XmlNode rootNode = xml.CreateElement("filesInfo");
            xml.AppendChild(rootNode);

            foreach (var file in FilesInfo)
            {
                XmlNode fileInfoNode = xml.CreateElement("fileInfo");
                XmlAttribute attribute = xml.CreateAttribute("name");
                attribute.Value = file.Name;
                fileInfoNode?.Attributes?.Append(attribute);
                fileInfoNode.InnerText = file.HashSum;
                rootNode.AppendChild(fileInfoNode);
            }

            using (var fs = new FileStream($"{directory.FullName}\\{directory.Name}.xml", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Delete))
            {
                xml.Save(fs);
                FilesInfo.Clear();
            }
        }
    }
}
