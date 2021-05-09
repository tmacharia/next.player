using System.IO;
using Serilog;
using Serilog.Events;

namespace Next.PCL
{
    public class Logger
    {
        public static void ConfigureLogger()
        {
            string folder = Path.Combine(FileSys.AppFolder, "logs");
            string logs_file = Path.Combine(folder, $"{GlobalClock.Now:dd-MMM-yyy}.txt");
            string errors_file = Path.Combine(folder, "errors.txt");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Conditional(
                    x => x.Level == LogEventLevel.Error,
                    s => s.File(errors_file))
                .WriteTo.Conditional(
                    x => x.Level != LogEventLevel.Error,
                    s => s.File(logs_file))
                .CreateLogger();

            Log.Information("Serilog started!");
        }
    }
}