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
    /// Interaction logic for EdytorScierzek.xaml
    /// </summary>
    public partial class OśCzasu : Window
    {
        List<List<odDo>> elementy = new List<List<odDo>>();

        public OśCzasu()
        {
            InitializeComponent();
            for (int i = 0; i < Statyczne.otwartyplik.sciezki.Count; i++)
            {
                var akt = new odDo(Statyczne.otwartyplik.sciezki[i]);
                //szukaj miejsca na wyświetlenie
                bool znaleniono = false;
                for (int i2 = 0; i2 < elementy.Count; i2++)
                {
                    bool znTera = true;
                    foreach (var x in elementy[i2])
                    {
                        if (
                           (x.start > akt.start && x.end < akt.end)
                            ||
                            (x.start < akt.start && x.end > akt.start)
                            ||
                            (x.start < akt.end && x.end > akt.end)
                            )
                        {
                            znTera = false;
                            break;
                        }
                    }
                    if(znTera)
                    {
                        znaleniono = true;
                        elementy[i2].Add(akt);
                        if(Statyczne.otwartyplik.sciezki[i].oryginał==null)
                        rysuj(akt, i2,EdytorNut.kolory[i%EdytorNut.kolory.Length]);
                        else

                            rysuj(akt, i2, EdytorNut.kolory[Statyczne.otwartyplik.sciezki.IndexOf(Statyczne.otwartyplik.sciezki[i].oryginał) % EdytorNut.kolory.Length]);
                        break;
                    }
                }
                if (!znaleniono)
                {
                    var ost = new List<odDo>();
                    ost.Add(akt);
                    if (Statyczne.otwartyplik.sciezki[i].oryginał == null)
                        rysuj(akt, elementy.Count, EdytorNut.kolory[i % EdytorNut.kolory.Length]);
                    else

                        rysuj(akt, elementy.Count, EdytorNut.kolory[Statyczne.otwartyplik.sciezki.IndexOf(Statyczne.otwartyplik.sciezki[i].oryginał) % EdytorNut.kolory.Length]);
                        
                    elementy.Add(ost);
                }
            }
        }

        const double skalaX = 20;
        const double skalaY = 30;
        private void rysuj(odDo akt, int i2, Brush kolor)
        {
            var prostokat = new Rectangle();
            prostokat.Margin = new Thickness((plik.tempo * (akt.start) / (60 * plik.Hz) * skalaX),i2 * skalaY, 0, 0);
            var wid = plik.tempo * akt.dlugosc / (60 * plik.Hz) * skalaX;
            if (wid < 0)
                wid = 0;
            prostokat.Width = wid;
            prostokat.Height = (skalaY);
            prostokat.Fill = kolor;
            prostokat.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            prostokat.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            prostokat.Tag = akt;
            prostokat.MouseDown += prostokat_MouseClick;
            panel.Children.Add(prostokat);
        }

        private void prostokat_MouseClick(object sender, MouseButtonEventArgs e)
        {
            
                if (aktywna != null)
                    aktywna.Stroke = null;
                aktywna = (Rectangle)sender;

                aktywna.Stroke = Brushes.Green;
                Nazwa.Content = ((odDo)aktywna.Tag).sciezka.nazwa;
            
        }
        struct odDo
        {
            public sciezka sciezka;
            public long start;
            public long dlugosc;
            public odDo(sciezka s)
            {
                sciezka = s;
                start = s.delay;
                dlugosc = (long)s.dlugosc;
            }
            public long end
            {
                get
                {
                    return start+dlugosc;
                }
            }
        }

        public Rectangle aktywna { get; set; }

        private void edytujNuty_click(object sender, RoutedEventArgs e)
        {
            var okno = new EdytorNut(((odDo)aktywna.Tag).sciezka);
            okno.Show();

        }
    }
}
