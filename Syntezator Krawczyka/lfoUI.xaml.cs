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
    public partial class lfoUI : UserControl
    {
        lfo parentNode;
        public lfoUI(lfo t)
        {
            parentNode = t;
            InitializeComponent();
        }

        private void sinusoidalny_Checked(object sender, RoutedEventArgs e)
        {

            parentNode.ustawienia["typ"] = "sinusoidalna";
        }

        private void trójkątny_Checked(object sender, RoutedEventArgs e)
        {
            parentNode.ustawienia["typ"] = "trójkątna";
        }

        private void prostokątny_Checked(object sender, RoutedEventArgs e)
        {

            parentNode.ustawienia["typ"] = "prostokątna";
        }

        private void piłokształtny_Checked(object sender, RoutedEventArgs e)
        {

            parentNode.ustawienia["typ"] = "piłokształtna";
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["moc"] = (sliderA.Value).ToString(CultureInfo.InvariantCulture);
        }
        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["czestotliwosc"] = (sliderB.Value*50).ToString(CultureInfo.InvariantCulture);
            
        }
        private void slider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["gladkosc"] = sliderC.Value.ToString(CultureInfo.InvariantCulture);
            
        }
        void ustawSuwaki()
        {

            //sliderA.Value = double.Parse(parentNode.ustawienia["przesuniecie"], CultureInfo.InvariantCulture) / 5;
            //sliderB.Value = double.Parse(parentNode.ustawienia["czestotliwosc"], CultureInfo.InvariantCulture);
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            ustawSuwaki();
        }
    }
}
