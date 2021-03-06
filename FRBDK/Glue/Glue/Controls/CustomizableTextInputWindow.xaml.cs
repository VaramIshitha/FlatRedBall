﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlatRedBall.Glue.Controls
{


    /// <summary>
    /// Interaction logic for CustomizableTextInputWindow.xaml
    /// </summary>
    public partial class CustomizableTextInputWindow : Window
    {
        public string Message
        {
            get => (string)this.Label.Content;
            set => this.Label.Content = value;
        }

        public string Result
        {
            get => TextBox.Text;
            set => TextBox.Text = value;
        }

        public CustomizableTextInputWindow()
        {
            InitializeComponent();

            TextBox.Focus();

            this.WindowStartupLocation = WindowStartupLocation.Manual;

            // uses winforms:
            System.Drawing.Point point = System.Windows.Forms.Control.MousePosition;
            this.Left = point.X - this.Width/2;
            // not sure why this is so high
            //this.Top = point.Y - this.Height/2;
            this.Top = point.Y - 50;
        }

        public void HighlghtText()
        {
            TextBox.SelectAll();
        }

        public void AddControl(UserControl control, AboveOrBelow aboveOrBelow = AboveOrBelow.Below)
        {
            if(aboveOrBelow == AboveOrBelow.Above)
            {
                AboveTextBoxStackPanel.Children.Add(control);
            }
            else if(aboveOrBelow == AboveOrBelow.Below)
            {
                BelowTextBoxStackPanel.Children.Add(control);
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Escape:
                    this.DialogResult = false;
                    e.Handled = true;
                    break;
                case Key.Enter:
                    this.DialogResult = true;
                    e.Handled = true;
                    break;
            }
        }
    }
}
