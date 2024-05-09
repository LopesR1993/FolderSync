using Cocona;
using FolderSyncing.Commands;
using FolderSyncing.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddSingleton<ISyncService, SyncService>();
builder.Services.AddSingleton<ILogger>(new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger());

var app = builder.Build();

app?.AddCommands<SyncCommand>();

app?.Run();
