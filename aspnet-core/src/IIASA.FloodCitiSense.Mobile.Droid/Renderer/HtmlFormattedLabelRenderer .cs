//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HtmlFormattedLabelRenderer .cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   HtmlFormattedLabelRenderer .cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using Android.Content;
using Android.Text;
using Android.Widget;
using IIASA.FloodCitiSense.Mobile.Core.Controls;
using IIASA.FloodCitiSense.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HtmlFormattedLabel), typeof(HtmlFormattedLabelRenderer))]
namespace IIASA.FloodCitiSense.Renderer
{
    public class HtmlFormattedLabelRenderer : LabelRenderer
    {
        public HtmlFormattedLabelRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var view = (HtmlFormattedLabel)Element;
            if (view?.Text == null) return;

            Control.SetText(Html.EscapeHtml(view.Text), TextView.BufferType.Spannable);
        }
    }
}