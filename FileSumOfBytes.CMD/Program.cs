using System;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.CompilerServices;
using FileSumOfBytes.Logic.Model;
using FileOptions = FileSumOfBytes.Logic.Model.FileOptions;

namespace FileSumOfBytes.CMD
{
    class Program
    {
        static void Main(string[] args)
        {
            //test
            var tempTestDirectoryPath = "testFolder\\";

            //TODO:write logic for work

            var directory = new DirectoryInfo(tempTestDirectoryPath);

            var fileOptions = new FileOptions();
            fileOptions.FilesInfo.CollectionChanged += ShowFileInfo;

            fileOptions.AnalyzeFiles(directory);
            fileOptions.WriteToXml(directory);
        }

        /// <summary>
        /// Handles the event of adding to the collection and prints it to the console.
        /// </summary>
        private static void ShowFileInfo(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add) return;
            if (e.NewItems[0] is FileHashSum newFile) Console.WriteLine($"{newFile.Name} - {newFile.HashSum}");
        }
    }
}
