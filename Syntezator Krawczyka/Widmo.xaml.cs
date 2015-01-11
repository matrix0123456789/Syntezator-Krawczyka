using AForge.Math;
using System;
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
    /// Interaction logic for Widmo.xaml
    /// </summary>
    public partial class Widmo : Window
    {
        public static Widmo wid = null;
        public static Queue<float[,]> dane = new Queue<float[,]>();

        public Widmo()
        {
            InitializeComponent();
            rysujSkale();
        }
        public static void pokarz()
        {
            if (wid == null)
                wid = new Widmo();
            wid.Show();
            wid.Start();
            czas = DateTime.Now;
            

        }
        bool logarytmicznaSkala = false;
        bool wykresMocy = false;
        void rysujSkale()
        {
            skala.Children.Clear();
            var prógPx = 100d;
            if (!logarytmicznaSkala)
            {
                var HzNaPx = plik.Hz / 2 / wykres.ActualWidth;
                var HzNaPróg = prógPx * HzNaPx;
                var log = Math.Log10(HzNaPróg);
                var HzNaPróg10 = Math.Pow(10, Math.Floor(log));
                var mantysa = log - Math.Floor(log);
                if (mantysa > Math.Log10(5))
                    HzNaPróg10 = HzNaPróg10 * 5;
                else if (mantysa > Math.Log10(2))
                    HzNaPróg10 = HzNaPróg10 * 2;
                
                prógPx = HzNaPróg10 / HzNaPx;

                for (var i = 0; i * prógPx < wykres.ActualWidth; i++)
                {
                    var Text = new TextBlock();
                    Text.Text = (HzNaPróg10 * i).ToString();
                    Text.Margin = new Thickness(prógPx * i, 0, 0, 0);
                    skala.Children.Add(Text);
                }
            }

        }
        static float[,] aktualniePrzetwarzane;
        static int pozycja = 0;
        private void Start()
        {
            timer = new Timer(akt, null, 1000 / częstotliwość, 1000 / częstotliwość);
        }
        static DateTime czas = new DateTime(0);

        private void akt(object state)
        {

            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (ThreadStart)delegate()
            {
                try
                {
                    aktTresc();
                }
                catch { }
            });
        }
        int ilepr = 1024;
        private void aktTresc()
        {
            lock (dane)
            {
                var ilePróbek = ilepr;
                Title = (DateTime.Now - czas).TotalMilliseconds.ToString();
                czas = czas.AddMilliseconds(1000 / częstotliwość);
                while ((DateTime.Now - czas).TotalMilliseconds > 1000 / częstotliwość * 10)
                {
                    czas = czas.AddMilliseconds(1000 / częstotliwość);
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
                            else if (dane.Count > 0)
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

                        linia.Points.Add(new Point(0, 0));
                        linia.Points.Add(new Point(wykres.ActualWidth, 0));
                        wykres.Children.Add(linia);
                        return;
                    }
                    aktualniePrzetwarzane = dane.Dequeue();
                    pozycja = 0;
                }
                var zespolone = new Complex[ilePróbek];
                for (int i = 0; i < ilePróbek; i++)
                {
                    pozycja++;
                    if (pozycja == aktualniePrzetwarzane.Length / 2)
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
                    zespolone[i] = new Complex(aktualniePrzetwarzane[0, pozycja], 0);
                }
                AForge.Math.FourierTransform.FFT(zespolone, FourierTransform.Direction.Forward);
                var naJeden = plik.Hz / ilePróbek * 2;
                for (int i = 0; i < ilePróbek / 2; i++)
                {

                    double X, Y;
                    X = i / (double)ilePróbek * wykres.ActualWidth * 2;
                    Y = wykres.ActualHeight - zespolone[i].Magnitude * wykres.ActualHeight * 3;
                    // AForge.Math.FourierTransform.FFT

                    linia.Points.Add(new Point(X, Y));
                }
                wykres.Children.Add(linia);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            wid = null;
            timer = null;
        }

        public static Timer timer;

        int częstotliwość = 48000 / 1024;
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            MainWindow.thi.Window_KeyDown(sender, e);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            MainWindow.thi.Window_KeyUp(sender, e);

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            rysujSkale();
        }
    }
}
