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
            parentNode.jedenPrzebieg = null;
        }

        private void trójkątny_Checked(object sender, RoutedEventArgs e)
        {
            parentNode.ustawienia["typ"] = "trójkątna";
            parentNode.jedenPrzebieg = null;
        }

        private void prostokątny_Checked(object sender, RoutedEventArgs e)
        {

            parentNode.ustawienia["typ"] = "prostokątna";
            parentNode.jedenPrzebieg = null;
        }

        private void piłokształtny_Checked(object sender, RoutedEventArgs e)
        {

            parentNode.ustawienia["typ"] = "piłokształtna";
            parentNode.jedenPrzebieg = null;
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["moc"] = (sliderA.Value).ToString(CultureInfo.InvariantCulture);
            parentNode.jedenPrzebieg = null;
        }
        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["czestotliwosc"] = (sliderB.Value * sliderB.Value * 150).ToString(CultureInfo.InvariantCulture);
            parentNode.jedenPrzebieg = null;
            
        }
        private void slider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["gladkosc"] = sliderC.Value.ToString(CultureInfo.InvariantCulture);
            parentNode.jedenPrzebieg = null;

        }
        private void slider4_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["kwantyzacja"] = sliderD.Value.ToString(CultureInfo.InvariantCulture);
            parentNode.jedenPrzebieg = null;

        }
        void ustawSuwaki()
        {

            sliderA.Value = double.Parse(parentNode.ustawienia["moc"], CultureInfo.InvariantCulture);
            sliderB.Value = Math.Sqrt(double.Parse(parentNode.ustawienia["czestotliwosc"], CultureInfo.InvariantCulture) / 150);
            sliderC.Value = double.Parse(parentNode.ustawienia["gladkosc"], CultureInfo.InvariantCulture);
            sliderD.Value = double.Parse(parentNode.ustawienia["kwantyzacja"], CultureInfo.InvariantCulture);
            switch (parentNode.ustawienia["typ"])
            {
                case ("sinusoidalna"):
                    sinusoidalny.IsChecked = true;
                    break;
                case ("trójkątna"):
                    trójkątny.IsChecked = true;
                    break;
                case ("prostokątna"):
                    prostokątny.IsChecked = true;
                    break;
                case ("piłokształtna"):
                    piłokształtny.IsChecked = true;
                    break;
            }
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            ustawSuwaki();
            /*if (parentNode.wyjście[0].DrógiModół.GetType() == typeof(oscylator))
            {
                DoOscylatora.Visibility = Visibility.Visible;  
            }
            else
            {
                DoOscylatora.Visibility = Visibility.Collapsed;
            }*/
        }
    }
}
