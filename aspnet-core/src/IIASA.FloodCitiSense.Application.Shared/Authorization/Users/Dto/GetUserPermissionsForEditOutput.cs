using IIASA.FloodCitiSense.Authorization.Permissions.Dto;
using System.Collections.Generic;

namespace IIASA.FloodCitiSense.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}