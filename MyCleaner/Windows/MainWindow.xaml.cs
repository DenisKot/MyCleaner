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
    public sealed partial class MainWindow : Window
    {
        private SearchFilesBaseManager _searchBaseManager;
        private DeleteFileBaseManager _deleteBaseManager;

        private long _filesSize;
        private int _filesCount;
        private long _deletedFilesLenght;
        private int _deletedFiles;

        private readonly List<string> _filesToDelete;

        private readonly SynchronizationContext _context;

        private State _state;

        public MainWindow()
        {
            InitializeComponent();
            this._filesToDelete = new List<string>();

            this._state = State.None;

            MainBtn.Click += MainButtonClick;

            this._context = SynchronizationContext.Current;
        }

        private void MainButtonClick(object sender, EventArgs e)
        {
            switch (this._state)
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
            this._state = State.Searching;
            MainBtn.Content = "Stop";

            LogTextBox.Document.Blocks.Clear();
            LogTextBox.Document.Blocks.Add(new Paragraph(new Run("Scanning...")));

            this._filesToDelete.Clear();
            this._filesSize = 0;
            this._filesCount = 0;

            FilesSizeLabel.Content = HumanReadableByte.GetSize(0);
            FilesCountLabel.Content = "0 files";

            if (this._searchBaseManager == null)
            {
                this._searchBaseManager = new SearchFilesBaseManager();

                this._searchBaseManager.OnProgressChanged += WorkerProgressChanged;
                this._searchBaseManager.OnFinished += this.SearchBaseFilesComplete;
            }

            //// Start Seraching
            Thread thread = new Thread(this._searchBaseManager.Work);
            thread.Start(this._context);
        }

        private void CancelSearching()
        {
            this._searchBaseManager.Canel();
        }

        private void Clean()
        {
            this._state = State.Cleaning;
            MainBtn.Content = "Stop";

            this._deletedFiles = 0;
            this._deletedFilesLenght = 0;

            LogTextBox.Document.Blocks.Clear();
            LogTextBox.Document.Blocks.Add(new Paragraph(new Run("Deleting...")));

            if (this._deleteBaseManager == null)
            {
                this._deleteBaseManager = new DeleteFileBaseManager();

                this._deleteBaseManager.OnFileDeleted += FileDeleted;
                this._deleteBaseManager.OnFileNotDeleted += FileNotDeleted;
                this._deleteBaseManager.OnFinished += CleanFinished;
            }

            DeleteFileWrapper wrapper = new DeleteFileWrapper();
            wrapper.Context = this._context;
            wrapper.FilesList = this._filesToDelete;

            Thread thread = new Thread(this._deleteBaseManager.Work);
            thread.Start(wrapper);
        }

        private void StopCleaning()
        {
            this._deleteBaseManager.Canel();
        }
        
        /// <summary>
        /// Progress
        /// </summary>
        /// <param name="file"></param>
        private void WorkerProgressChanged(string file)
        {
            this._filesToDelete.Add(file);

            long fileSize = new FileInfo(file).Length;

            LogTextBox.Document.Blocks.Add(new Paragraph(new Run(file + " - " + HumanReadableByte.GetSize(fileSize))));

            this._filesSize += fileSize;
            this._filesCount++;

            FilesSizeLabel.Content = HumanReadableByte.GetSize(this._filesSize);
            FilesCountLabel.Content = Convert.ToString(this._filesCount) + " files";
        }

        /// <summary>
        /// Finished Search Files
        /// </summary>
        /// <param name="successfull"></param>
        private void SearchBaseFilesComplete(bool successfull)
        {
            if (successfull)
            {
                this._state = State.SearchedWaiting;
                MainBtn.Content = "Clean";
            }
            else
            {
                this._state = State.Aborted;
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

            this._deletedFiles++;
            this._deletedFilesLenght += size;

            FilesSizeLabel.Content = "Deleted: " + HumanReadableByte.GetSize(this._deletedFilesLenght); 
            FilesCountLabel.Content = "Deleted: " + this._deletedFiles + " files";
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
                this._state = State.Finished;
                MainBtn.Content = "Successful";
            }
            else
            {
                this._state = State.Aborted;
                MainBtn.Content = "Search Again";
            }

            FilesSizeLabel.Content = "Deleted: " + HumanReadableByte.GetSize(this._deletedFilesLenght); 
            FilesCountLabel.Content = "Deleted: " + this._deletedFiles + " files";
        }
    }
}
