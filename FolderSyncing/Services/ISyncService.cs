namespace FolderSyncing.Services
{
    public interface ISyncService
    {
        Task<bool> SyncFilesAsync(string sourceFolder, string destinationFolder);
    }
}
