using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Syntezator_Krawczyka
{
    public class suwak : Slider
    {
        [Bindable(true)]
        [Category("Własne")]
        public string Opis
        {
            get
            {
                return _opis;
            }
            set
            {
                ToolTip = value + "\r\n" + this.Value;
                _opis = value;
            }
        }
        string _opis;
        public suwak()
        {
            ValueChanged += suwak_ValueChanged;
            MouseRightButtonDown += suwak_MouseRightButtonDown;
        }

        void suwak_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var okno = new suwakOkno();
            okno.Show();
        }

        void suwak_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var test = sender == this;
            ToolTip = _opis + "\r\n" + base.Value;


        }
    }
}
