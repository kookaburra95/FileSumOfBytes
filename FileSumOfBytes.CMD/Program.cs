using System;
using System.Collections.Specialized;
using System.IO;
using FileSumOfBytes.Logic.Model;
using FileOptions = FileSumOfBytes.Logic.Model.FileOptions;

namespace FileSumOfBytes.CMD
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Для завершения нажмите любую клавишу.\n" +
                              "Если программа не закончила работу, данные не будут сохранены.\n" +
                              "Дождитесь информации о завершении работы программы.\n\n");
            
            while (true)
            {
                Console.WriteLine("Введите директорию:");
                var directoryPath = Console.ReadLine();

                if (string.IsNullOrEmpty(directoryPath))
                {
                    Console.WriteLine("Неккоректный ввод. Повторите ввод.");
                    continue;
                }

                var directory = new DirectoryInfo(directoryPath);

                if (!directory.Exists)
                {
                    Console.WriteLine("Директория не найдена. Повторите ввод.");
                    continue;
                }

                Console.WriteLine("\nИнформация о файлах:\n");

                var fileOptions = new FileOptions();
                fileOptions.FilesInfo.CollectionChanged += ShowFileInfo;
                fileOptions.IsEnded += IsEndedMessage;


                fileOptions.AnalyzeFilesAsync(directory);
                break;
            }
            
            Console.ReadLine();
        }

        /// <summary>
        /// Handles the event of adding to the collection and prints it to the console.
        /// </summary>
        private static void ShowFileInfo(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add) return;
            if (e.NewItems[0] is FileHashSum newFile) Console.WriteLine($"{newFile.Name} - {newFile.HashSum}");
        }

        /// <summary>
        /// Handles the event of ended analyze files and prints message to the console.
        /// </summary>
        private static void IsEndedMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
