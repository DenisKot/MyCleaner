using System;

namespace MyCleaner.Configuration
{
    class HumanReadableByte
    {
        private static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static string GetSize(Int64 value)
        {
            if (value < 0) { return "-" + GetSize(-value); }

            int i = 0;
            decimal dValue = value;
            while (Math.Round(dValue / 1024) >= 1)
            {
                dValue /= 1024;
                i++;
            }

            return $"{dValue:n1} {SizeSuffixes[i]}";
        }
    }
}
