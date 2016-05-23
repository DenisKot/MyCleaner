using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using MyCleaner.Configuration;
using MyCleaner.Core.Business.Impl;
using MyCleaner.Core.Configuration;

namespace MyCleaner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SearchFilesManager searchManager = null;
        private DeleteFileManager deleteManager = null;

        private long filesSize;
        private int filesCount;
        private long deletedFilesLenght;
        private int deletedFiles;

        private List<string> filesToDelete;

        private SynchronizationContext context;

        private State state;

        public MainWindow()
        {
            InitializeComponent();
            filesToDelete = new List<string>();

            state = State.None;

            MainBtn.Click += MainButtonClick;

            context = SynchronizationContext.Current;
        }

        private void MainButtonClick(object sender, EventArgs e)
        {
            switch (state)
            {
                case State.None:
                case State.Aborted:
                    SearchFiles();
                    break;

                case State.Searching:
                    CancelSearching();
                    break;

                case State.SearchedWaiting:
                    Clean();
                    break;

                case State.Cleaning:
                    StopCleaning();
                    break;

                case State.Finished:
                    SearchFiles();
                    break;

            }
        }

        private void SearchFiles()
        {
            state = State.Searching;
            MainBtn.Content = "Stop";

            LogTextBox.Document.Blocks.Clear();
            LogTextBox.Document.Blocks.Add(new Paragraph(new Run("Scanning...")));

            filesToDelete.Clear();
            filesSize = 0;
            filesCount = 0;

            FilesSizeLabel.Content = HumanReadableByte.GetSize(0);
            FilesCountLabel.Content = "0 files";

            if (searchManager == null)
            {
                searchManager = new SearchFilesManager();

                searchManager.OnProgressChanged += WorkerProgressChanged;
                searchManager.OnFinished += SearchFilesComplete;
            }

            //// Start Seraching
            Thread thread = new Thread(searchManager.Work);
            thread.Start(context);
        }

        private void CancelSearching()
        {
            searchManager.Canel();
        }

        private void Clean()
        {
            state = State.Cleaning;
            MainBtn.Content = "Stop";

            deletedFiles = 0;
            deletedFilesLenght = 0;

            LogTextBox.Document.Blocks.Clear();
            LogTextBox.Document.Blocks.Add(new Paragraph(new Run("Deleting...")));

            if (deleteManager == null)
            {
                deleteManager = new DeleteFileManager();

                deleteManager.OnFileDeleted += FileDeleted;
                deleteManager.OnFileNotDeleted += FileNotDeleted;
                deleteManager.OnFinished += CleanFinished;
            }

            DeleteFileWrapper wrapper = new DeleteFileWrapper();
            wrapper.Context = context;
            wrapper.FilesList = filesToDelete;

            Thread thread = new Thread(deleteManager.Work);
            thread.Start(wrapper);
        }

        private void StopCleaning()
        {
            deleteManager.Canel();
        }


        /// <summary>
        /// Progress
        /// </summary>
        /// <param name="file"></param>
        private void WorkerProgressChanged(string file)
        {
            filesToDelete.Add(file);

            long fileSize = new FileInfo(file).Length;

            LogTextBox.Document.Blocks.Add(new Paragraph(new Run(file + " - " + HumanReadableByte.GetSize(fileSize))));

            filesSize += fileSize;
            filesCount++;

            FilesSizeLabel.Content = HumanReadableByte.GetSize(filesSize);
            FilesCountLabel.Content = Convert.ToString(filesCount) + " files";
        }

        /// <summary>
        /// Finished Search Files
        /// </summary>
        /// <param name="successfull"></param>
        private void SearchFilesComplete(bool successfull)
        {
            if (successfull)
            {
                state = State.SearchedWaiting;
                MainBtn.Content = "Clean";
            }
            else
            {
                state = State.Aborted;
                MainBtn.Content = "Search Again";
            }
        }

        /// <summary>
        /// File deleted
        /// </summary>
        /// <param name="file"></param>
        /// <param name="size"></param>
        private void FileDeleted(string file, long size)
        {
            LogTextBox.Document.Blocks.Add(new Paragraph(new Run(file)));

            deletedFiles++;
            deletedFilesLenght += size;

            FilesSizeLabel.Content = "Deleted: " + HumanReadableByte.GetSize(deletedFilesLenght); 
            FilesCountLabel.Content = "Deleted: " + deletedFiles + " files";
        }

        /// <summary>
        /// File not deleted
        /// </summary>
        /// <param name="file"></param>
        /// <param name="size"></param>
        private void FileNotDeleted(string file)
        {
            LogTextBox.Document.Blocks.Add(new Paragraph(new Run("Not deleted: " + file)));
        }

        private void CleanFinished(bool successful)
        {
            if (successful)
            {
                state = State.Finished;
                MainBtn.Content = "Successful";
            }
            else
            {
                state = State.Aborted;
                MainBtn.Content = "Search Again";
            }

            FilesSizeLabel.Content = "Deleted: " + HumanReadableByte.GetSize(deletedFilesLenght); 
            FilesCountLabel.Content = "Deleted: " + deletedFiles + " files";
        }
    }
}
