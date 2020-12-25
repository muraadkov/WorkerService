using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmailService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerFolderService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEmailSender _sender;
        private FileSystemWatcher watcher;
        private readonly string path = @"C:/Users/murka/Desktop/folder";  

        public Worker(ILogger<Worker> logger, IEmailSender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.Created += OnCreate;
            watcher.Changed += OnChange;
            watcher.Deleted += OnDelete;
            watcher.Renamed += OnRename;
            
            return base.StartAsync(cancellationToken);
        }

        private void OnCreate(object sender, FileSystemEventArgs e)
        {
            _logger.LogError("Some error");
            _logger.LogWarning("Some warning"); 
            _logger.LogInformation("File created at: {time}", DateTimeOffset.Now);
            var message = new Message(new string[] { "mr.prikoolnii@gmail.com" }, "SE1904 subject", $"created file with name {e.Name}", e.FullPath);
            _sender.SendEmail(message);
        }

        private void OnChange(object sender, FileSystemEventArgs e)
        {
            _logger.LogError("Some error");
            _logger.LogWarning("Some warning");
            _logger.LogInformation("File changed at: {time}", DateTimeOffset.Now);
            var message = new Message(new string[] { "mr.prikoolnii@gmail.com" }, "SE1904 subject", $"changed file, with name {e.Name}", e.FullPath);
            _sender.SendEmail(message);
        }

        private void OnDelete(object sender, FileSystemEventArgs e)
        {
            _logger.LogError("Some error");
            _logger.LogWarning("Some warning");
            _logger.LogInformation("File deleted at: {time}", DateTimeOffset.Now);
            var message = new Message(new string[] { "mr.prikoolnii@gmail.com" }, "SE1904 subject", $"deleted file, with name {e.Name}", e.FullPath);
            _sender.SendEmail(message);
        }
        private void OnRename(object sender, FileSystemEventArgs e)
        {
            _logger.LogError("Some error");
            _logger.LogWarning("Some warning");
            _logger.LogInformation("File renamed at: {time}", DateTimeOffset.Now);
            var message = new Message(new string[] { "mr.prikoolnii@gmail.com" }, "SE1904 subject", $"renamed file, with name {e.Name}", e.FullPath);
            _sender.SendEmail(message);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                watcher.EnableRaisingEvents = true;
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
