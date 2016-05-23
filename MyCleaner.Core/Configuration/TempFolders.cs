using System;
using System.Collections.Generic;
using System.IO;

namespace MyCleaner.Core.Configuration
{
    public static class TempFolders
    {
        public static List<string> GetTempFolders()
        {
            var list = new List<string>
            {
                //// Ненужные файлы, созданые системой
                @"C:\Windows\temp",
                //// Ненужные файлы, созданые приложениями
                Path.GetTempPath(),
                @"C:\ProgramData\Microsoft\Windows\WER\ReportArchive",
                @"C:\ProgramData\Microsoft\Windows\WER\ReportQueue",
                @"C:\ProgramData\Microsoft\Windows\WER\Temp",
                //// Отчёты об ошибкам
                @"C:\ProgramData\Microsoft\Windows\Caches",
                //// Недавние 
                Environment.GetEnvironmentVariable("USERPROFILE") + @"\Recent",
                Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Roaming\Microsoft\Windows\Recent",
                //// Windows Defender
                @"C:\ProgramData\Microsoft\Windows Defender\Scans\History\Results\Resource",
                @"C:\ProgramData\Microsoft\Windows Defender\Scans\History\Results\Quick",
                @"C:\ProgramData\Microsoft\Windows Defender\Definition Updates\Backup",
                //// Explorer and IconCache
                Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Microsoft\Windows\Explorer",
                Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\IconCache.db",
                //// Кэш базисных установщиков Windows
                @"C:\Windows\Installer\$PatchCache$\",
                @"C:\Windows\WinSxS\Backup",
                //// Данные Windows Prefetch
                @"C:\Windows\Prefetch",
                //// Файлы в корзине
                @"C:\$Recycle.Bin\",
                //// Системный кэш
                @"C:\Windows\WinSxS\ManifestCache",
                @"C:\Windows\System32\config\systemprofile\AppData\LocalLow\Microsoft\CryptnetUrlCache\Content",
                //// Temporary location for install events
                @"C:\Windows\WinSxS\InstallTemp",
                //// Temp directory used for various operations, you’ll find pending renames here
                @"C:\Windows\WinSxS\Temp",
                //// Log files
                @"C:\Windows\System32\LogFiles",
                //// Adobe flash player
                Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Roaming\Macromedia\Flash Player"
        };

            //// Кэш Google Chrome
            //// Google Chrome rollback journal file
            if (Directory.Exists(Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Google\Chrome"))
            {
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\Local Storage");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\Cache\");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\Application Cache\Cache");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\GPUCache");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\Shortcuts-journal");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\Current Tabs");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\Last Tabs");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\Top Sites");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\SHistory Provider Cache");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\Network Action Predictor");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\History");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\Visited Links");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Safe Browsing Cookies-journal");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\Cookies-journal");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\Favicons-journal");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\History-journal");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\QuotaManager-journal");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\Shortcuts-journal");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\Top Sites-journal");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\Web Data-journal");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\Network Action Predictor-journal");
                list.Add(Environment.GetEnvironmentVariable("USERPROFILE") +
                         @"\AppData\Local\Google\Chrome\User Data\Default\Application Cache\Index-journal");
            }

            //// Кэш Mozilla FireFox
            if (Directory.Exists(Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Mozilla\Firefox\Profiles"))
            {
                foreach (var path in Directory.GetDirectories(Environment.GetEnvironmentVariable("USERPROFILE") +
                                                              @"\AppData\Local\Mozilla\Firefox\Profiles"))
                {
                    list.Add(path + @"\jumpListCache");
                    list.Add(path + @"\thumbnails");
                    list.Add(path + @"\cache2\entries");
                }
            }

            return list;
        }
    }
}