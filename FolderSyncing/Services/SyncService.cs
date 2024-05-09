
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
                var directories = Directory.EnumerateDirectories(sourceFolder);
                var files = Directory.EnumerateFiles(sourceFolder);

                foreach (var directory in directories)
                {
                    var directoryName = Path.GetFileName(directory);

                    var destinationDirectory = Path.Combine(destinationFolder, directoryName);

                    if (!Directory.Exists(destinationDirectory))
                    {
                        await CreateDirectory(directoryName, destinationFolder);
                    }

                    await SyncFilesAsync(Path.Combine(sourceFolder, directoryName), Path.Combine(destinationFolder, directoryName));
                }

                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);

                    var destinationFilePath = Path.Combine(destinationFolder, fileName);

                    if (!File.Exists(destinationFilePath))
                    {
                        await CreateFile(file, destinationFolder);
                    }
                    else
                    {
                        var currentFileLastWriteTime = File.GetLastWriteTimeUtc(file);
                        var destinationFileLastWriteTime = File.GetLastWriteTimeUtc(destinationFilePath);

                        if (currentFileLastWriteTime > destinationFileLastWriteTime)
                        {
                            await CreateFile(file, destinationFolder);
                        }
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
            var destinationDirectory = Path.Combine(destinationFolder, directoryName);
            Directory.CreateDirectory(destinationDirectory);

            _logger.LogInformation($"Created directory {directoryName} in {destinationDirectory}");

            return Task.CompletedTask;
        }

        private Task CreateFile(string sourceFilePath, string destinationFolder)
        {
            var fileName = Path.GetFileName(sourceFilePath);
            var destinationFilePath = Path.Combine(destinationFolder, fileName);

            File.Copy(sourceFilePath, destinationFilePath, true);

            _logger.LogInformation($"Created File {fileName} in {destinationFolder}");

            return Task.CompletedTask;
        }
    }
}
