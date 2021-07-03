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
            string errors_log_file = Path.Combine(folder, "errors.txt");

            var config = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .WriteTo.Console();

#if RELEASE
            config = config.WriteTo.Conditional(
                    x => x.Level == LogEventLevel.Error,
                    s => s.File(errors_log_file))
                .WriteTo.Conditional(
                    x => x.Level != LogEventLevel.Error,
                    s => s.File(logs_file));
#endif

            Log.Logger = config.CreateLogger();

            Log.Information("Serilog started!");
        }
    }
}