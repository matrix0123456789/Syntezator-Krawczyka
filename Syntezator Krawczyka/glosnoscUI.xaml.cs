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
    /// Interaction logic for GranieUI.xaml
    /// </summary>
    public partial class glosnoscUI : UserControl
    {
        public glosnosc parentNode;
        public glosnoscUI(glosnosc thi)
        {
            parentNode = thi;
            InitializeComponent();
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["głośność"] = ((Slider)sender).Value.ToString(CultureInfo.InvariantCulture);
        }
        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["ucinanieWartość"] = ((Slider)sender).Value.ToString(CultureInfo.InvariantCulture);
        }

        void ustawSuwaki()
        {

             slider1.Value= double.Parse(parentNode.ustawienia["głośność"], CultureInfo.InvariantCulture);
        }
        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            ustawSuwaki();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            parentNode.ustawienia["ucinanie"] = "false";
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            parentNode.ustawienia["ucinanie"] = "true";
        }
    }
}
