using FolderSyncing.Exceptions;
using System.Diagnostics;
using System.Globalization;

namespace FolderSyncing.Validation
{
    public static class ValidationHelper
    {
        public static void ValidateInputs(string intervalSeconds, string sourceFolder, string destinationFolder)
        {
            ValidateTimeInput(intervalSeconds);
            ValidateSourceFolderInput(sourceFolder);
            ValidateDestinationFolderInput(destinationFolder);
        }

        private static void ValidateTimeInput(string timeInput)
        {
            if (!int.TryParse(timeInput, out int output) || output < 1)
            {
                throw new InvalidTimeException();
            }
        }

        private static void ValidateDestinationFolderInput(string destinationFolder)
        {
            if (!Directory.Exists(destinationFolder))
            {
                throw new DirectoryNotFoundException();
            }
        }


        private static void ValidateSourceFolderInput(string sourceFolderInput)
        {
            if (!Directory.Exists(sourceFolderInput)) 
            {
                throw new DirectoryNotFoundException();
            }
        }
    }
}
