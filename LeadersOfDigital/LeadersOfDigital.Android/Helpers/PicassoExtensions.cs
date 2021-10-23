using System.Threading.Tasks;
using Android.Content;
using Java.IO;
using Square.Picasso;

namespace LeadersOfDigital.Android.Helpers
{
    public static class PicassoExtensions
    {
        public static async Task<RequestCreator> LoadAsync(this Picasso picasso, Context context, byte[] bytes)
        {
            using var file = File.CreateTempFile("_temp", null, context.CacheDir);
            using var stream = new FileOutputStream(file);
            await stream.WriteAsync(bytes);

            return picasso
                .Load(file);
        }

        public static RequestCreator Load(this Picasso picasso, Context context, byte[] bytes)
        {
            using var file = File.CreateTempFile("_temp", null, context.CacheDir);
            using var stream = new FileOutputStream(file);
            stream.Write(bytes);

            return picasso
                .Load(file);
        }
    }
}
