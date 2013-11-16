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
    public partial class sekwencerUI : UserControl
    {
        public sekwencer głównySekwencer;
        public sekwencerUI(sekwencer th)
        {
            głównySekwencer = th;
            InitializeComponent();
            MainWindow.thi.głównySekwencer = th;
        }
        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            głównySekwencer.ustawienia["oktawy"] = ((short)slider1.Value).ToString(CultureInfo.InvariantCulture);
        }
    }
}
