using IIASA.FloodCitiSense.Mobile.Core.Models.NavigationMenu;
using MvvmHelpers;
using System.Collections.Generic;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}