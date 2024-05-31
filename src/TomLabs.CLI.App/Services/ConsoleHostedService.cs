using CommandLine;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TomLabs.CLI.App.Arguments;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace TomLabs.CLI.App.Services;

internal sealed class ConsoleHostedService(
	ILogger<ConsoleHostedService> logger,
	IHostApplicationLifetime appLifetime)
	: IHostedService
{
	private readonly ILogger _logger = logger;

	private readonly string[] args = Environment.GetCommandLineArgs();

	public Task StartAsync(CancellationToken cancellationToken)
	{
		_logger.LogDebug("Starting with arguments: {Join}", string.Join(" ", args));

		appLifetime.ApplicationStarted.Register(() =>
		{
			Task.Run(async () =>
			{
				try
				{
					Parser.Default.ParseArguments<Options>(args)
						.WithParsed(o =>
						{
							
						})
						.WithNotParsed(errors =>
						{
							_logger.LogError("Errors: {@Errors}", errors);
						});
					
					await Task.Delay(-1, cancellationToken);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Unhandled exception!");
				}
				finally
				{
					// Stop the application once the work is done
					appLifetime.StopApplication();
				}
			}, cancellationToken);
		});

		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}