using Abp.Dependency;
using IIASA.FloodCitiSense.Mobile.Core.Core;
using IIASA.FloodCitiSense.Mobile.Core.Models.Common;
using Plugin.InputKit.Shared.Controls;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using CheckBox = Plugin.InputKit.Shared.Controls.CheckBox;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Element
{
    public class ElementService : IElementService, ISingletonDependency
    {
        public void RenderElements(StackLayout layout)
        {

            var text = ReadJson();

            var json = Elements.FromJson(text);

            foreach (var elementSection in json.Sections)
            {
                if (elementSection?.Elements != null)
                    foreach (var element in elementSection?.Elements)
                    {
                        if (element != null)
                        {
                            var view = RenderUI(element);
                            layout.Children.Add(view);
                        }
                    }
            }
        }

        private View RenderUI(Models.Common.Element element)
        {
            var layout = new StackLayout();
            try
            {
                layout.Orientation = StackOrientation.Horizontal;
                layout.HorizontalOptions = LayoutOptions.FillAndExpand;
                layout.VerticalOptions = LayoutOptions.CenterAndExpand;
                layout.Children.Add(new Label
                {
                    Text = element.Caption,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center

                });
                switch (element.Type)
                {
                    case "boolean":
                    case "bool":
                        layout.Children.Add(new CheckBox
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Center,
                        });
                        break;
                    case "radio":
                        layout.Children.Add(new RadioButton
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Center
                        });
                        break;
                    case "checkbox":
                        layout.Children.Add(new CheckBox
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Center
                        });
                        break;
                    case "entry":
                        layout.Children.Add(new Entry
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Center
                        });
                        break;
                    case "password":
                        layout.Children.Add(new Entry
                        {
                            IsPassword = true,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Center
                        });
                        break;
                    case "string":
                        layout.Children.Add(new Label
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Center
                        });
                        break;
                    case "datetime":
                    case "date":
                        layout.Children.Add(new DatePicker
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Center
                        });
                        break;
                    case "time":
                        layout.Children.Add(new TimePicker
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Center
                        });
                        break;
                    case "html":
                        layout.Children.Add(new WebView
                        {
                            Source = element.Url
                        });
                        break;
                    default:

                        break;
                }
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }

            return layout;
        }

        private static string ReadJson()
        {
            var assembly = typeof(Elements).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(Constant.ElementJson);
            string text = "";
            if (stream != null)
                using (var reader = new System.IO.StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }

            return text;
        }
    }
}