using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
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
    /// Interaction logic for Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        public Start()
        {

            thi = this;
           // if (polecenia())
            {
                InitializeComponent();
                try
                {
                    if (Syntezator_Krawczyka.Properties.Settings.Default != null)
                    {
                        if (Syntezator_Krawczyka.Properties.Settings.Default.OstatnioOtwarte != null)
                            for (var i = Syntezator_Krawczyka.Properties.Settings.Default.OstatnioOtwarte.Count - 1; i >= 0 && i >= Syntezator_Krawczyka.Properties.Settings.Default.OstatnioOtwarte.Count - 20; i--)
                            {
                                var lab = new Label();
                                var str = Syntezator_Krawczyka.Properties.Settings.Default.OstatnioOtwarte[i];
                                lab.ToolTip = str;
                                lab.Content = str.Substring(1+str.LastIndexOfAny(new char[] { '/', '\\' }));
                                lab.MouseLeftButtonDown += uruchomOstatnie;
                                OstOtw.Children.Add(lab);

                            }
                    }
                }
                catch (Exception e) { MessageBox.Show(e.ToString()); }
            }
            ThreadPool.QueueUserWorkItem((a) => { Backup.czyśćStare(new TimeSpan(14, 0, 0, 0)); });//czyści backup starszy niż 14 dni

        }

        void uruchomOstatnie(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var main = new MainWindow();

                string[] explode = ((string)(sender as Label).ToolTip).Split('.');
                if (explode.Last() == "mid" || explode.Last() == "midi")
                    Statyczne.otwartyplik = new plikmidi((string)(sender as Label).ToolTip);
                else
                    Statyczne.otwartyplik = new plik((string)(sender as Label).ToolTip);
                main.Show();

            }
            catch (Exception e2) { MessageBox.Show(e2.ToString(), "Błąd", MessageBoxButton.OK, MessageBoxImage.Error); }
            Close();
        }
        

        private void Pusty_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var main = new MainWindow();
                Statyczne.otwartyplik = new plik(Syntezator_Krawczyka.Properties.Resources.przyklad, true);
                main.Show();
            }
            catch (Exception e2) { MessageBox.Show(e2.ToString(), "Błąd", MessageBoxButton.OK, MessageBoxImage.Error); }
            Close();

        }

        private void OtwórzClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var main = new MainWindow();
                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.Filter = "Wszystkie pliki z nutami|*.mid;*.midi;*.xml;*.jms|Plik XML|*.xml|Plik Jaebe Music Studio|*.jms|Plik MIDI|*.mid;*.midi|Wszystkie Pliki|*.*";
                dialog.ShowDialog();
                if (dialog.FileName != null)
                {
                    string[] explode = dialog.FileName.Split('.');
                    if (explode.Last() == "mid" || explode.Last() == "midi")
                        Statyczne.otwartyplik = new plikmidi(dialog.FileName);
                    else
                        Statyczne.otwartyplik = new plik(dialog.FileName);
                    main.Show();
                }
            }
            catch (Exception e2) { MessageBox.Show(e2.ToString(), "Błąd", MessageBoxButton.OK, MessageBoxImage.Error); }
            Close();
        }

        public static Window thi;

        private void WyczyśćListę_Click(object sender, RoutedEventArgs e)
        {
            if (Syntezator_Krawczyka.Properties.Settings.Default.OstatnioOtwarte == null)
                Syntezator_Krawczyka.Properties.Settings.Default.OstatnioOtwarte = new System.Collections.Specialized.StringCollection();
            Syntezator_Krawczyka.Properties.Settings.Default.OstatnioOtwarte.Clear();
            Syntezator_Krawczyka.Properties.Settings.Default.Save();
            OstOtw.Children.Clear();
            plik.aktJumpList();
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            MainWindow.Grid_Dropp(sender, e);
        }

        private void wybrano()
        {
            Close();
        }
    }
}
