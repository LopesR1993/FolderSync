using Cocona;
using FolderSyncing.Services;
using FolderSyncing.Validation;
using Microsoft.Extensions.Logging;

namespace FolderSyncing.Commands
{
    public class SyncCommand : ISyncCommand
    {
        private readonly ILogger<ISyncCommand> _logger;
        private readonly ISyncService _syncService;

        public SyncCommand(ILogger<SyncCommand> logger, ISyncService syncService)
        {
            _logger = logger;
            _syncService = syncService;
        }

        [Command("start")]
        public async Task SyncFoldersAsync(
            [Option('i', Description = "The interval between synchronization in seconds")] string intervalSeconds,
            [Option('s', Description = "The path to the folder to be synchronized")] string sourceFolder,
            [Option('d', Description = "The path to the replica folder")] string destinationFolder,
            [Option('l', Description = "The path to the log file.")] string logFileLocation)
            CancellationToken cancellationToken)
        {
            try
            {
                ValidationHelper.ValidateInputs(intervalSeconds, sourceFolder, destinationFolder, logFileLocation);
                _logger.LogInformation($"Started synchronizing folders and files.\n Source: {sourceFolder}\n Destination: {destinationFolder}");

                var interval = int.Parse(intervalSeconds) * 1000;
                while (!cancellationToken.IsCancellationRequested)
                {
                    var result = await _syncService.SyncFilesAsync(sourceFolder, destinationFolder);
                    await Task.Delay(interval, cancellationToken);
                }
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case OperationCanceledException:
                        _logger.LogInformation("Synchronization was cancelled.");
                        break;

                    default:
                        _logger.LogError(e.Message, "An error occured while synchronizing folders");
                        break;
                }
            }
        }
    }
}
