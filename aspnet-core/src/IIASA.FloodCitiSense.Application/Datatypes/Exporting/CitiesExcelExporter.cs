using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using IIASA.FloodCitiSense.DataExporting.Excel.EpPlus;
using IIASA.FloodCitiSense.DataTypes.Dtos;
using IIASA.FloodCitiSense.Dto;

namespace IIASA.FloodCitiSense.DataTypes.Exporting
{
    public class CitiesExcelExporter : EpPlusExcelExporterBase, ICitiesExcelExporter
    {
        private readonly IAbpSession _abpSession;

        private readonly ITimeZoneConverter _timeZoneConverter;

        public CitiesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession
        )
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCityForView> cities)
        {
            return CreateExcelPackage(
                "Cities.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Cities"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("User") + L("Name")
                    );

                    AddObjects(
                        sheet, 2, cities,
                        _ => _.City.Name,
                        _ => _.UserName
                    );
                });
        }
    }
}