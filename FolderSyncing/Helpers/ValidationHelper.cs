using FolderSyncing.Exceptions;
using System.Diagnostics;
using System.Globalization;

namespace FolderSyncing.Validation
{
    public static class ValidationHelper
    {
        public static void ValidateInputs(string intervalSeconds, string sourceFolder, string destinationFolder, string logFileLocation)
        {
            ValidateTimeInput(intervalSeconds);
            ValidateSourceFolderInput(sourceFolder);
            ValidateDestinationFolderInput(destinationFolder);
            ValidateLogFileLocationInput(logFileLocation);
        }

        private static void ValidateTimeInput(string timeInput)
        {
            int output = -1;
            if (!int.TryParse(timeInput, out output) || output < 1)
            {
                throw new InvalidTimeException();
            }
        }
        private static void ValidateLogFileLocationInput(string logFileLocation)
        {
            
        }

        private static void ValidateDestinationFolderInput(string destinationFolder)
        {
            
        }


        private static void ValidateSourceFolderInput(string sourceFolderInput)
        {
            if (!Directory.Exists(sourceFolderInput)) 
            {
                throw new InexistentSourceFolderException(sourceFolderInput);
            }
        }
    }
}
