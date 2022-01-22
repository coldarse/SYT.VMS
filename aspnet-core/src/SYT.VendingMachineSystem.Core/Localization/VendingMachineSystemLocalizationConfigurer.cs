using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace SYT.VendingMachineSystem.Localization
{
    public static class VendingMachineSystemLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(VendingMachineSystemConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(VendingMachineSystemLocalizationConfigurer).GetAssembly(),
                        "SYT.VendingMachineSystem.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
