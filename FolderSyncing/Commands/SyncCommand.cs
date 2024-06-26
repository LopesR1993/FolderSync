﻿using Cocona;
using FolderSyncing.Services;
using FolderSyncing.Validation;

namespace FolderSyncing.Commands
{
    public class SyncCommand : ISyncCommand
    {
        private readonly Serilog.ILogger _logger;
        private readonly ISyncService _syncService;

        public SyncCommand(Serilog.ILogger logger, ISyncService syncService)
        {
            _logger = logger;
            _syncService = syncService;
        }

        [Command("start")]
        public async Task SyncFoldersAsync(
            [Option('i', Description = "The interval between synchronization in seconds")] string intervalSeconds,
            [Option('s', Description = "The path to the folder to be synchronized")] string sourceFolder,
            [Option('d', Description = "The path to the replica folder")] string destinationFolder)
        {
            try
            {
                ValidationHelper.ValidateInputs(intervalSeconds, sourceFolder, destinationFolder);
                _logger.Information($"Started synchronizing folders and files.\n Source: {sourceFolder}\n Destination: {destinationFolder}");

                var interval = int.Parse(intervalSeconds) * 1000;
                while (true)
                {
                    var result = await _syncService.SyncFilesAsync(sourceFolder, destinationFolder);
                    await Task.Delay(interval);
                }
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case OperationCanceledException:
                        _logger.Information("Synchronization was cancelled.");
                        break;

                    default:
                        _logger.Error(e.Message, "An error occured while synchronizing folders");
                        break;
                }
            }
        }
    }
}
