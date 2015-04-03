using Microsoft.Win32;
using Syntezator_Krawczyka.Synteza;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for EdytorScierzek.xaml
    /// </summary>
    public partial class OśCzasu : zawartośćOkna, IDisposable
    {
        List<List<odDo>> elementy = new List<List<odDo>>();
        float dlugosc
        {
            get
            {
                float ret = 0;
                foreach (var x in elementy)
                {
                    foreach (var y in x)
                    {
                        if (y.start + y.dlugosc > ret)
                            ret = y.start + y.dlugosc;
                    }
                }
                return ret;
            }
        }
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
                szukaj(akt, i + Statyczne.otwartyplik.sciezki.Count);
            }
            rysujSkala(plik.tempo * dlugosc / (60 * plik.Hz));
            tim = new Timer(TimerAkt, null, 30, 30);

        }

        private void TimerAkt(object state)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, (ThreadStart)delegate()
            {
                int bu = 0;
                if (Statyczne.bufor != null)
                    bu = Statyczne.bufor.BufferedBytes;
                var pozycjaSuwaka = granie.graniePrzy - bu / 4;
                PasekPostępu.Margin = new Thickness(pozycjaSuwaka / plik.Hz / 60 * plik.tempo * skalaX, 0, 0, 0);
            });
        }
        float iRysujSkala = 0;
        private void rysujSkala(double p)
        {
            for (; iRysujSkala < p; iRysujSkala += 4)
            {
                var lab = new Label();
                lab.Content = iRysujSkala;
                lab.Margin = new Thickness(iRysujSkala * skalaX, 0, 0, 0);
                lab.HorizontalAlignment = HorizontalAlignment.Left;
                lab.VerticalAlignment = VerticalAlignment.Top;
                panelSkala.Children.Add(lab);
            }
            for (; iRysujSkala > p; iRysujSkala -= 4)
            {
                panelSkala.Children.RemoveAt(panelSkala.Children.Count - 1);
            }
        }

        const double skalaX = 20;
        const double skalaY = 30;
        private void rysuj(odDo akt, int i2, Brush kolor)
        {
            var prostokat = new Rectangle();
            var gr = new Grid();
            gr.Margin = new Thickness((plik.tempo * (akt.start) / (60 * plik.Hz) * skalaX), i2 * skalaY + 25, 0, 0);
            var wid = plik.tempo * akt.dlugosc / (60 * plik.Hz) * skalaX;
            if (wid < 0)
                wid = 0;
            gr.Width = wid;
            gr.Height = (skalaY);
            prostokat.Fill = kolor;
            prostokat.Stroke = strokeNormal;
            gr.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            gr.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            gr.Tag = akt;
            gr.MouseDown += prostokat_MouseClick;
            gr.Children.Add(prostokat);
            akt.sciezka.zmianaDługości += () =>
            {
                wid = plik.tempo * akt.dlugosc / (60 * plik.Hz) * skalaX;
                if (wid < 0)
                    wid = 0;
                gr.Width = wid;
            };
            var lab = new Label();
            lab.Content = akt.sciezka.ToString();
            gr.Children.Add(lab);
            panel.Children.Add(gr);
        }
        static Brush strokeNormal = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
        private Timer tim;
        void szukaj(odDo akt, int i)
        {
            bool znaleniono = false;
            for (int i2 = 0; i2 < elementy.Count; i2++)
            {
                bool znTera = true;
                foreach (var x in elementy[i2])
                {
                    if (
                       (x.start >= akt.start && x.end <= akt.end)
                        ||
                        (x.start <= akt.start && x.end > akt.start)
                        ||
                        (x.start <= akt.end && x.end > akt.end)
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
                    if (Statyczne.otwartyplik.sciezki.Count > i && Statyczne.otwartyplik.sciezki[i].oryginał != null)
                        rysuj(akt, i2, EdytorNut.kolory[Statyczne.otwartyplik.sciezki.IndexOf(Statyczne.otwartyplik.sciezki[i].oryginał) % EdytorNut.kolory.Length]);
                    else rysuj(akt, i2, EdytorNut.kolory[i % EdytorNut.kolory.Length]);


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
                (aktywna.Children[0] as Rectangle).Stroke = strokeNormal;

                (aktywna.Children[0] as Rectangle).StrokeThickness = 1;
            }
            aktywna = (Grid)sender;

            (aktywna.Children[0] as Rectangle).Stroke = Brushes.Black;
            (aktywna.Children[0] as Rectangle).StrokeThickness = 2;
            suwakGlosnosc.Visibility = Visibility.Visible;
            suwakGlosnosc.Value = ((odDo)aktywna.Tag).sciezka.głośność;
            if (((odDo)aktywna.Tag).sciezka.GetType() == typeof(sciezka))
            {
                nazwa.Content = ((sciezka)((odDo)aktywna.Tag).sciezka).nazwa;
                edytSciezka.Visibility = Visibility.Visible;
                edytSample.Visibility = Visibility.Collapsed;
                delay.Text = (((sciezka)((odDo)aktywna.Tag).sciezka).delayUstawione).ToString();
                aktModuły();
            }
            else
            {
                nazwa.Content = ((jedenSample)((odDo)aktywna.Tag).sciezka).sample.plik;
                edytSciezka.Visibility = Visibility.Collapsed;
                edytSample.Visibility = Visibility.Visible;
                SampleDelay.Text = (((jedenSample)((odDo)aktywna.Tag).sciezka).delayUstawione).ToString();
            }

        }
        class odDo
        {
            public IodDo sciezka;
            public long start;
            public long dlugosc
            {
                get
                {
                    if (sciezka.GetType() == typeof(sciezka))
                    {
                        return (long)sciezka.dlugosc;
                    }
                    else if (sciezka.GetType() == typeof(jedenSample))
                    {
                        if ((sciezka as jedenSample).end < (sciezka as jedenSample).dlugosc)
                            return (sciezka as jedenSample).end - (sciezka as jedenSample).start;
                        else
                            return (sciezka as jedenSample).dlugosc - (sciezka as jedenSample).start;
                    }
                    else return 0;

                }
            }
            public odDo(sciezka s)
            {
                sciezka = s;
                start = s.delay;
            }
            public odDo(jedenSample s)
            {
                sciezka = s;
                start = s.delay;
            }
            public void odsw()
            {

                start = sciezka.delay;
            }
            public long end
            {
                get
                {
                    return start + dlugosc;
                }
            }
        }

        public Grid aktywna { get; set; }

        private void edytujNuty_click(object sender, RoutedEventArgs e)
        {
            if (((odDo)aktywna.Tag).sciezka.GetType() == typeof(sciezka))
            {
                EdytorNut okno;
                if (((sciezka)((odDo)aktywna.Tag).sciezka).oryginał != null)
                    okno = new EdytorNut(((sciezka)((odDo)aktywna.Tag).sciezka).oryginał, aktywna.Background);
                else
                    okno = new EdytorNut((sciezka)((odDo)aktywna.Tag).sciezka, aktywna.Background);
                okno.Show();
            }

        }

        private void nowaPróbka_click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Filter = "Plik muzyczny|*.wav;*.wave|Plik muzyczny|*.mp3|Wszystkie plik muzyczne|*.mp3;*.wav;*.wave";
            dialog.FilterIndex = 3;
            dialog.ShowDialog();
            foreach (var x in dialog.FileNames)
            {
                var a = new jedenSample(x);
                Statyczne.otwartyplik.sameSample.Add(a);
                while (a.dlugosc == 0)
                    Thread.Sleep(10);
                var akt = new odDo(a);

                //szukaj miejsca na wyświetlenie
                szukaj(akt, Statyczne.otwartyplik.sciezki.Count + Statyczne.otwartyplik.sameSample.Count - 1);

                rysujSkala(plik.tempo * dlugosc / (60 * plik.Hz));
            }
            Statyczne.otwartyplik.zmiana();
        }

        private void SampleDelay_TextChanged(object sender, TextChangedEventArgs e)
        {
            EdytorNut.usuńLitery((TextBox)sender);
            if (aktywna != null)
            {
                try
                {
                    var n = (((odDo)aktywna.Tag).sciezka as jedenSample);
                    var cz = double.Parse(SampleDelay.Text);
                    var atrybut = Statyczne.otwartyplik.xml.CreateAttribute("delay");
                    atrybut.Value = cz.ToString(CultureInfo.InvariantCulture);
                    n.xml.Attributes.SetNamedItem(atrybut);
                    aktywna.Margin = new Thickness((cz * skalaX), aktywna.Margin.Top, 0, 0);
                    (((odDo)aktywna.Tag).sciezka as jedenSample).delay = (long)(plik.Hz * 60 / plik.tempo * cz);
                    (((odDo)aktywna.Tag).sciezka as jedenSample).delayUstawione = cz;
                    //n.nuta.opuznienie = (long)(plik.Hz * 60 / plik.tempo * cz);
                    (sender as TextBox).Background = Brushes.White;
                    ((odDo)aktywna.Tag).start = (long)(plik.Hz * 60 / plik.tempo * cz);
                    rysujSkala(plik.tempo * dlugosc / (60 * plik.Hz));
                    Statyczne.otwartyplik.zmiana();
                }
                catch (FormatException)
                {
                    (sender as TextBox).Background = Brushes.Red;
                }
            }
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem != null)
                {
                    ((aktywna.Tag as odDo).sciezka as sciezka).sekw = (comboBox1.SelectedItem as FrameworkElement).Tag as soundStart;
                    var atr = ((aktywna.Tag as odDo).sciezka as sciezka).xml.OwnerDocument.CreateAttribute("sound");
                    atr.Value = (string)(comboBox1.SelectedItem as ComboBoxItem).Content;
                    ((aktywna.Tag as odDo).sciezka as sciezka).xml.Attributes.Append(atr);
                    Statyczne.otwartyplik.zmiana();
                }
            }
            catch (NullReferenceException) { }
        }
        private void delay__TextChanged(object sender, TextChangedEventArgs e)
        {
            // if ((aktywna.Tag as sciezka).gotowe)
            {
                try
                {
                    var oddo = ((odDo)aktywna.Tag);
                    ((sciezka)((odDo)aktywna.Tag).sciezka).delayUstawione = double.Parse(delay.Text);
                    ((sciezka)((odDo)aktywna.Tag).sciezka).xml.Attributes.GetNamedItem("delay").Value = (((sciezka)((odDo)aktywna.Tag).sciezka).delayUstawione.ToString(CultureInfo.InvariantCulture));
                    ((sciezka)((odDo)aktywna.Tag).sciezka).delay = (int)(float.Parse(delay.Text) * 60 * plik.Hz / plik.tempo);
                    ((odDo)aktywna.Tag).start = ((sciezka)((odDo)aktywna.Tag).sciezka).delay;

                    double marTop = aktywna.Margin.Top;
                    bool znaleniono = false;
                    for (int i2 = 0; i2 < elementy.Count; i2++)
                    {
                        if (!elementy[i2].Contains(oddo))
                            continue;
                        bool znTera = true;
                        foreach (var x in elementy[i2])
                        {
                            if (x == oddo)
                                continue;
                            if (
                               (x.start >= oddo.start && x.end <= oddo.end)
                                ||
                                (x.start <= oddo.start && x.end > oddo.start)
                                ||
                                (x.start <= oddo.end && x.end > oddo.end)
                                )
                            {
                                znTera = false;
                                break;
                            }
                        }
                        if (znTera)
                        {
                            marTop = aktywna.Margin.Top;
                            znaleniono = true;
                        }
                        else
                        {
                            elementy[i2].Remove(oddo);
                        }
                        break;
                    }
                    if (!znaleniono)
                    {
                        for (int i2 = 0; i2 < elementy.Count; i2++)
                        {
                            bool znTera = true;
                            foreach (var x in elementy[i2])
                            {
                                if (x == oddo)
                                    continue;
                                if (
                                   (x.start >= oddo.start && x.end <= oddo.end)
                                    ||
                                    (x.start <= oddo.start && x.end > oddo.start)
                                    ||
                                    (x.start <= oddo.end && x.end > oddo.end)
                                    )
                                {
                                    znTera = false;
                                    break;
                                }
                            }
                            if (znTera)
                            {
                                marTop = i2 * skalaY + 25;
                                znaleniono = true;
                                break;
                            }
                        }
                    }
                    if (!znaleniono)
                    {
                        var ost = new List<odDo>();
                        ost.Add(oddo); elementy.Add(ost);
                        marTop = skalaY * (elementy.Count - 1) + 25;

                    }
                    aktywna.Margin = new Thickness(((float.Parse(delay.Text)) * skalaX), marTop, 0, 0);



                    rysujSkala(plik.tempo * dlugosc / (60 * plik.Hz));
                    Statyczne.otwartyplik.zmiana();
                }
                catch (System.FormatException)
                {
                    (sender as TextBox).Background = Brushes.Red;
                }
            }
        }
        //int ilemod = 0;
        void aktModuły()
        {
            lock (comboBox1)
            {
                //var cou = Statyczne.otwartyplik.moduły.Count;
                //if (ilemod != cou)
                {
                    comboBox1.Items.Clear();

                    comboBox1.Items.Add(new ComboBoxItem());
                    (comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1) as FrameworkElement).Tag = null;
                    (comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1) as ComboBoxItem).Content = "(puste)";
                    if ((((odDo)aktywna.Tag).sciezka as sciezka).sekw == null)
                    {
                        comboBox1.SelectedItem = comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1);
                    }
                    foreach (var mod in Statyczne.otwartyplik.moduły)
                    {
                        comboBox1.Items.Add(new ComboBoxItem());
                        (comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1) as FrameworkElement).Tag = mod.Value.sekw;
                        (comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1) as ComboBoxItem).Content = mod.Key;
                        if ((((odDo)aktywna.Tag).sciezka as sciezka).sekw == mod.Value.sekw)
                        {
                            comboBox1.SelectedItem = comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1);
                        }
                    }
                    // ilemod = cou;
                }
            }
        }

        private void duplikuj_click(object sender, RoutedEventArgs e)
        {
            var scorg = (aktywna.Tag as odDo).sciezka as sciezka;
            var sc = Statyczne.otwartyplik.duplikujScierzke(scorg);
            sc.delayUstawione = scorg.delayUstawione;
            sc.delay = scorg.delay;
            var akt = new odDo(sc);
            //akt.dlugosc = scorg.dlugosc;
            akt.start = scorg.delay;
            //szukaj miejsca na wyświetlenie
            szukaj(akt, Statyczne.otwartyplik.sciezki.IndexOf(scorg));
            Statyczne.otwartyplik.zmiana();
        }

        private void usuń_click(object sender, RoutedEventArgs e)
        {

            Statyczne.otwartyplik.sciezki.Remove((((odDo)aktywna.Tag).sciezka as sciezka));
            if (Statyczne.otwartyplik.scieżkiZId.ContainsKey((((odDo)aktywna.Tag).sciezka as sciezka).nazwa))
                Statyczne.otwartyplik.scieżkiZId.Remove((((odDo)aktywna.Tag).sciezka as sciezka).nazwa);
            (((odDo)aktywna.Tag).sciezka as sciezka).xml.ParentNode.RemoveChild((((odDo)aktywna.Tag).sciezka as sciezka).xml);
            ((((((odDo)aktywna.Tag).sciezka as sciezka).UI as sciezkaUI).Parent) as WrapPanel).Children.Remove((((odDo)aktywna.Tag).sciezka as sciezka).UI);
            panel.Children.Remove(aktywna);
            aktywna = null;
            edytSciezka.Visibility = Visibility.Collapsed;
            edytSample.Visibility = Visibility.Collapsed;
            suwakGlosnosc.Visibility = Visibility.Collapsed;
            Statyczne.otwartyplik.zmiana();
        }

        private void przytnij_click(object sender, RoutedEventArgs e)
        {
            var scorg = (aktywna.Tag as odDo).sciezka as jedenSample;
            var okno = new EdytujWave(scorg);
            okno.Show();
            //Statyczne.otwartyplik.zmiana();
        }

        private void graj_click(object sender, RoutedEventArgs e)
        {
            Statyczne.otwartyplik.zmiana();
            granie.graniePrzy = (int)(((odDo)aktywna.Tag).sciezka as sciezka).delay;
            Statyczne.otwartyplik.grajStart(new List<sciezka>() { (((odDo)aktywna.Tag).sciezka as sciezka) }, null);
        }

        public void Dispose()
        {
            if (tim != null)
                tim.Dispose();
        }

        private void suwakGlosnosc_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (aktywna != null)
                ((odDo)aktywna.Tag).sciezka.głośność = (float)suwakGlosnosc.Value;
        }

        private void mikrofonnagraj_click(object sender, RoutedEventArgs e)
        {
            var okno = new Nagrywanie();
            okno.Show();
        }
    }
    interface IodDo
    {
        event Action zmianaDługości;
        long delay { get; }
        long dlugosc { get; }
        float głośność { get; set; }
    }
}
