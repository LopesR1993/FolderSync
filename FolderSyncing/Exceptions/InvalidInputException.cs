namespace FolderSyncing.Exceptions
{
    public class InvalidTimeException : Exception
    {
        public InvalidTimeException() : base("Invalid time provided. Value must be a number and greater than zero.")
        {

        }
    }
}
