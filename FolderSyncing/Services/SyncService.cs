namespace FolderSyncing.Services
{
    public class SyncService : ISyncService
    {
        private readonly Serilog.ILogger _logger;

        public SyncService(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public async Task<bool> SyncFilesAsync(string sourceFolder, string destinationFolder)
        {
            try
            {
                var directories = Directory.EnumerateDirectories(sourceFolder);
                var files = Directory.EnumerateFiles(sourceFolder);

                // Sync files and directories from source to destination
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
                        await CreateFile(file, destinationFolder, false);
                    }
                    else
                    {
                        var currentFileLastWriteTime = File.GetLastWriteTimeUtc(file);
                        var destinationFileLastWriteTime = File.GetLastWriteTimeUtc(destinationFilePath);

                        if (currentFileLastWriteTime > destinationFileLastWriteTime)
                        {
                            await CreateFile(file, destinationFolder, true);
                        }
                    }
                }

                // Delete files and directories from destination if they were deleted from source
                var destinationDirectories = Directory.EnumerateDirectories(destinationFolder);
                var destinationFiles = Directory.EnumerateFiles(destinationFolder);

                foreach (var destinationDirectory in destinationDirectories)
                {
                    var directoryName = Path.GetFileName(destinationDirectory);
                    var sourceDirectory = Path.Combine(sourceFolder, directoryName);

                    if (!Directory.Exists(sourceDirectory))
                    {
                        await DeleteDirectory(new DirectoryInfo(destinationDirectory));
                    }
                }

                foreach (var destinationFile in destinationFiles)
                {
                    var fileName = Path.GetFileName(destinationFile);
                    var sourceFilePath = Path.Combine(sourceFolder, fileName);

                    if (!File.Exists(sourceFilePath))
                    {
                        await DeleteFile(new FileInfo(destinationFile));
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to sync folders. Exception: {e.Message}");
                return false;
            }
        }

        private async Task CreateDirectory(string directoryName, string destinationFolder)
        {
            var destinationDirectory = Path.Combine(destinationFolder, directoryName);
            Directory.CreateDirectory(destinationDirectory);

            _logger.Information($"Created directory {directoryName} in {destinationDirectory}");
        }

        private async Task CreateFile(string sourceFilePath, string destinationFolder, bool isUpdate)
        {
            var fileName = Path.GetFileName(sourceFilePath);
            var destinationFilePath = Path.Combine(destinationFolder, fileName);

            using (FileStream sourceStream = File.Open(sourceFilePath, FileMode.Open))
            {
                using (FileStream destinationStream = File.Create(destinationFilePath))
                {
                    await sourceStream.CopyToAsync(destinationStream);
                }
            }

            if (isUpdate)
            {
                _logger.Information($"Updated File {fileName} in {destinationFolder}");
            }
            else
            {
                _logger.Information($"Created File {fileName} in {destinationFolder}");
            }
        }
        private async Task DeleteDirectory(DirectoryInfo directory)
        {
            Directory.Delete(directory.FullName, true);
            _logger.Information($"Deleted directory {directory.Name} from {directory?.Parent?.FullName ?? "disk"}");
        }

        private async Task DeleteFile(FileInfo file)
        {
            File.Delete(file.FullName);
            _logger.Information($"Deleted file {file.Name} from {file.DirectoryName}");
        }
    }
}
