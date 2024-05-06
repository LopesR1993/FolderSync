using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSyncing.Exceptions
{
    public class InexistentSourceFolderException : Exception
    {
        public InexistentSourceFolderException(string sourceFolder) : base($"The provided source folder ({sourceFolder}) doesn't exist.")
        {
            
        }
    }
}
