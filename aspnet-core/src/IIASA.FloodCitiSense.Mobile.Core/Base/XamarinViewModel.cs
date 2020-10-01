using Abp.Dependency;
using Abp.ObjectMapping;
using IIASA.FloodCitiSense.ApiClient;
using IIASA.FloodCitiSense.Mobile.Core.Core.DataStorage;
using IIASA.FloodCitiSense.Mobile.Core.Core.Dependency;
using IIASA.FloodCitiSense.Mobile.Core.Localization;
using IIASA.FloodCitiSense.Mobile.Core.Services.Modal;
using IIASA.FloodCitiSense.Mobile.Core.Services.Navigation;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace IIASA.FloodCitiSense.Mobile.Core.Base
{
    public abstract class XamarinViewModel : ExtendedBindableObject, ITransientDependency
    {
        private bool _isBusy;
        protected readonly INavigationService NavigationService;
        protected readonly IModalService ModalService;
        public IObjectMapper ObjectMapper { get; set; }

        public ICommand NavigateAsyncCommand => new Command<string>(async (str) => await NavigateAsync(str));

        private async Task NavigateAsync(string str)
        {
            await NavigationService.NavigateToPage(str);
        }

        public bool IsNotBusy => !IsBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
                RaisePropertyChanged(() => IsNotBusy);
            }
        }

        public bool IsConnectedToInternet { get; } = Connectivity.NetworkAccess == NetworkAccess.Internet;

        protected XamarinViewModel()
        {
            ModalService = Resolver.Resolve<IModalService>(); ;
            NavigationService = Resolver.Resolve<INavigationService>();
            ObjectMapper = NullObjectMapper.Instance;
        }



        public virtual async Task InitializeAsync(object navigationData)
        {
            var dataStorage = Resolver.Resolve<IDataStorageManager>();
            var appContext = Resolver.Resolve<IApplicationContext>();

            //if (appContext != null && dataStorage != null)
            //{
            //    if (appContext.AppInformation != null )
            //    {
            //        var time = dataStorage.Retrieve<DateTime>(DataStorageKey.LastTokenRequestedTime);
            //        if (appContext.AppInformation.LastTokenRequestedTime != DateTime.MinValue && appContext.AppInformation.LastTokenRequestedTime != time)
            //        {
            //            await dataStorage.StoreAsync(DataStorageKey.AccessToken, appContext.AppInformation.AccessToken);
            //            await dataStorage.StoreAsync(DataStorageKey.RefreshToken, appContext.AppInformation.RefreshToken, true);
            //            await dataStorage.StoreAsync(DataStorageKey.LastTokenRequestedTime, appContext.AppInformation.LastTokenRequestedTime);
            //        }
            //    }
            //}

            await Task.FromResult(false);
        }

        public object GetPropertyValue(string propertyName)
        {
            return GetType().GetProperty(propertyName)?.GetValue(this, null);
        }

        public T GetPropertyValue<T>(string propertyName)
        {
            var value = GetPropertyValue(propertyName);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public async Task SetBusyAsync(Func<Task> func, string loadingMessage = null)
        {
            if (loadingMessage == null)
            {
                loadingMessage = L.Localize("LoadWithThreeDot");
            }
            IsBusy = true;
            try
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(message: loadingMessage))
                {
                    await func();
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected async Task WaitAndExecute(int milisec, Action actionToExecute)
        {
            await Task.Delay(milisec);
            using (await MaterialDialog.Instance.LoadingSnackbarAsync(message: L.Localize("LoadWithThreeDot")))
            {
                actionToExecute();
            }
        }
    }
}