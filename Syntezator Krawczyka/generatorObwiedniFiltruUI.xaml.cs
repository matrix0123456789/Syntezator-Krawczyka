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
    /// Interaction logic for oscylatorUI.xaml
    /// </summary>
    public partial class generatorObwiedniFiltruUI : UserControl
    {
        public generatorObwiedniFiltru parentNode;
        public generatorObwiedniFiltruUI(generatorObwiedniFiltru th)
        {
            parentNode = th;
            InitializeComponent();
        }


        private void sliderA_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["A"] = (sliderA.Value * 2000).ToString(CultureInfo.InvariantCulture);
        }

        private void sliderD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["D"] = (sliderD.Value * 2000).ToString(CultureInfo.InvariantCulture);
        }

        private void sliderS_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["S"] = sliderS.Value.ToString(CultureInfo.InvariantCulture);
        }

        private void sliderR_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["R"] = (sliderR.Value * 2000).ToString(CultureInfo.InvariantCulture);
        }
        void ustawSuwaki()
        {

            sliderA.Value = double.Parse(parentNode.ustawienia["A"], CultureInfo.InvariantCulture) / 2000;
            sliderD.Value = double.Parse(parentNode.ustawienia["D"], CultureInfo.InvariantCulture) / 2000;
            sliderS.Value = double.Parse(parentNode.ustawienia["S"], CultureInfo.InvariantCulture);
            sliderR.Value = double.Parse(parentNode.ustawienia["R"], CultureInfo.InvariantCulture) / 2000;
        }
        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            ustawSuwaki();
        }
    }
}
