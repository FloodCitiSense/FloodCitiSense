using Abp.AutoMapper;
using IIASA.FloodCitiSense.MultiTenancy.Dto;
using System.ComponentModel;

namespace IIASA.FloodCitiSense.Mobile.Core.Models.Tenants
{
    [AutoMapFrom(typeof(TenantEditDto))]
    public class TenantEditModel : TenantEditDto, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}