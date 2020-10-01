using Acr.UserDialogs;
using IIASA.FloodCitiSense.Mobile.Core.Localization;

namespace IIASA.FloodCitiSense.Mobile.Core.UI
{
    public static class UserDialogHelper
    {
        public static void Warn(string localizationKeyOrMessage,
            LocalizationSource localizationSource = LocalizationSource.LocalTranslation)
        {
            UserDialogs.Instance.Alert(Localize(localizationKeyOrMessage, localizationSource), L.Localize("Warning"));
        }

        public static void Error(string localizationKeyOrMessage,
            LocalizationSource localizationSource = LocalizationSource.LocalTranslation)
        {
            UserDialogs.Instance.Alert(Localize(localizationKeyOrMessage, localizationSource), L.Localize("Error"));
        }

        public static void Success(string localizationKeyOrMessage,
            LocalizationSource localizationSource = LocalizationSource.LocalTranslation)
        {
            UserDialogs.Instance.Alert(Localize(localizationKeyOrMessage, localizationSource), L.Localize("Success"));
        }

        private static string Localize(string localizationKeyOrMessage,
            LocalizationSource localizationSource = LocalizationSource.LocalTranslation)
        {
            switch (localizationSource)
            {
                case LocalizationSource.RemoteTranslation:
                    return L.Localize(localizationKeyOrMessage);
                case LocalizationSource.LocalTranslation:
                    return LocalTranslationHelper.Localize(localizationKeyOrMessage);
                case LocalizationSource.NoTranslation:
                    return localizationKeyOrMessage;
                default:
                    return localizationKeyOrMessage;
            }
        }
    }
}