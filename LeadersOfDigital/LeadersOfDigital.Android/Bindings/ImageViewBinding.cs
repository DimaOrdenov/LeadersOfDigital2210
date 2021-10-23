using System;
using Android.Widget;
using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Android.Binding.Target;

namespace LeadersOfDigital.Android.Bindings
{
    public class ImageViewBinding : MvxAndroidTargetBinding<ImageView, string>
    {
        private readonly ILogger<ImageViewBinding> _logger;

        public ImageViewBinding(ImageView target, ILogger<ImageViewBinding> logger)
            : base(target)
        {
            _logger = logger;
        }

        protected override void SetValueImpl(ImageView target, string value)
        {
            try
            {
                if (value != null)
                {
                    target.SetImageResource(target.Context.Resources.GetIdentifier(value, "drawable", target.Context.PackageName));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't set image");
            }
        }
    }
}
