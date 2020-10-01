namespace IIASA.FloodCitiSense.Mobile.Core.UI.Assets
{
    public static class AssetsHelper
    {
        public const string AssetsNamespace = "IIASA.FloodCitiSense.Mobile.Core.UI.Assets";

        public static string ProfileImagePlaceholderNamespace => GetImageNamespace("Person.png");

        public static string GetImageNamespace(string fileName)
        {
            return string.Format("{0}.Images.{1}", AssetsNamespace, fileName);
        }

        public static string GetFileNamespace(string fileName)
        {
            return string.Format("{0}.Files.{1}", AssetsNamespace, fileName);
        }
    }
}