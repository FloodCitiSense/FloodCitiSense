using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using Lottie.Forms;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Animations.Base;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.Mobile.Core.Controls
{
    public class LottieLoader : PopupPage, IDisposable
    {
        private static string _defaultJsonAnimationFile;

        private static BaseAnimation _defaultPopupAnimation = new ScaleAnimation
        {
            ScaleIn = 0.8,
            ScaleOut = 0.8,
            DurationIn = 400,
            DurationOut = 300,
            EasingIn = Easing.SinOut,
            EasingOut = Easing.SinIn
            //HasBackgroundAnimation = true
        };

        private readonly string _jsonAnimationFile;
        private readonly bool _loop;
        private readonly INavigation _navigation;
        private readonly FormattedString _text;
        private bool _disposed;

        public LottieLoader(FormattedString text, string jsonAnimationFile = null, bool loop = true,
            BaseAnimation animation = null, bool autoShow = true)
        {
            if (string.IsNullOrEmpty(_defaultJsonAnimationFile) && string.IsNullOrEmpty(jsonAnimationFile))
                throw new ArgumentException("Please specify a jsonAnimationFile or call Init() to set a deafult one",
                    nameof(jsonAnimationFile));


            _text = text;
            _jsonAnimationFile = jsonAnimationFile;
            _loop = loop;

            if (_text == null) _text = "Loading".Translate();

            if (animation != null)
                SetAnimation(animation);

            SetContent();

            if (autoShow)
                Show();
        }

        public bool IsRunning { get; private set; }

        public void Dispose()
        {
            Dispose(true);
        }

        public static void SetDefaultJsonFile(string defaultJsonAnimationFile)
        {
            if (string.IsNullOrEmpty(defaultJsonAnimationFile))
                throw new ArgumentException(nameof(defaultJsonAnimationFile));

            _defaultJsonAnimationFile = defaultJsonAnimationFile;
        }

        public static void SetDefaultPopupAnimation(BaseAnimation defaultPopupAnimation)
        {
            _defaultPopupAnimation =
                defaultPopupAnimation ?? throw new ArgumentNullException(nameof(defaultPopupAnimation));
        }

        private void SetAnimation(BaseAnimation animation)
        {
            Animation = animation;
        }

        private void SetContent()
        {
            var jsonAnimationFile =
                string.IsNullOrEmpty(_jsonAnimationFile) ? _defaultJsonAnimationFile : _jsonAnimationFile;

            var animation = new AnimationView
            {
                Loop = _loop,
                AutoPlay = true,
                Animation = jsonAnimationFile
            };

            var label1 = new Label
            {
                TextColor = Color.Black,
                Text = "UploadMessage1".Translate(),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            var label2 = new Label
            {
                TextColor = Color.Black,
                Text = "UploadMessage2".Translate(),
                HorizontalOptions = LayoutOptions.StartAndExpand,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.WordWrap
            };

            var label3 = new Label
            {
                TextColor = Color.Black,
                Text = "UploadMessage3".Translate(),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            var button = new Button
            {
                Text = "Ok".Translate(),
                Command = new Command(Close),
                BackgroundColor = Color.Accent,
                TextColor = Color.White
            };

            animation.Play();
            var frame = new Frame { CornerRadius = 10, Margin = 20 };

            var layout = new AbsoluteLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            AbsoluteLayout.SetLayoutBounds(animation, new Rectangle(.5, .1, .5, .2));
            AbsoluteLayout.SetLayoutFlags(animation, AbsoluteLayoutFlags.All);

            AbsoluteLayout.SetLayoutBounds(label1, new Rectangle(.5, .3, .9, .1));
            AbsoluteLayout.SetLayoutFlags(label1, AbsoluteLayoutFlags.All);


            AbsoluteLayout.SetLayoutBounds(label2, new Rectangle(.5, .5, 1, .3));
            AbsoluteLayout.SetLayoutFlags(label2, AbsoluteLayoutFlags.All);

            AbsoluteLayout.SetLayoutBounds(label3, new Rectangle(.5, .75, .9, .1));
            AbsoluteLayout.SetLayoutFlags(label3, AbsoluteLayoutFlags.All);

            AbsoluteLayout.SetLayoutBounds(button, new Rectangle(.5, .9, .4, .1));
            AbsoluteLayout.SetLayoutFlags(button, AbsoluteLayoutFlags.All);

            layout.Children.Add(animation);
            layout.Children.Add(label1);
            layout.Children.Add(label2);
            layout.Children.Add(label3);
            layout.Children.Add(button);
            frame.Content = layout;
            BackgroundColor = Color.Transparent;
            Content = frame;
        }

        public void Show()
        {
            IsRunning = true;
            PopupNavigation.Instance.PushAsync(this);
        }

        public void Close()
        {
            PopupNavigation.Instance.RemovePageAsync(this);
            IsRunning = false;
            Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }
    }
}