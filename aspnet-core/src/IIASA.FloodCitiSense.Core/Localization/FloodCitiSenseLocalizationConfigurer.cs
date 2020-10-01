using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace IIASA.FloodCitiSense.Localization
{
    public static class FloodCitiSenseLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    FloodCitiSenseConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(FloodCitiSenseLocalizationConfigurer).GetAssembly(),
                        "IIASA.FloodCitiSense.Localization.FloodCitiSense"
                    )
                )
            );
        }
    }
}