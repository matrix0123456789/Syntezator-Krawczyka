using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for Logowanie.xaml
    /// </summary>
    public partial class Logowanie : Window
    {
        public Logowanie()
        {
            InitializeComponent();
            if (Statyczne.serwer.zalogowano)
            {

                zalogowano.Visibility = Visibility.Visible;
                logowanie.Visibility = Visibility.Collapsed;
                wyswietlUtwory(Statyczne.serwer.utworyZalogowanego);
            }
            else
            {
                zalogowano.Visibility = Visibility.Collapsed;
                logowanie.Visibility = Visibility.Visible;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow.oknoLogowanie = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Statyczne.serwer.loguj(login.Text, haslo.Password);
        }

        private void link_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://syntezator.aq.pl");
        }

        private void haslo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(null, null);

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Syntezator_Krawczyka.Properties.Settings.Default.Haslo = Syntezator_Krawczyka.Properties.Settings.Default.Login = "";
            Syntezator_Krawczyka.Properties.Settings.Default.Save();
            Statyczne.serwer = new PolaczenieHTTP();
            zalogowano.Visibility = Visibility.Collapsed;
            logowanie.Visibility = Visibility.Visible;
        }



        internal void wyswietlUtwory(UtworySerwer utworySerwer)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, (ThreadStart)delegate()
                    {
                        if (UtworyStack != null)
                        {
                            UtworyStack.Children.Clear();
                            foreach (var x in utworySerwer)
                            {
                                UtworyStack.Children.Add(x.UI);
                            }
                        }
                    });
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((Action) =>
                {
                    Statyczne.serwer.pobierzUtwory();
                });
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Statyczne.serwer.wyślij(Statyczne.otwartyplik);       
        }
    }
}
