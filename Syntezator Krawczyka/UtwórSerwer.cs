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
        public enum uprawnienia { publiczny, prywatny }
        static Regex reg = new Regex("\"id\":\"([0-9]*)\", ?\"tytul\":\"([^\"]*)\", ?\"plik\":\"([^\"]*)\", ?\"login\":\"([^\"]*)\", ?\"uprawnienia\":\"([^\"]*)\"");
        public long id;
        public string tytul;
        private string plik;
        private string login;
        public uprawnienia upr;
        private string URL
        {
            get
            {
                return "http://jaebe.za.pl/utwory/" + id + plik + ".jms";
            }
        }

        public UtwórSerwer(string x)
        {
            var grupy = reg.Match(x).Groups;
            id = long.Parse(grupy[1].Value);
            tytul = grupy[2].Value;
            plik = grupy[3].Value;
            login = grupy[4].Value;
            if (grupy[5].Value == "publiczne")
            {
                upr = uprawnienia.publiczny;

            }
            else
                upr = uprawnienia.prywatny;
        }
        public System.Windows.UIElement UI
        {
            get
            {
                var grid = new UtwórSerwerUI(this, login, tytul);
                //grid.Background = Brushes.Wheat;
                //grid.Tag = this;
                /* var autLab = new Label();
                 autLab.Content = login;
                 autLab.VerticalAlignment = VerticalAlignment.Top;
                 autLab.Margin = new Thickness(10, 0, 0, 0);
                 var tytLab = new Label();
                 tytLab.Content = tytul;
                 tytLab.VerticalAlignment = VerticalAlignment.Top;
                 tytLab.Margin = new Thickness(0, 25, 0, 0);
                 tytLab.FontSize = 16;
                 grid.Children.Add(autLab);
                 grid.Children.Add(tytLab);*/
                grid.MouseDown += grid_MouseDown;
                return grid;
            }
        }

        private void grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                Statyczne.otwartyplik = new plik(((UtwórSerwer)((FrameworkElement)sender).Tag).URL);
            //MainWindow.oknoLogowanie.Close();
        }

    }
}