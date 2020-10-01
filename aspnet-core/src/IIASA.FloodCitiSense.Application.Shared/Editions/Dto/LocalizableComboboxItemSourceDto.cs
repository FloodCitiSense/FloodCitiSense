using System.Collections.ObjectModel;

namespace IIASA.FloodCitiSense.Editions.Dto
{
    //Mapped in CustomDtoMapper
    public class LocalizableComboboxItemSourceDto
    {
        public Collection<LocalizableComboboxItemDto> Items { get; set; }
    }
}