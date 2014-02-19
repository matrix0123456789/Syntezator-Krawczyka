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
    public partial class cutoffUI : UserControl
    {
        cutoff parentNode;
        public cutoffUI(cutoff t)
        {
            parentNode = t;
            InitializeComponent();
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["moc"] = (sliderA.Value * 5).ToString(CultureInfo.InvariantCulture);
            parentNode.akt();
        }
        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["gladkosc"] = (sliderB.Value).ToString(CultureInfo.InvariantCulture);
            parentNode.akt();
            
        }
        void ustawSuwaki()
        {

            sliderA.Value = double.Parse(parentNode.ustawienia["moc"], CultureInfo.InvariantCulture) / 5;
            //sliderB.Value = double.Parse(parentNode.ustawienia["czestotliwosc"], CultureInfo.InvariantCulture);
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            ustawSuwaki();
            parentNode.akt();
        }
    }
}
