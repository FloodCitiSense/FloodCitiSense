using IIASA.FloodCitiSense.Mobile.Core.Interface;
using IIASA.FloodCitiSense.ViewModels.Map;
using Plugin.InputKit.Shared.Controls;
using System;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI;

namespace IIASA.FloodCitiSense.Views.Map
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage, IXamarinView
    {
        private MapPageModel _bindContext;

        public MapPage()
        {
            InitializeComponent();
            _bindContext = BindingContext as MapPageModel;
            map.UiSettings.ZoomControlsEnabled = true;
            map.UiSettings.CompassEnabled = true;
            map.UiSettings.MyLocationButtonEnabled = true;
        }

        private void LayersClicked(object sender, EventArgs e)
        {
            if (_bindContext != null)
            {
                _bindContext.IsFilterViewVisible = false;
                _bindContext.IsLegendViewVisible = false;
                _bindContext.IsLayerViewVisible = !_bindContext.IsLayerViewVisible;
            }
        }

        private void LegendClicked(object sender, EventArgs e)
        {
            if (_bindContext != null)
            {
                _bindContext.IsFilterViewVisible = false;
                _bindContext.IsLayerViewVisible = false;
                _bindContext.IsLegendViewVisible = !_bindContext.IsLegendViewVisible;
            }
        }

        private void FilterClicked(object sender, EventArgs e)
        {
            if (_bindContext != null)
            {
                _bindContext.IsLayerViewVisible = false;
                _bindContext.IsLegendViewVisible = false;
                _bindContext.IsFilterViewVisible = !_bindContext.IsFilterViewVisible;
            }
        }

        private void MapSwitch_OnToggled(object sender, ToggledEventArgs e)
        {
            this.map.MapType = this.map.MapType == MapType.Satellite ? MapType.Street : MapType.Satellite;
        }

        private void Default_OnClicked(object sender, EventArgs e)
        {
            this.map.MapType = MapType.Street;
            this.Satellite.IsChecked = false;
        }

        private void Satellite_OnClicked(object sender, EventArgs e)
        {
            this.map.MapType = MapType.Satellite;
            this.Default.IsChecked = false;
        }

       

        private void RadioButton_OnClicked(object sender, EventArgs e)
        {
            IsShowAll.IsChecked = false;
            IsLastDay.IsChecked = false;
            IsLastWeek.IsChecked = false;
            IsDateFilter.IsChecked = false;

            if (sender is RadioButton rb)
            {
                rb.IsChecked = true;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //map.InitialCameraUpdate
        }

        private void MaterialRadioButtonGroup_OnSelectedIndexChanged(object sender, SelectedIndexChangedEventArgs e)
        {

        }
    }
}