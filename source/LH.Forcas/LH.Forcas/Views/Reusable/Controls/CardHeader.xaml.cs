﻿using Xamarin.Forms;

namespace LH.Forcas.Views.Reusable.Controls
{
    public partial class CardHeader
    {
        public static readonly BindableProperty HeaderTextProperty = BindableProperty.Create(
           nameof(HeaderText),
           typeof(string),
           typeof(GroupHeader),
           propertyChanged: UpdateLabelText);

        private static void UpdateLabelText(BindableObject bindable, object oldvalue, object newvalue)
        {
            var header = (CardHeader)bindable;
            header.HeaderLabel.Text = (string)newvalue;
        }

        public CardHeader()
        {
            this.InitializeComponent();
        }

        public string HeaderText
        {
            get { return (string)this.GetValue(HeaderTextProperty); }
            set { this.SetValue(HeaderTextProperty, value); }
        }
    }
}
