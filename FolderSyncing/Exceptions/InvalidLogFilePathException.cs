namespace FolderSyncing.Exceptions
{
    public class InvalidLogFilePathException : Exception
    {
        public InvalidLogFilePathException(string logFileLocation) : base($"Unable to write to the path: {logFileLocation}")
        {

        }
    }
}

