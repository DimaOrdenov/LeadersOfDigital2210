using Android.Content;
using Android.Views;
using Android.Views.InputMethods;

namespace LeadersOfDigital.Android.Helpers
{
    public static class ViewExtensions
    {
        public static void HideKeyboard(this View view, Context context)
        {
            var inputManager = (InputMethodManager)context.GetSystemService(Context.InputMethodService);
            inputManager.HideSoftInputFromWindow(view.WindowToken, 0);
        }
    }
}
