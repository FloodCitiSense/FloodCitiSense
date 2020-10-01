using Abp.AutoMapper;
using IIASA.FloodCitiSense.MultiTenancy.Dto;
using System.ComponentModel;

namespace IIASA.FloodCitiSense.Mobile.Core.Models.Tenants
{
    [AutoMapFrom(typeof(TenantListDto))]
    [AutoMapTo(typeof(TenantEditDto), typeof(CreateTenantInput))]
    public class TenantListModel : TenantListDto, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}