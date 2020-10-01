using IIASA.FloodCitiSense.Mobile.Core.Core;
using IIASA.FloodCitiSense.Mobile.Core.Core.Dependency;
using System;
using System.Globalization;
using System.Reflection;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.Mobile.Core.Base
{
    public static class ViewManager
    {
        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached(
                "AutoWireViewModel",
                typeof(bool),
                typeof(ApplicationBootstrapper),
                default(bool),
                propertyChanged: OnAutoWireViewModelChanged
            );

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(AutoWireViewModelProperty, value);
        }

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is Element view))
            {
                return;
            }

            var viewType = view.GetType();
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = String.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

            var viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null)
            {
                return;
            }

            if (ApplicationBootstrapper.AbpBootstrapper?.IocManager == null)
            {
                view.BindingContext = Activator.CreateInstance(viewModelType, true);
                return;
            }

            var viewModel = Resolver.Resolve(viewModelType);
            view.BindingContext = viewModel;
        }
    }
}