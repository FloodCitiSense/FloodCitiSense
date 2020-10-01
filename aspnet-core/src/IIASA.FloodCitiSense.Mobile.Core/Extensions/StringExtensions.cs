using System;
using System.IO;
using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Mobile.Core.Extensions
{
    using System.Reflection;

    using IIASA.FloodCitiSense.Mobile.Core.UI.Assets;

    using JetBrains.Annotations;

    public static class StringExtensions
    {
        public static bool CheckUrlValid(this string source)
        {
            return Uri.TryCreate(source, UriKind.Absolute, out Uri uriResult) && uriResult.Scheme == Uri.UriSchemeHttps;
        }


        public static bool IsVideo(this string path)
        {
            if (File.Exists(path) || path.HasExtension())
            {
                var ext = Path.GetExtension(path);
                return ext == ".mp4";
            }
            return false;
        }

        public static bool IsPicture(this string path)
        {
            if (File.Exists(path) || path.HasExtension())
            {
                var ext = Path.GetExtension(path);
                return ext == ".jpg";
            }
            return false;
        }

        public static bool IsUrlValid(this string source)
        {
            return Uri.TryCreate(source, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public static bool HasExtension(this string source)
        {
            if (!source.IsUrlValid()) return false;
            var ext = Path.GetExtension(source);
            return !string.IsNullOrEmpty(ext);

        }

        public static async Task<string> ReadTextAsync([NotNull] this string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(AssetsHelper)).Assembly;

            var stream = assembly.GetManifestResourceStream(path);
            using (var file = new StreamReader(stream ?? throw new InvalidOperationException()))
            {
                return await file.ReadToEndAsync();
            }
        }
    }
}