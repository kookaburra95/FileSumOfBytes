using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using DevExpress.Mvvm;
using FileSumOfBytes.Logic.Model;
using Application = System.Windows.Application;
using FileOptions = FileSumOfBytes.Logic.Model.FileOptions;

namespace FileSumOfBytes.WPF.ViewModels 
{
    class MainViewModel : ViewModelBase
    {
        private readonly FolderBrowserDialog _folderBrowserDialog;
        
        /// <summary>
        /// File options model.
        /// </summary>
        private FileOptions _fileOptions;

        private DirectoryInfo _directoryInfo;

        /// <summary>
        /// Collection of results(file name and hash).
        /// </summary>
        public ObservableCollection<FileHashSum> FilesInfo { get; }

        public string DirectoryPath { get; set; } = string.Empty;
        public bool IsAnalysisEnded { get; set; } = true;

        /// <summary>
        /// Command for button btFolder.
        /// </summary>
        public DelegateCommand ChooseFolderCommand { get; private set; }
        
        /// <summary>
        /// Command for button btAnalysis.
        /// </summary>
        public DelegateCommand StartAnalysisFilesCommand { get; private set; }  

        public MainViewModel()
        {
            FilesInfo = new ObservableCollection<FileHashSum>();
            _folderBrowserDialog = new FolderBrowserDialog();
            ChooseFolderCommand = new DelegateCommand(ChooseFolder);
            StartAnalysisFilesCommand = new DelegateCommand(StartAnalysisFiles);

            _fileOptions = new FileOptions();
            _fileOptions.FilesInfo.CollectionChanged += ShowFileInfo;
            _fileOptions.IsEnded += IsEndedMessage;
        }

        /// <summary>
        /// Choose folder and set Directory path.
        /// </summary>
        private void ChooseFolder()
        {
            if (_folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                DirectoryPath = _folderBrowserDialog.SelectedPath;
                _directoryInfo = new DirectoryInfo(DirectoryPath);
            }
        }

        /// <summary>
        /// Analysis files.
        /// </summary>
        private void StartAnalysisFiles()
        {
            FilesInfo.Clear();

            if (_directoryInfo != null && _directoryInfo.Exists)
            {
                MessageBox.Show("Запущен анализ файлов.\n" +
                                "Дождитесь сообщения о завершении работы.\n" +
                                "В противном случае данные не будут сохранены.", "Анализ файлов.");

                _fileOptions.AnalyzeFilesAsync(_directoryInfo);

                IsAnalysisEnded = false;
            }
            else
            {
                MessageBox.Show("Выберите директорию.","Выбор");
            }
        }

        /// <summary>
        /// Handles the event of adding to the collection and prints it to the DataGrid.
        /// </summary>
        private void ShowFileInfo(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add) return;
            if (e.NewItems[0] is FileHashSum newFile)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    FilesInfo.Add(newFile);
                });
            }
        }

        /// <summary>
        /// Handles the event of ended analyze files and prints message to the MessageBox.
        /// </summary>
        private void IsEndedMessage(string message)
        {
            MessageBox.Show($"{message}\n" +
                            $"Данные сохранены в выбранной директории.");
            IsAnalysisEnded = true;
        }
    }
}   
