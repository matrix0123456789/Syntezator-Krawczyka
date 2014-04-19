using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Syntezator_Krawczyka.Synteza
{
    /// <summary>
    /// Interaction logic for sekwencer.xaml
    /// </summary>
    public partial class zmianaWysokościUI : UserControl
    {
        public zmianaWysokości głównySekwencer;
        public zmianaWysokościUI(zmianaWysokości th)
        {
            głównySekwencer = th;
            InitializeComponent();
        }
        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            głównySekwencer.ustawienia["oktawy"] = ((short)slider1.Value).ToString(CultureInfo.InvariantCulture);
        }

        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            głównySekwencer.ustawienia["tony"] = (Math.Floor(slider2.Value*2)/2).ToString(CultureInfo.InvariantCulture);
        }
        private void slider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            głównySekwencer.ustawienia["czestotliwosc"] = ((float)slider3.Value).ToString(CultureInfo.InvariantCulture);
        }
        void ustawSuwaki()
        {
            slider1.Value = double.Parse(głównySekwencer.ustawienia["oktawy"], CultureInfo.InvariantCulture);
            slider2.Value = double.Parse(głównySekwencer.ustawienia["tony"], CultureInfo.InvariantCulture);
            slider3.Value = double.Parse(głównySekwencer.ustawienia["czestotliwosc"], CultureInfo.InvariantCulture);
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            ustawSuwaki();
            głównySekwencer.akt();
        }

    }
}
