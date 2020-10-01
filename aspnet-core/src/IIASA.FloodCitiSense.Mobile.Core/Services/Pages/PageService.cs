using Abp.Dependency;
using IIASA.FloodCitiSense.Mobile.Core.Base;
using IIASA.FloodCitiSense.Mobile.Core.Core.Dependency;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Pages
{
    public class PageService : IPageService, ISingletonDependency
    {
        public Page MainPage
        {
            get => Application.Current.MainPage;
            set => Application.Current.MainPage = value;
        }

        public async Task<Page> CreatePage(Type viewType, object navigationParameter)
        {
            var view = (Page)Resolver.Resolve(viewType);
            if (!(view.BindingContext is XamarinViewModel viewModel))
            {
                throw new System.Exception($"BindingContext of views must inherit {nameof(XamarinViewModel)}. Given view's BindingContext is not like that: {viewType}");
            }

            await viewModel.InitializeAsync(navigationParameter);
            return view;
        }
    }
}