using System.Collections.Generic;
using IIASA.FloodCitiSense.Auditing.Dto;
using IIASA.FloodCitiSense.Dto;

namespace IIASA.FloodCitiSense.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);
    }
}
