using Cocona;
using FolderSyncing.Commands;
using FolderSyncing.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = CoconaApp.CreateBuilder();
var app = builder.Build();

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(app.Configuration)
    .WriteTo.Console()
    .WriteTo.File(app?.Configuration?.GetSection("LoggingPath").Value ?? "logs/log.txt")
    .CreateLogger();

builder.Services.AddSingleton<ISyncService, SyncService>();
builder.Services.AddSingleton<ISyncCommand, SyncCommand>();

app.AddCommands<ISyncCommand>();

app.Run();
