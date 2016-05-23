using System;
using System.IO;
using System.Threading;
using MyCleaner.Core.Configuration;

namespace MyCleaner.Core.Business.Impl
{
    public sealed class SearchFilesBaseManager : BaseManager
    {
        public override void Work(object param)
        {
            var context = (SynchronizationContext) param;

            //// Temp folders
            var tempFolders = TempFolders.GetTempFolders();

            foreach (var folder in tempFolders)
            {
                GetFiles(folder, "*.*", context);
            }

            //// File extensions
            var fileFilters = FileExtensions.GetFileExtenshions();

            foreach (var filter in fileFilters)
            {
                GetFiles("c:\\", filter, context);
            }

            context.Send(OnWorkCompleted, running);
        }

        private void GetFiles(string path, string pattern, SynchronizationContext context)
        {
            if (!running)
                return;

            try
            {
                var list = Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly);

                context.Send(OnFilesFound, list);

                foreach (var directory in Directory.GetDirectories(path))
                    GetFiles(directory, pattern, context);
            }
            catch
            {
                try
                {
                    if (File.Exists(path))
                    {
                        context.Send(OnFileFound, path);
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }

        private void OnFileFound(object param)
        {
            OnProgressChanged?.Invoke((string) param);
        }

        private void OnFilesFound(object param)
        {
            if (OnProgressChanged != null)
            {
                var arr = (string[]) param;

                foreach (var file in arr)
                {
                    OnProgressChanged(file);
                }
            }
        }

        private void OnWorkCompleted(object cancelled)
        {
            OnFinished?.Invoke((bool)cancelled);
        }

        public event Action<string> OnProgressChanged;

        public event Action<bool> OnFinished;
    }
}