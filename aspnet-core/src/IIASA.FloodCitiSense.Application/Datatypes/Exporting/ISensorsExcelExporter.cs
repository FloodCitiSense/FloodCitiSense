using System.Collections.Generic;
using IIASA.FloodCitiSense.Datatypes.Dtos;
using IIASA.FloodCitiSense.Dto;

namespace IIASA.FloodCitiSense.Datatypes.Exporting
{
    public interface ISensorsExcelExporter
    {
        FileDto ExportToFile(List<GetSensorForView> sensors);
    }
}