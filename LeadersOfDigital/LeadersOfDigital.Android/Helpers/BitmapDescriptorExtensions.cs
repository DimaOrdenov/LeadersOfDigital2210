using Android.Content;
using Android.Gms.Maps.Model;
using Android.Graphics;
using AndroidX.Core.Content;

namespace LeadersOfDigital.Android.Helpers
{
    public static class BitmapDescriptorExtensions
    {
        public static BitmapDescriptor GetBitmapDescriptorFromVector(Context context, int resourceId)
        {
            var vectorDrawable = ContextCompat.GetDrawable(context, resourceId);
            vectorDrawable.SetBounds(0, 0, vectorDrawable.IntrinsicWidth, vectorDrawable.IntrinsicHeight);
            
            var bitmap = Bitmap.CreateBitmap(vectorDrawable.IntrinsicWidth, vectorDrawable.IntrinsicHeight, Bitmap.Config.Argb8888);
            
            var canvas = new Canvas(bitmap);
            vectorDrawable.Draw(canvas);

            return BitmapDescriptorFactory.FromBitmap(bitmap);
        }
    }
}
