//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HtmlFormattedLabelRenderer.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   HtmlFormattedLabelRenderer.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using Foundation;
using IIASA.FloodCitiSense.Mobile.Core.Controls;
using IIASA.FloodCitiSense.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HtmlFormattedLabel), typeof(HtmlFormattedLabelRenderer))]
namespace IIASA.FloodCitiSense.Renderer
{
    public class HtmlFormattedLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Label> e)
        {
            base.OnElementChanged(e);

            var view = (HtmlFormattedLabel)Element;
            if (view == null) return;

            //Original Credits : https://forums.xamarin.com/discussion/23670/how-to-display-html-formatted-text-in-a-uilabel
            var attr = new NSAttributedStringDocumentAttributes();
            var nsError = new NSError();
            attr.DocumentType = NSDocumentType.HTML;

            Control.AttributedText = new NSAttributedString(view.Text, attr, ref nsError);
        }
    }
}