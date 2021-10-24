using Android.Widget;
using LeadersOfDigital.Services;
using Xamarin.Essentials;

namespace LeadersOfDigital.Android.Services
{
    public class DialogService : IDialogService
    {
        public void ShowToast(string text)
            => Toast.MakeText(Platform.CurrentActivity, text, ToastLength.Short).Show();
    }
}
