namespace MyCleaner.Configuration
{
    public enum State
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// Searching files
        /// </summary>
        Searching,

        /// <summary>
        /// Files are found and now waiting
        /// </summary>
        SearchedWaiting,

        /// <summary>
        /// Deleting files
        /// </summary>
        Cleaning,

        /// <summary>
        /// Cleaning finished
        /// </summary>
        Finished,

        /// <summary>
        /// When stoped searching or cleaning
        /// </summary>
        Aborted
    }
}
