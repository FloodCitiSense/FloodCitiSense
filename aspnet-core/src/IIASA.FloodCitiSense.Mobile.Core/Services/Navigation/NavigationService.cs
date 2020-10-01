using Abp.Dependency;
using IIASA.FloodCitiSense.Mobile.Core.Core.DataStorage;
using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.Interface;
using IIASA.FloodCitiSense.Mobile.Core.Services.Pages;
using Microsoft.AppCenter.Crashes;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Navigation
{
    public class NavigationService : INavigationService, ISingletonDependency
    {
        private readonly IDataStorageManager _dataStoreManager;
        private readonly IPageService _pageService;

        public NavigationService(IPageService pageService, IDataStorageManager dataStoreManager)
        {
            _pageService = pageService;
            _dataStoreManager = dataStoreManager;
        }

        public async Task SetMainPage<TView>(object navigationParameter = null, bool clearNavigationHistory = false)
            where TView : IXamarinView
        {
            try
            {
                var page = await _pageService.CreatePage(typeof(TView), navigationParameter);

                if (_pageService.MainPage is NavigationPage navigationPage)
                {
                    if (clearNavigationHistory)
                        _pageService.MainPage =
                            new NavigationPage(page); //TODO: Can clear in a different way? And release views..?
                    else
                        await navigationPage.Navigation.PushAsync(page);
                }
                else
                {
                    _pageService.MainPage = new NavigationPage(page);
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public async Task NavigateToPage(string page)
        {
            try
            {
                var typeName = $"IIASA.FloodCitiSense.Views.{page}";
                var assemblyName = "IIASA.FloodCitiSense.Mobile.Shared";
                var assemblyQualifiedName = Assembly.CreateQualifiedName(assemblyName, typeName);
                var type = Type.GetType(assemblyQualifiedName);
                var createdPage = await _pageService.CreatePage(type, null);

                if (_pageService.MainPage is NavigationPage navigationPage)
                    await navigationPage.Navigation.PushAsync(createdPage);
                else
                    _pageService.MainPage = new NavigationPage(createdPage);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public async Task SetDetailPageAsync(Type viewType, object navigationParameter = null, bool pushToStack = false)
        {
            try
            {
                var currentPage = _pageService.MainPage;

                if (currentPage is NavigationPage) currentPage = currentPage.Navigation.NavigationStack.Last();

                if (!(currentPage is MasterDetailPage masterDetailPage))
                    throw new Exception($"Current MainPage is not a {typeof(MasterDetailPage)}!");

                var newPage = await _pageService.CreatePage(viewType, navigationParameter);

                if (pushToStack && masterDetailPage.Detail is NavigationPage navPage)
                    await navPage.PushAsync(newPage);
                else
                    masterDetailPage.Detail = new NavigationPage(newPage);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public async Task<Page> GoBackAsync()
        {
            try
            {
                if (_pageService.MainPage is NavigationPage navigationPage)
                {
                    var currentPage = navigationPage.Navigation.NavigationStack.Last();
                    if (currentPage is MasterDetailPage masterDetail &&
                        masterDetail.Detail is NavigationPage detailNavigationPage)
                        if (detailNavigationPage.Navigation.NavigationStack.Count > 1)
                            return await detailNavigationPage.Navigation.PopAsync();

                    return await navigationPage.Navigation.PopAsync();
                }
                else if (_pageService.MainPage is MasterDetailPage masterDetail &&
                         masterDetail.Detail is NavigationPage detailNavigationPage)
                {
                    return await detailNavigationPage.Navigation.PopAsync();
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }

            return null;
        }
    }
}