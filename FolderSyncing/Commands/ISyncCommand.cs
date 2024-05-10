using Cocona;

namespace FolderSyncing.Commands
{
    public interface ISyncCommand
    {
        Task SyncFoldersAsync(
            [Option('i', Description = "The interval between synchronization in seconds")] string intervalSeconds,
            [Option('s', Description = "The path to the folder to be synchronized")] string sourceFolder,
            [Option('d', Description = "The path to the replica folder")] string destinationFolder);
    }
}
