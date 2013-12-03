using System;
using System.Collections.Generic;
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
using System.Globalization;

namespace Syntezator_Krawczyka.Synteza
{
    /// <summary>
    /// Interaction logic for flangerUI.xaml
    /// </summary>
    public partial class pogłosUI : UserControl
    {
        pogłos parentNode;
        public pogłosUI(pogłos t)
        {
            parentNode = t;
            InitializeComponent();
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["czas"] = (Math.Pow(2, Math.Round(sliderA.Value)) * (plik.tempo / 60)).ToString(CultureInfo.InvariantCulture);
            
        }
        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["zmniejszenie"] = (sliderB.Value).ToString(CultureInfo.InvariantCulture);

        }
        private void slider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["glosnosc"] = (sliderC.Value).ToString(CultureInfo.InvariantCulture);
            
        }
        void ustawSuwaki()
        {

            if(double.Parse(parentNode.ustawienia["czas"], CultureInfo.InvariantCulture)!=0)
                sliderA.Value = Math.Log(60 * double.Parse(parentNode.ustawienia["czas"], CultureInfo.InvariantCulture) / plik.tempo, 2);
            sliderB.Value = double.Parse(parentNode.ustawienia["zmniejszenie"], CultureInfo.InvariantCulture);
            sliderC.Value = double.Parse(parentNode.ustawienia["glosnosc"], CultureInfo.InvariantCulture);
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            ustawSuwaki();
        }
    }
}
