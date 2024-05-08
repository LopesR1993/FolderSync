
using Microsoft.Extensions.Logging;

namespace FolderSyncing.Services
{
    public class SyncService : ISyncService
    {
        private readonly ILogger<ISyncService> _logger;

        public SyncService(ILogger<ISyncService> logger)
        {
            _logger = logger;
        }
        public async Task<bool> SyncFilesAsync(string sourceFolder, string destinationFolder)
        {
            try
            {
                var directories = Directory.GetDirectories(sourceFolder);
                var files = Directory.GetFiles(sourceFolder);
                foreach (var directory in directories)
                {
                    var directoryName = Path.GetFileName(directory);
                    if (!Directory.Exists(destinationFolder + $"/{directoryName}"))
                    {
                        await CreateDirectory(directoryName!, destinationFolder);

                        await SyncFilesAsync($"{sourceFolder}/{directoryName}", $"{destinationFolder}/{directoryName}");
                    }
                }

                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    if (!File.Exists($"{destinationFolder}/{fileName}"))
                    {
                        await CreateFile(new FileInfo(file), $"{destinationFolder}");
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to sync folders. Exception: {e.Message}");
                return false;
            }
        }

        private Task CreateDirectory(string directoryName, string destinationFolder)
        {
            Directory.CreateDirectory(destinationFolder + $"/{directoryName}");

            _logger.LogInformation($"Created directory {directoryName} in {destinationFolder}/{directoryName}");

            return Task.CompletedTask;
        }

        private Task CreateFile(FileInfo file, string destinationFolder)
        {
            file.CopyTo($"{destinationFolder}/{file.Name}");

            _logger.LogInformation($"Created File {file.Name} in {destinationFolder}");

            return Task.CompletedTask;
        }
    }
}
