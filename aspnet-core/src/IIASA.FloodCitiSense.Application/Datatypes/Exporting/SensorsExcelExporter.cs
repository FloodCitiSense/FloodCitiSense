using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using IIASA.FloodCitiSense.DataExporting.Excel.EpPlus;
using IIASA.FloodCitiSense.Datatypes.Dtos;
using IIASA.FloodCitiSense.Dto;

namespace IIASA.FloodCitiSense.Datatypes.Exporting
{
    public class SensorsExcelExporter : EpPlusExcelExporterBase, ISensorsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SensorsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSensorForView> sensors)
        {
            return CreateExcelPackage(
                "Sensors.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Sensors"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, 2, sensors,
                        _ => _.Sensor.Name
                        );

					

                });
        }
    }
}
