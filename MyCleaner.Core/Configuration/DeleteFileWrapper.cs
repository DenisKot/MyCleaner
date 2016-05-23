using System.Collections.Generic;
using System.Threading;

namespace MyCleaner.Core.Configuration
{
    public struct DeleteFileWrapper
    {
        public List<string> FilesList;

        public SynchronizationContext Context;
    }
}
