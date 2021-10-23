using Android.Widget;
using LeadersOfDigital.Android.Bindings;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
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

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<ImageView>(
                nameof(ImageViewBinding),
                x => new ImageViewBinding(x, Mvx.IoCProvider.Resolve<ILogger<ImageViewBinding>>()));

            base.FillTargetFactories(registry);
        }
    }
}
