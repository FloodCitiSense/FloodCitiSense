using System.Collections.Generic;
using IIASA.FloodCitiSense.DataTypes.Dtos;
using IIASA.FloodCitiSense.Dto;

namespace IIASA.FloodCitiSense.DataTypes.Exporting
{
    public interface ICitiesExcelExporter
    {
        FileDto ExportToFile(List<GetCityForView> cities);
    }
}