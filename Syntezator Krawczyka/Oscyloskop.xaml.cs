﻿using System;
using System.Collections.Generic;
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
    /// Interaction logic for Oscyloskop.xaml
    /// </summary>
    public partial class Oscyloskop : zawartośćOkna, IDisposable
    {
        public static Oscyloskop oscyl = null;
        public static Queue<float[,]> dane = new Queue<float[,]>();

        Oscyloskop()
        {
            InitializeComponent();
        }
        public static void pokarz()
        {
            if (oscyl == null)
                oscyl = new Oscyloskop();
            if(kontenerOkien.gdzieJest.ContainsKey(oscyl))
                kontenerOkien.gdzieJest[oscyl].Show();
            else
            {
                var okno = new kontenerOkno(oscyl);
                kontenerOkien.gdzieJest[oscyl] = okno;
                okno.Show();
            }
            
            oscyl.Start();
            czas = DateTime.Now;

        }

        private void Start()
        {
            timer = new Timer(akt, null, 17, 17);
        }
        static float[,] aktualniePrzetwarzane;
        static int pozycja = 0;
        int typX = 0, typY = 1;
        static DateTime czas = new DateTime(0);
        private void akt(object state)
        {

            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (ThreadStart)delegate()
            {
                lock (dane)
                {
                    var ilePróbek = (int)(plik.Hz / częstotliwość);
                    //Title = (DateTime.Now - czas).TotalMilliseconds.ToString();
                    czas = czas.AddMilliseconds(1000 / częstotliwość);
                    while ((DateTime.Now - czas).TotalMilliseconds > 1000 / częstotliwość*10)
                    {
                    czas=czas.AddMilliseconds(1000 / częstotliwość);
                        if (aktualniePrzetwarzane == null && dane.Count != 0)
                        {

                            aktualniePrzetwarzane = dane.Dequeue();
                            pozycja = 0;
                        }
                        int dododania = ilePróbek;
                        if (aktualniePrzetwarzane != null)
                            while (dododania > 0)
                            {
                                if (aktualniePrzetwarzane.Length / 2 > dododania + pozycja)
                                {
                                    pozycja += dododania;
                                    break;
                                }
                                else if(dane.Count>0)
                                {
                                    dododania -= aktualniePrzetwarzane.Length / 2 - pozycja;
                                    pozycja = 0;
                                    aktualniePrzetwarzane = dane.Dequeue();
                                }
                                else
                                {
                                    break;
                                }
                            }
                    }



                    wykres.Children.Clear();
                    var linia = new Polyline();
                    linia.Stroke = Brushes.Black;
                    if (aktualniePrzetwarzane == null)
                    {
                        if (dane.Count == 0)
                        {

                            linia.Points.Add(new Point(0, 0.5 * wykres.ActualHeight));
                            linia.Points.Add(new Point(wykres.ActualWidth, 0.5 * wykres.ActualHeight));
                            wykres.Children.Add(linia);
                            return;
                        }
                        aktualniePrzetwarzane = dane.Dequeue();
                        pozycja = 0;
                    }
                    int przeskokint = (int)przeskok;
                    for (int i = 0; i < ilePróbek; i += przeskokint)
                    {
                        pozycja += przeskokint;
                        if (pozycja >= aktualniePrzetwarzane.Length / 2)
                        {
                            if (dane.Count == 0)
                            {

                                //linia.Points.Add(new Point(wykres.ActualWidth * i / ilePróbek, 0.5 * wykres.ActualHeight));
                                //linia.Points.Add(new Point(wykres.ActualWidth, 0.5 * wykres.ActualHeight));
                                aktualniePrzetwarzane = null;
                                pozycja = 0;
                                //wykres.Children.Add(linia);
                                break;
                            }
                            aktualniePrzetwarzane = dane.Dequeue();
                            pozycja = 0;
                        }
                        double X, Y;
                        if (typX == 0)
                        {
                            X = (-aktualniePrzetwarzane[0, pozycja] + 2f) / 4f * wykres.ActualHeight;
                        }
                        else if (typX == 1)
                        {
                            X = (-aktualniePrzetwarzane[1, pozycja] + 2f) / 4f * wykres.ActualHeight;
                        }
                        else
                        {
                            X = wykres.ActualWidth * i / ilePróbek;
                        }
                        if (typY == 0)
                        {
                            Y = (-aktualniePrzetwarzane[0, pozycja] + 2f) / 4f * wykres.ActualHeight;
                        }
                        else if (typY == 1)
                        {
                            Y = (-aktualniePrzetwarzane[1, pozycja] + 2f) / 4f * wykres.ActualHeight;
                        }
                        else
                        {
                            Y = wykres.ActualWidth * i / ilePróbek;
                        }
                        linia.Points.Add(new Point(X,Y));
                    }
                    wykres.Children.Add(linia);
                }
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            oscyl = null;
            timer = null;
        }

        public static Timer timer;

         double częstotliwość = 57.90841667912205;
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            MainWindow.thi.Window_KeyDown(sender, e);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            MainWindow.thi.Window_KeyUp(sender, e);

        }

        private void OśX_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            typX = OśX.SelectedIndex;
        }
        private void OśY_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            typY = OśY.SelectedIndex;
        }
        double przeskok = 1;
        private void jakosc_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            przeskok = 100 / jakosc.Value;
        }

        public void Dispose()
        {

            oscyl = null;
            timer = null;
        }
    }
}
