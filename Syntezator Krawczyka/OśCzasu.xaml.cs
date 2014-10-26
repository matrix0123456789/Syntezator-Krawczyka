﻿using Microsoft.Win32;
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
    public partial class OśCzasu : Window
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
                panel.Children.Add(lab);
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

            var lab = new Label();
            lab.Content = akt.sciezka.ToString();
            gr.Children.Add(lab);
            panel.Children.Add(gr);
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
            if (((odDo)aktywna.Tag).sciezka.GetType() == typeof(sciezka))
            {
                nazwa.Content = ((sciezka)((odDo)aktywna.Tag).sciezka).nazwa;
                edytSciezka.Visibility = Visibility.Visible;
                edytSample.Visibility = Visibility.Collapsed;
                delay.Text = ( ((sciezka)((odDo)aktywna.Tag).sciezka).delayUstawione).ToString();
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
            public void odsw()
            {

                start = sciezka.delay;
                dlugosc = sciezka.dlugosc;
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
            dialog.Filter = "Pliki dźwiękowe|*.wav;*.wave";
            dialog.ShowDialog();
            foreach (var x in dialog.FileNames)
            {
                var a = new jedenSample(x);
                Statyczne.otwartyplik.sameSample.Add(a);
                while (a.dlugosc == 0)
                    Thread.Sleep(10);
                var akt = new odDo(a);

                //szukaj miejsca na wyświetlenie
                szukaj(akt, Statyczne.otwartyplik.sciezki.Count + Statyczne.otwartyplik.sameSample.Count-1);

                rysujSkala(plik.tempo * dlugosc / (60 * plik.Hz));
            }
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
                    ((sciezka)((odDo)aktywna.Tag).sciezka).delayUstawione = double.Parse(delay.Text);
                    ((sciezka)((odDo)aktywna.Tag).sciezka).xml.Attributes.GetNamedItem("delay").Value = (((sciezka)((odDo)aktywna.Tag).sciezka).delayUstawione.ToString(CultureInfo.InvariantCulture));
                    ((sciezka)((odDo)aktywna.Tag).sciezka).delay = (int)(float.Parse(delay.Text) * 60 * plik.Hz / plik.tempo);
                    aktywna.Margin = new Thickness(((float.Parse(delay.Text)) * skalaX), aktywna.Margin.Top, 0, 0);
                    ((odDo)aktywna.Tag).start = ((sciezka)((odDo)aktywna.Tag).sciezka).delay;

                    rysujSkala(plik.tempo * dlugosc / (60 * plik.Hz));
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
           var sc= Statyczne.otwartyplik.duplikujScierzke(scorg);
           sc.delayUstawione = scorg.delayUstawione;
           sc.delay = scorg.delay;
           var akt = new odDo(sc);
           akt.dlugosc = scorg.dlugosc;
           akt.start = scorg.delay;
            //szukaj miejsca na wyświetlenie
           szukaj(akt, Statyczne.otwartyplik.sciezki.IndexOf(scorg));
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
        }

        private void przytnij_click(object sender, RoutedEventArgs e)
        {
            var scorg = (aktywna.Tag as odDo).sciezka as jedenSample;
            var okno = new EdytujWave(scorg);
            okno.Show();
        }

        private void graj_click(object sender, RoutedEventArgs e)
        {
            Statyczne.otwartyplik.grajStart(new List<sciezka>(){(((odDo)aktywna.Tag).sciezka as sciezka)});
        }
    }
    interface IodDo
    {
        long delay { get; }
        long dlugosc { get; }
    }
}
