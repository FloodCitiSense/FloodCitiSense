using Xamarin.Forms;

namespace IIASA.FloodCitiSense.Mobile.Core.Controls
{
    public class ExpandableGrid : Grid
    {
        static readonly Thickness IconMargin = new Thickness(10, 0);

        View content;
        public View Content
        {
            get => content;
            set
            {
                content = value;
                Children.Add(value, 0, 1);
            }
        }

        Label lblDesc = new Label { FontSize = 25, FontAttributes = FontAttributes.Bold, Margin = new Thickness(30, 0) };
        Label icon = new Label { Text = "+", Margin = IconMargin, HorizontalOptions = LayoutOptions.Start, FontSize = 25, FontAttributes = FontAttributes.Bold };

        public string Caption
        {
            set
            {
                lblDesc.Text = value;
            }
        }

        public ExpandableGrid()
        {
            RowDefinitions = new RowDefinitionCollection {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = 0 },
            };
            ColumnDefinitions = new ColumnDefinitionCollection {
                new ColumnDefinition { Width = GridLength.Star },
            };
            Children.Add(icon, 0, 0);
            Children.Add(lblDesc, 0, 0);

            lblDesc.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(LabelClick) });
            icon.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(LabelClick) });
        }

        void LabelClick()
        {
            if (RowDefinitions[1].Height.IsAuto)
            {
                RowDefinitions[1].Height = 0;
                icon.Text = "+";
            }
            else
            {
                RowDefinitions[1].Height = GridLength.Auto;
                icon.Text = "-";
            }
        }
    }
}
