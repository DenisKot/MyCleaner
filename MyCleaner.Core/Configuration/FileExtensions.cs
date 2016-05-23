using System.Collections.Generic;

namespace MyCleaner.Core.Configuration
{
    public static class FileExtensions
    {
        public static List<string> GetFileExtenshions()
        {
            List<string> list = new List<string>
            {
                "*.tmp",
                "*.log",
                "*.bak",
                "*.chk",
                "*.~*",
                "*.jnk",
                "*.cache",
                "*.cvr",
                "*.etl",
                "*.regtrans-ms",
                "*.blf"
            };

            return list;
        }
    }
}
