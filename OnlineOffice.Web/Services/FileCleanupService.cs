using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OfficeIMO.Collaborative.Web.Services;

public class FileCleanupService : BackgroundService
{
    private readonly DocumentSessionService _sessionService;
    private readonly ILogger<FileCleanupService> _logger;
    private readonly string _uploadPath;

    public FileCleanupService(DocumentSessionService sessionService, IWebHostEnvironment env, ILogger<FileCleanupService> logger)
    {
        _sessionService = sessionService;
        _logger = logger;
        _uploadPath = sessionService.UploadPath;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var cutoff = DateTime.UtcNow.AddHours(-24);
                if (Directory.Exists(_uploadPath))
                {
                    var files = Directory.GetFiles(_uploadPath);
                    foreach (var file in files)
                    {
                        try
                        {
                            var created = File.GetCreationTimeUtc(file);
                            if (created < cutoff)
                            {
                                File.Delete(file);
                                _logger.LogInformation("Cleaned up old upload: {File}", file);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to delete file {File}", file);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during file cleanup cycle");
            }

            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
    }
}
