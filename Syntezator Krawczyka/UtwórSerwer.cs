using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Syntezator_Krawczyka
{
   public class UtwórSerwer
   {
       static Regex reg = new Regex("\"id\":\"([0-9]*)\", ?\"tytul\":\"([^\"]*)\", ?\"plik\":\"([^\"]*)\", ?\"login\":\"([^\"]*)\"");
        public long id;
        public string tytul;
        private string plik;
        private string login;
        private string URL
        {
            get
            {
                return "http://syntezator.aq.pl/utwory/" + id + plik + ".synkra";
            }
        }

        public UtwórSerwer(string x)
        {
            var grupy=reg.Match(x).Groups;
            id = long.Parse(grupy[1].Value);
            tytul = grupy[2].Value;
            plik = grupy[3].Value;
            login = grupy[4].Value;
        }
        public System.Windows.UIElement UI
        {
            get
            {
                var grid = new Grid();
                grid.Background = Brushes.Wheat;
                grid.Tag = this;
                var autLab = new Label();
                autLab.Content = login;
                autLab.VerticalAlignment = VerticalAlignment.Top;
                autLab.Margin = new Thickness(10, 0, 0, 0);
                var tytLab = new Label();
                tytLab.Content = tytul;
                tytLab.VerticalAlignment = VerticalAlignment.Top;
                tytLab.Margin = new Thickness(0, 25, 0, 0);
                tytLab.FontSize = 16;
                grid.Children.Add(autLab);
                grid.Children.Add(tytLab);
                grid.MouseDown+=grid_MouseDown;
                return grid;
            }
        }

        private void grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Statyczne.otwartyplik = new plik(((UtwórSerwer)((Grid)sender).Tag).URL);
            MainWindow.oknoLogowanie.Close();
        }
    }
}