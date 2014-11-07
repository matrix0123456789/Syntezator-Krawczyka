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
    /// Interaction logic for DrumPad.xaml
    /// </summary>
    public partial class DrumPad : Window
    {
        public DrumPad()
        {
            InitializeComponent();
            rysuj();
        }
        short wierszy = 4, kolumn = 4;
        void rysuj()
        {
            rysuj(kolumn, wierszy);
        }
        void rysuj(short kolumn, short wierszy)
        {
            Pady.Children.Clear();
            Pady.ColumnDefinitions.Clear();
            for (short i = 0; i < kolumn; i++)
            {
                var a = new ColumnDefinition();

                Pady.ColumnDefinitions.Add(a);
            }
            Pady.RowDefinitions.Clear();
            for (short i = 0; i < wierszy; i++)
            {
                var a = new RowDefinition();
                Pady.RowDefinitions.Add(a);
            }

            for (short i = 0; i < wierszy; i++)
            {
                for (short j = 0; j < kolumn; j++)
                {
                    var przyc = new Rectangle();
                    przyc.Stroke = Brushes.Black;
                    if (Statyczne.otwartyplik.DrumLista.Count > i * wierszy + j && Statyczne.otwartyplik.DrumLista[i * wierszy + j].sekw != null)
                        przyc.Fill = Brushes.Red;
                    else
                        przyc.Fill = Brushes.Green;
                    przyc.Tag = i * kolumn + j;
                    przyc.MouseDown += przyc_MouseDown;
                    przyc.MouseUp += przyc_MouseUp;
                    Grid.SetRow(przyc, i);
                    Grid.SetColumn(przyc, j);
                    Pady.Children.Add(przyc);
                }
            }
        }

        void przyc_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && Statyczne.otwartyplik.DrumLista.Count > (int)(sender as FrameworkElement).Tag)
            {
                var jeden = Statyczne.otwartyplik.DrumLista[(int)(sender as FrameworkElement).Tag];
                if (jeden.nuta != null)
                {
                    jeden.nuta.długość = (long)((jeden.nuta.start.ElapsedMilliseconds) * plik.kHz);
                    jeden.nuta = null;
                }
            }
        }

        void przyc_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                if (Statyczne.otwartyplik.DrumLista.Count > (int)((sender as FrameworkElement).Tag))
                    edytowany = Statyczne.otwartyplik.DrumLista[(int)(sender as FrameworkElement).Tag];
                else
                {
                    while ((Statyczne.otwartyplik.DrumLista.Count <= (int)((sender as FrameworkElement).Tag)))
                    {
                        edytowany = new DrumJeden();
                        edytowany.nowyXML();
                        Statyczne.otwartyplik.xml.DocumentElement.AppendChild(edytowany.xml);
                        Statyczne.otwartyplik.DrumLista.Add(edytowany);
                    }
                }
                aktModuły();

                edycja.Visibility = Visibility.Visible;


            }
            else if (e.ChangedButton == MouseButton.Left && Statyczne.otwartyplik.DrumLista.Count > (int)(sender as FrameworkElement).Tag)
            {

                var jeden = Statyczne.otwartyplik.DrumLista[(int)(sender as FrameworkElement).Tag];
                if (jeden.sekw != null)
                {

                    if (jeden.sekw.czyWłączone)
                    {
                        short oktawa = 0;

                        if (jeden.nuta == null)
                        {
                            jeden.nuta = new nuta();
                            lock (klawiaturaKomputera.wszytskieNuty)
                                klawiaturaKomputera.wszytskieNuty.Add(jeden.nuta);
                        }
                        jeden.nuta.ilepróbek = jeden.nuta.ilepróbekNaStarcie = plik.Hz / funkcje.częstotliwość(0, jeden.wysokość / 2f);
                        jeden.nuta.długość = int.MaxValue / 16;
                        jeden.nuta.sekw = jeden.sekw;
                    }
                }
            }
        }
        private void KolumnyT_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                wierszy = short.Parse(WierszeT.Text);
                kolumn = short.Parse(KolumnyT.Text);
            }
            catch { return; }
            rysuj();
        }

        internal DrumJeden edytowany { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            edycja.Visibility = Visibility.Collapsed;
        }
        void aktModuły()
        {
            lock (comboBox1)
            {
                comboBox1.Items.Clear();

                comboBox1.Items.Add(new ComboBoxItem());
                (comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1) as FrameworkElement).Tag = null;
                (comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1) as ComboBoxItem).Content = "(puste)";
                if (edytowany.sekw == null)
                {
                    comboBox1.SelectedItem = comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1);
                }
                foreach (var mod in Statyczne.otwartyplik.moduły)
                {
                    comboBox1.Items.Add(new ComboBoxItem());
                    (comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1) as FrameworkElement).Tag = mod.Value.sekw;
                    (comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1) as ComboBoxItem).Content = mod.Key;
                    if (edytowany.sekw == mod.Value.sekw)
                    {
                        comboBox1.SelectedItem = comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1);
                    }
                }
            }
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                if (comboBox1.SelectedItem != null)
                {
                    edytowany.sekw = (comboBox1.SelectedItem as FrameworkElement).Tag as soundStart;
                    var atr = edytowany.xml.OwnerDocument.CreateAttribute("sound");
                    atr.Value = (string)(comboBox1.SelectedItem as ComboBoxItem).Content;
                    edytowany.xml.Attributes.Append(atr);



                } if (edytowany.sekw != null)
                    (Pady.Children[Statyczne.otwartyplik.DrumLista.IndexOf(edytowany)] as Rectangle).Fill = Brushes.Red;
                else
                    (Pady.Children[Statyczne.otwartyplik.DrumLista.IndexOf(edytowany)] as Rectangle).Fill = Brushes.Green;
            }
            catch (NullReferenceException) { }
        }
        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            edytowany.oktawy = ((short)slider1.Value);
            var atr = edytowany.xml.OwnerDocument.CreateAttribute("oktawy");
            atr.Value = edytowany.oktawy.ToString(CultureInfo.InvariantCulture);
            edytowany.xml.Attributes.Append(atr);
        }

        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            edytowany.wysokość = (float)(Math.Floor(slider2.Value * 2) / 2);
            var atr = edytowany.xml.OwnerDocument.CreateAttribute("note");
            atr.Value = edytowany.wysokość.ToString(CultureInfo.InvariantCulture);
            edytowany.xml.Attributes.Append(atr);

        }
        private void slider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            edytowany.czestotliwosc = ((float)slider3.Value);
            var atr = edytowany.xml.OwnerDocument.CreateAttribute("frequency");
            atr.Value = edytowany.czestotliwosc.ToString(CultureInfo.InvariantCulture);
            edytowany.xml.Attributes.Append(atr);
        }
    }
}
