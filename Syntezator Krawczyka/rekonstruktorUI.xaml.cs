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
    public partial class rekonstruktorUI : UserControl
    {
        public rekonstruktor parentNode;
        public rekonstruktorUI(rekonstruktor thi)
        {
            parentNode = thi;
            InitializeComponent();
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["dlugosc"] =(Math.Pow(2,(int) ((Slider)sender).Value)).ToString(CultureInfo.InvariantCulture);
            parentNode.akt();
            Statyczne.otwartyplik.zmiana();
        }

        void ustawSuwaki()
        {

             slider1.Value= Math.Log(double.Parse(parentNode.ustawienia["dlugosc"], CultureInfo.InvariantCulture),2);
        }
        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            ustawSuwaki();
        }

    }
}
