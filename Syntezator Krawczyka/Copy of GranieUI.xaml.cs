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

namespace Syntezator_Krawczyka.Synteza
{
    /// <summary>
    /// Interaction logic for GranieUI.xaml
    /// </summary>
    public partial class GranieUI : UserControl
    {
        public granie parentNode;
        public GranieUI(granie thi)
        {
            parentNode = thi;
            InitializeComponent();
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["głośność"] = ((Slider)sender).Value.ToString();
        }
    }
}
