using LeadersOfDigital.ViewModels.Authorization;
using MvvmCross.ViewModels;

namespace LeadersOfDigital
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            RegisterAppStart<AuthorizationViewModel>();
        }
    }
}
