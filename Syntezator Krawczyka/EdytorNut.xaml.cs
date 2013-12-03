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
                const double skalaX=10;
                const double skalaY=10;
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
                double tonMin=funkcje.ton(ilepróbekMin);
                double tonMax=funkcje.ton(ilepróbekMax);
                foreach (var x in input.nuty)
                {
                    var prostokat = new Rectangle();
                    prostokat.Margin = new Thickness((plik.tempo * x.opuznienie / (60 * plik.Hz) * skalaX), (tonMin - funkcje.ton(x.ilepróbekNaStarcie)) * skalaY, 0, 0);

                        prostokat.Width = plik.tempo * x.długość / (60*plik.Hz) * skalaX;
                    prostokat.Height=( skalaY / 2);
                    prostokat.Fill = Brushes.Red;
                    prostokat.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    prostokat.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    panel.Children.Add(prostokat);
                }
            }
        }
    }
}
