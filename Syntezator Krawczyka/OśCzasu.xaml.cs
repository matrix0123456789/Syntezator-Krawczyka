using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                szukaj(akt, i);
            }
            for (int i = 0; i < Statyczne.otwartyplik.sameSample.Count; i++)
            {
                var akt = new odDo(Statyczne.otwartyplik.sameSample[i]);
                //szukaj miejsca na wyświetlenie
                szukaj(akt, i);
            }
        }

        const double skalaX = 20;
        const double skalaY = 30;
        private void rysuj(odDo akt, int i2, Brush kolor)
        {
            var prostokat = new Rectangle();
            prostokat.Margin = new Thickness((plik.tempo * (akt.start) / (60 * plik.Hz) * skalaX), i2 * skalaY, 0, 0);
            var wid = plik.tempo * akt.dlugosc / (60 * plik.Hz) * skalaX;
            if (wid < 0)
                wid = 0;
            prostokat.Width = wid;
            prostokat.Height = (skalaY);
            prostokat.Fill = kolor;
            prostokat.Stroke = strokeNormal;
            prostokat.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            prostokat.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            prostokat.Tag = akt;
            prostokat.MouseDown += prostokat_MouseClick;
            panel.Children.Add(prostokat);
        }
        static Brush strokeNormal = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
        void szukaj(odDo akt, int i)
        {
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
                if (znTera)
                {
                    znaleniono = true;
                    elementy[i2].Add(akt);
                    if (Statyczne.otwartyplik.sciezki[i].oryginał == null)
                        rysuj(akt, i2, EdytorNut.kolory[i % EdytorNut.kolory.Length]);
                    else

                        rysuj(akt, i2, EdytorNut.kolory[Statyczne.otwartyplik.sciezki.IndexOf(Statyczne.otwartyplik.sciezki[i].oryginał) % EdytorNut.kolory.Length]);
                    break;
                }
            }
            if (!znaleniono)
            {
                var ost = new List<odDo>();
                ost.Add(akt);
                if (akt.sciezka.GetType() == typeof(sciezka) && Statyczne.otwartyplik.sciezki[i].oryginał != null)
                    rysuj(akt, elementy.Count, EdytorNut.kolory[Statyczne.otwartyplik.sciezki.IndexOf(Statyczne.otwartyplik.sciezki[i].oryginał) % EdytorNut.kolory.Length]);
                else
                    rysuj(akt, elementy.Count, EdytorNut.kolory[i % EdytorNut.kolory.Length]);
                elementy.Add(ost);
            }
        }
        private void prostokat_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (aktywna != null)
            {
                aktywna.Stroke = strokeNormal;

                aktywna.StrokeThickness = 1;
            }
            aktywna = (Rectangle)sender;

            aktywna.Stroke = Brushes.Black;
            aktywna.StrokeThickness = 2;
            if (((odDo)aktywna.Tag).sciezka.GetType() == typeof(sciezka))
            {
                nazwa.Content = ((sciezka)((odDo)aktywna.Tag).sciezka).nazwa;
                edytSciezka.Visibility = Visibility.Visible;
                edytSample.Visibility = Visibility.Collapsed;
            }
            else
            {
                nazwa.Content = ((jedenSample)((odDo)aktywna.Tag).sciezka).sample.plik;
                edytSciezka.Visibility = Visibility.Collapsed;
                edytSample.Visibility = Visibility.Visible;
                SampleDelay.Text = (((jedenSample)((odDo)aktywna.Tag).sciezka).delay * plik.tempo / plik.Hz / 60f).ToString();
            }

        }
        struct odDo
        {
            public object sciezka;
            public long start;
            public long dlugosc;
            public odDo(sciezka s)
            {
                sciezka = s;
                start = s.delay;
                dlugosc = (long)s.dlugosc;
            }
            public odDo(jedenSample s)
            {
                sciezka = s;
                start = s.delay;
                dlugosc = s.dlugosc;
            }
            public long end
            {
                get
                {
                    return start + dlugosc;
                }
            }
        }

        public Rectangle aktywna { get; set; }

        private void edytujNuty_click(object sender, RoutedEventArgs e)
        {
            if (((odDo)aktywna.Tag).sciezka.GetType() == typeof(sciezka))
            {
                var okno = new EdytorNut((sciezka)((odDo)aktywna.Tag).sciezka);
                okno.Show();
            }

        }

        private void nowaPróbka_click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Filter = "Pliki dźwiękowe|*.wav;*.wave";
            dialog.ShowDialog();
            foreach (var x in dialog.FileNames)
            {
                var a = new jedenSample(x);
                Statyczne.otwartyplik.sameSample.Add(a);
                var akt = new odDo(a);
                //szukaj miejsca na wyświetlenie
                szukaj(akt, 0);
            }
        }

        private void SampleDelay_TextChanged(object sender, TextChangedEventArgs e)
        {
            EdytorNut.usuńLitery((TextBox)sender);
            if (aktywna != null)
            {
                try
                {
                    //var n = (nutaXml)aktywna.Tag;
                    var cz = double.Parse(SampleDelay.Text);
                    var atrybut = Statyczne.otwartyplik.xml.CreateAttribute("delay");
                    atrybut.Value = cz.ToString(CultureInfo.InvariantCulture);
                    // n.xml.Attributes.SetNamedItem(atrybut);
                    aktywna.Margin = new Thickness((cz * skalaX), aktywna.Margin.Top, 0, 0);
                    (((odDo)aktywna.Tag).sciezka as jedenSample).delay = (long)(plik.Hz * 60 / plik.tempo * cz);
                    //n.nuta.opuznienie = (long)(plik.Hz * 60 / plik.tempo * cz);
                    (sender as TextBox).Background = Brushes.White;
                }
                catch (FormatException)
                {
                    (sender as TextBox).Background = Brushes.Red;
                }
            }
        }
    }
}
