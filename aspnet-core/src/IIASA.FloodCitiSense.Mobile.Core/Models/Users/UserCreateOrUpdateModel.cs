using Abp.AutoMapper;
using IIASA.FloodCitiSense.Authorization.Users.Dto;
using System.ComponentModel;

namespace IIASA.FloodCitiSense.Mobile.Core.Models.Users
{
    [AutoMapFrom(typeof(CreateOrUpdateUserInput))]
    public class UserCreateOrUpdateModel : CreateOrUpdateUserInput, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}