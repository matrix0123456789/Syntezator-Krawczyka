using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Backup.xaml
    /// </summary>
    public partial class Backup : Window
    {
        public Backup()
        {
            InitializeComponent();
            rysuj();
        }
        static Regex szukajDaty = new Regex("kopia([0-9]+)\\.(jms|synkra)");
        void rysuj()
        {
            var p = pliki;
            lista.Children.Clear();
            for (int i = p.Length - 1; i >= 0; i--)
            {
                var x = p[i];
                var grid = new Grid();
                var lab = new Label(); try
                {
                    var data = DateTime.FromFileTime(long.Parse(szukajDaty.Match(x.Name).Groups[1].Value));

                    lab.Content = data.ToShortDateString() + " " + data.ToShortTimeString();
                    grid.Children.Add(lab);

                    var but1 = new Button();
                    but1.Content = "otwórz";
                    but1.HorizontalAlignment = HorizontalAlignment.Right;
                    but1.Click += but1_Click;
                    but1.Tag = x;
                    but1.Margin = new Thickness(0, 2, 45, 2);
                    grid.Children.Add(but1);

                    var but2 = new Button();
                    but2.Content = "usuń";
                    but2.HorizontalAlignment = HorizontalAlignment.Right;
                    but2.Click += but2_Click;
                    but2.Tag = x;
                    but2.Margin = new Thickness(0, 2, 5, 2);
                    grid.Children.Add(but2);

                    lista.Children.Add(grid);
                }
                catch { continue; }
            }
        }

        void but2_Click(object sender, RoutedEventArgs e)
        {
            ((sender as Button).Tag as FileInfo).Delete();
            rysuj();
        }

        void but1_Click(object sender, RoutedEventArgs e)
        {

            Statyczne.otwartyplik = new plik(((sender as Button).Tag as FileInfo).FullName);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var p = pliki;
            foreach (var x in p)
            {
                x.Delete();
            }
            rysuj();
        }
        static public void czyśćStare(TimeSpan czas)
        {
            var p = pliki;
            for (int i = p.Length - 1; i >= 0; i--)
            {
                var x = p[i];
                try
                {
                    var data = DateTime.FromFileTime(long.Parse(szukajDaty.Match(x.Name).Groups[1].Value));
                    if (DateTime.Now - data > czas)
                        x.Delete();
                }
                catch { }
            }
        }
        static FileInfo[] pliki
        {
            get
            {
                var fold = new DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\SyntezatorKrawczyka");
                return fold.GetFiles("kopia*");
            }
        }
    }
}
