using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SmtpServer
{
  /// <summary>
  /// SmtpServer background service
  /// </summary>
  public class SmtpServerBackgroundService : BackgroundService
  {
    private readonly ISmtpServerOptions _options;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SmtpServerBackgroundService> _logger;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="options">The SMTP server options.</param>
    /// <param name="serviceProvider">The service provider to use when resolving services.</param>
    /// <param name="logger">The logger to use.</param>
    public SmtpServerBackgroundService(
      ISmtpServerOptions options,
      IServiceProvider serviceProvider,
      ILogger<SmtpServerBackgroundService> logger)
    {
      _options = options;
      _serviceProvider = serviceProvider;
      _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      _logger.LogDebug($"{nameof(SmtpServerBackgroundService)} is starting.");
      try
      {
        var smtpServer = new SmtpServer(_options, _serviceProvider);

        _logger.LogDebug($"{nameof(SmtpServerBackgroundService)} is started.");

        stoppingToken.Register(() =>
          _logger.LogDebug($"{nameof(SmtpServerBackgroundService)} is stopping."));

        await smtpServer.StartAsync(stoppingToken);

        _logger.LogDebug($"{nameof(SmtpServerBackgroundService)} is stopped.");
      }
      catch (Exception exception)
      {
        _logger.LogError(exception, $"Exception occurred in {nameof(SmtpServerBackgroundService)}.");
        throw;
      }
    }
  }
  
}
