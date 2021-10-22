using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Android.Core;

namespace LeadersOfDigital.Android
{
    public class Setup : MvxAndroidSetup<App>
    {
        protected override ILoggerProvider CreateLogProvider()
        {
            return null;
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            return LoggerFactory.Create(b => b.SetMinimumLevel(LogLevel.Debug).AddConsole());
        }
    }
}
