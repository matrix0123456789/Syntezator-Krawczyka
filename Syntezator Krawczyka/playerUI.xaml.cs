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
    /// Interaction logic for player.xaml
    /// </summary>
    public partial class playerUI : UserControl
    {
        public player głównyplayer;
        public playerUI(player th)
        {
            głównyplayer = th;
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            głównyplayer.graj(true);
        }

        private void slider1_KeyUp(object sender, KeyEventArgs e)
        {

            try
            {
                głównyplayer.ustawienia["oktawy"] = short.Parse(slider1.Text).ToString();
            }
            catch { }
        }

        private void slider1_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                głównyplayer.ustawienia["oktawy"] = short.Parse(slider1.Text).ToString();
            }
            catch { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            głównyplayer.graj(false);
        }

    }
}
