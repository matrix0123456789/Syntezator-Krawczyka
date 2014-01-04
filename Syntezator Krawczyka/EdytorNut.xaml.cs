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
    /// Interaction logic for EdytorNut.xaml
    /// </summary>
    public partial class EdytorNut : Window
    {
        sciezka main;
        public EdytorNut(sciezka input)
        {
            InitializeComponent();
            main = input;
            if (input.nuty.Count > 0)
            {
                const double skalaX = 20;
                const double skalaY = 20;
                double ilepróbekMin, ilepróbekMax;
                ilepróbekMax = ilepróbekMin = input.nuty[0].ilepróbekNaStarcie;
                foreach (var x in input.nuty)
                {
                    if (x.ilepróbekNaStarcie > ilepróbekMax)
                    {
                        ilepróbekMax = x.ilepróbekNaStarcie;
                    }
                    else if (x.ilepróbekNaStarcie < ilepróbekMin)
                    {
                        ilepróbekMin = x.ilepróbekNaStarcie;
                    }
                }
                // double tonMin=funkcje.ton(ilepróbekMin)-funkcje.ton(ilepróbekMax)
                double tonMin = funkcje.ton(ilepróbekMin)+1;
                double tonMax = funkcje.ton(ilepróbekMax)-1;
                for (var i = tonMin; i >= tonMax; i = i - .5)
                {
                    var tonTeraz = Math.Round((i+600) % 6, 2);//+600 żeby dla ujemnych nie sprawdzał w odwrotnej kolejności
                    if (tonTeraz == .5 || tonTeraz == 1.5 || tonTeraz == 3 || tonTeraz == 4 || tonTeraz == 5)
                    {
                        var prostokat = new Rectangle();
                        prostokat.Margin = new Thickness(0, (tonMin - i) * skalaY, 0, 0);
                        prostokat.Height = (skalaY / 2);
                        prostokat.Fill = Brushes.Beige;
                        prostokat.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        prostokat.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                        panel.Children.Add(prostokat);
                    } else if (tonTeraz == 0)//oktawa
                    {
                        var prostokat = new Rectangle();
                        prostokat.Margin = new Thickness(0, (tonMin - i) * skalaY, 0, 0);
                        prostokat.Height = (1);
                        prostokat.Fill = Brushes.Black;
                        prostokat.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        prostokat.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                        panel.Children.Add(prostokat);
                    }
                }
                foreach (var x in input.nuty)
                {
                    var prostokat = new Rectangle();
                    prostokat.Margin = new Thickness((plik.tempo * (x.opuznienie - input.delay) / (60 * plik.Hz) * skalaX), (tonMin - funkcje.ton(x.ilepróbekNaStarcie)) * skalaY, 0, 0);

                    prostokat.Width = plik.tempo * x.długość / (60 * plik.Hz) * skalaX;
                    prostokat.Height = (skalaY / 2);
                    prostokat.Fill = Brushes.Red;
                    prostokat.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    prostokat.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    prostokat.Tag = x;
                    prostokat.MouseEnter += prostokat_MouseEnter;
                    panel.Children.Add(prostokat);
                }
            }
        }

        void prostokat_MouseEnter(object sender, MouseEventArgs e)
        {
            aktywna = (Rectangle)sender;
            var nuta = ((sender as Rectangle).Tag as nuta);

        }

        public Rectangle aktywna { get; set; }
    }
}
