using Serilog;
using System.Globalization;
using ILogger = Serilog.ILogger;

namespace Infrastructure.Utility
{
    public static class SerilogConfiguration
    {
        public static ILogger GetSeriLogger()
        {
            return new LoggerConfiguration()
                   .MinimumLevel.Information()
                   .WriteTo.Console()
                   .WriteTo.Logger(lc => lc
                   .WriteTo.File(GetLogFilePathYearMonth(),
                                 rollingInterval: RollingInterval.Day))
                   .CreateLogger();
        }
        
        private static string GetLogFilePathYearMonth()
        {
            var timestamp = DateTime.Now;
            var year = timestamp.Year.ToString();
            var month = timestamp.Month.ToString("00", CultureInfo.InvariantCulture);

            var logsDirectory = Path.Combine("Logs", year, month);
            Directory.CreateDirectory(logsDirectory);

            return Path.Combine(logsDirectory, "log.txt");
        }
    }
}
