using System.Collections.Generic;
using IIASA.FloodCitiSense.Authorization.Users.Dto;
using IIASA.FloodCitiSense.Dto;

namespace IIASA.FloodCitiSense.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}