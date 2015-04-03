using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Syntezator_Krawczyka.JaebeHelp
{
    /// <summary>
    /// Interaction logic for HelpButton.xaml
    /// </summary>
    public partial class HelpButton : UserControl
    {
        public HelpButton()
        {
            InitializeComponent();
        }
        [Bindable(true)]
        public String strona { get; set; }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var okno = new JaebeHelp.HelpWindow(JaebeHelp.HelpDatabase.baza, "pl", strona);
            okno.Show();
        }
    }
}
