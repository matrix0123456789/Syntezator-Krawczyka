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
using System.Windows.Shapes;

namespace Syntezator_Krawczyka
{
    /// <summary>
    /// Interaction logic for preloader.xaml
    /// </summary>
    public partial class preloader : Window
    {
        public preloader()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var okno = new MainWindow();
            okno.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Brak pliku NAudio.dll", "Brakuje pliku, bez którego nie można uruchomić programu.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
