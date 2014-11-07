using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        public Start()
        {
            try
            {
                new Statyczne();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Brakuje pliku NAudio.dll, bez którego program nie może odtwarzać dźwięku.", "Brak pliku NAudio.dll", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd ładowania biblioteki NAudio.dll, bez której program nie może odtwarzać dźwięku.", "Błąd pliku NAudio.dll", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show(ex.ToString());
            } InitializeComponent();
        }


        private void Pusty_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var main = new MainWindow();
                main.Show();
            }
            catch (Exception e2) { MessageBox.Show(e2.ToString(), "Błąd", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
