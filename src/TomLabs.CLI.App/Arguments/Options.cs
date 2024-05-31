using CommandLine;

namespace TomLabs.CLI.App.Arguments;

public class Options
{
    [Option('v', "verbositylevel", Required = false, HelpText = "Sets verbosity level for console output. 0 - Verbose, 1 - Debug, 2 - Information, 3 - Warning, 4 - Error, 5 - Fatal", Default = 1)]
    public Serilog.Events.LogEventLevel VerbosityLevel { get; set; }
}