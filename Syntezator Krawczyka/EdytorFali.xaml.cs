﻿using System;
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
    /// Interaction logic for EdytorFali.xaml
    /// </summary>
    public partial class EdytorFali : Window
    {
        public EdytorFali()
        {
            aktywne = null;
            InitializeComponent();
        }
        public FalaNiestandardowa aktywne;
        public EdytorFali(FalaNiestandardowa wej):base()
        {
            aktywne = wej;
            if(typeof(SkładoweHarmoniczne)==wej.GetType())
            {
                ładuj(wej as SkładoweHarmoniczne);
            }
        }
        void ładuj(SkładoweHarmoniczne wej)
        {
            SkładoweHarmoniczneLista.Children.Clear();
            for(int i=0;i<wej.Składowe.Count;i++)
            {

                SkładoweHarmoniczneLista.Children.Add(pokarzSkładowąHarmoniczną(i+1, wej.Składowe[i]));
            }
            var przyciskDodaj = new Button();
            przyciskDodaj.Content = "Dodaj";
            przyciskDodaj.Click += Button_Click_1;
            SkładoweHarmoniczneLista.Children.Add(przyciskDodaj);
            rysujWykres();
        }
        Grid pokarzSkładowąHarmoniczną(int nr,float ile)
        {

            var jedna = new Grid();
            var tekst = new Button();
            tekst.Content = (nr.ToString());
            tekst.Width = 45;
            tekst.Click += tekst_Click;
            tekst.HorizontalAlignment = HorizontalAlignment.Left;
            jedna.Children.Add(tekst);
            var suwak = new Slider();
            suwak.Maximum = 1;
            suwak.Value = ile;
            suwak.Tag = nr;
            suwak.Margin = new Thickness(50, 0, 0, 0);
            suwak.ValueChanged += suwak_ValueChanged;
            jedna.Children.Add(suwak);
            tekst.Tag = suwak;
            return jedna;
        }

        void tekst_Click(object sender, RoutedEventArgs e)
        {
            ((sender as Button).Tag as Slider).Value = 1f / ((int)((sender as Button).Tag as Slider).Tag);
        }

        void suwak_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (aktywne as SkładoweHarmoniczne).Składowe[(int)(sender as Slider).Tag-1] = (float)(sender as Slider).Value;
            rysujWykres();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            aktywne = new SkładoweHarmoniczne();
            ładuj(aktywne as SkładoweHarmoniczne);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           ( aktywne as SkładoweHarmoniczne).Składowe.Add(0);
           SkładoweHarmoniczneLista.Children.Insert((aktywne as SkładoweHarmoniczne).Składowe.Count - 1, pokarzSkładowąHarmoniczną((aktywne as SkładoweHarmoniczne).Składowe.Count, 0));
           rysujWykres();
        }
        void rysujWykres()
        {

            if (aktywne!=null&&typeof(SkładoweHarmoniczne) == aktywne.GetType())
            {
                var fala = aktywne.generujJedenPrzebieg((long)wykres.ActualWidth);
                wykres.Children.Clear();
                var linia = new Polyline();
                linia.Stroke = Brushes.Black;
                for (int i = 0; i < fala.Length; i++)
                {
                    linia.Points.Add(new Point(i, (fala[i] + 1) / 2 * wykres.ActualHeight));
                }
                wykres.Children.Add(linia);
            }
        }

        private void wykres_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            rysujWykres();
        }
    }
}
