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
    public partial class Widmo : zawartośćOkna
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
            if (kontenerOkien.gdzieJest.ContainsKey(wid))
                kontenerOkien.gdzieJest[wid].Show();
            else
            {
                var okno = new kontenerOkno(wid);
                kontenerOkien.gdzieJest[wid] = okno;
                okno.Show();
            }

            wid.Start();
            czas = DateTime.Now;

        }
        bool logarytmicznaSkala = false;
        bool wykresMocy = false;
        void rysujSkale()
        {
            skala.Children.Clear();
            var prógPx = 100d;
            if (!logaryticzna)
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
            else
            {


                for (var i = 1; i <5; i++)
                {
                    var Text = new TextBlock();
                    Text.Text = (Math.Pow(10, i)).ToString();

                    Text.Margin = new Thickness(((i - 1) * wykres.ActualWidth / Math.Log10(plik.Hz / 2)), 0, 0, 0);
                    // var ilenapr = plik.Hz / ilepr;
                    //var nrpr = Math.Pow(10, i) / ilenapr;
                    Text.Margin = new Thickness((Math.Log10(Math.Pow(10, i) / plik.Hz) + 3) * wykres.ActualWidth / 3, 0, 0, 0);
                    skala.Children.Add(Text);


                    var Text2 = new TextBlock();
                    Text2.Text = (Math.Pow(10, i)*2).ToString();

                    Text2.Margin = new Thickness((Math.Log10(Math.Pow(10, i)*2 / plik.Hz) + 3) * wykres.ActualWidth / 3, 0, 0, 0);
                    skala.Children.Add(Text2);


                    var Text5 = new TextBlock();
                    Text5.Text = (Math.Pow(10, i)*5).ToString();

                     Text5.Margin = new Thickness((Math.Log10(Math.Pow(10, i)*5 / plik.Hz) + 3) * wykres.ActualWidth / 3, 0, 0, 0);
                    skala.Children.Add(Text5);
                }
            }

        }
        static float[,] aktualniePrzetwarzane;
        static int pozycja = 0;
        private void Start()
        {
            timer = new Thread(akt);
            timer.Start();
            // timer = new Timer(akt, null, 1000 / częstotliwość, 1000 / częstotliwość);
        }

        private void akt(object obj)
        {
            while (true)
            { aktTresc(); }
        }
        static DateTime czas = new DateTime(0);

        DateTime czOst;
        int ilepr = 1024;
        private void aktTresc()
        {
            // while (czOst.AddTicks(100000 / częstotliwość) > DateTime.Now)
            //    Thread.Sleep(1);
            //  var a = (DateTime.Now - czas).TotalMilliseconds;
            //while ((DateTime.Now - czas).TotalMilliseconds < 0)
            //     Thread.Sleep(1);
            czOst = DateTime.Now;
            lock (dane)
            {
                var ilePróbek = ilepr;
                //Title = (DateTime.Now - czas).TotalMilliseconds.ToString();
                //czas = czas.AddTicks(100000 / częstotliwość);
                /*while ((DateTime.Now - czas).TotalMilliseconds > 1000 / częstotliwość * 10)
                {
                    czas = czas.AddTicks(1000 / częstotliwość);
                    /*while (aktualniePrzetwarzane == null && dane.Count == 0)
                    {
                        Thread.Sleep(1);
                    }
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
                }*/


                var zespolone = new Complex[ilePróbek];
                if (aktualniePrzetwarzane == null)
                {

                    while (dane.Count == 0)
                    {
                        Thread.Sleep(1);
                        if (czOst.AddSeconds(ilepr*2 / plik.Hz) < DateTime.Now)
                        {
                            aktualniePrzetwarzane = null;

                            goto rysuj;
                        }
                    }
                    aktualniePrzetwarzane = dane.Dequeue();
                    pozycja = 0;
                }
                try
                {
                    for (int i = 0; i < ilePróbek; i++)
                    {
                        pozycja++;
                        if (pozycja == aktualniePrzetwarzane.Length / 2)
                        {
                            while (dane.Count == 0)
                            {
                                Thread.Sleep(1);
                                if (czOst.AddSeconds(ilepr * 2 / plik.Hz) < DateTime.Now)
                                {
                                    aktualniePrzetwarzane = null;
                                    goto rysuj;
                                }//linia.Points.Add(new Point(wykres.ActualWidth * i / ilePróbek, 0.5 * wykres.ActualHeight));
                                //linia.Points.Add(new Point(wykres.ActualWidth, 0.5 * wykres.ActualHeight));
                                //aktualniePrzetwarzane = null;
                                //pozycja = 0;
                                //wykres.Children.Add(linia);
                                //break;
                            }

                            aktualniePrzetwarzane = dane.Dequeue();
                            pozycja = 0;
                        }
                        zespolone[i] = new Complex(aktualniePrzetwarzane[0, pozycja], 0);
                    }
                }
                catch { }
            rysuj:
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (ThreadStart)delegate()
                {
                    try
                    {
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
                        AForge.Math.FourierTransform.FFT(zespolone, FourierTransform.Direction.Forward);
                        //AForge.Math.FourierTransform.FFT(zespolone, FourierTransform.Direction.Backward);
                        var naJeden = plik.Hz / ilePróbek * 2;
                        for (int i = 0; i < ilePróbek / 2; i++)
                        {

                            double X, Y;
                            if (logaryticzna)
                                X = (Math.Log10((i + 1) / (double)ilePróbek) + 3) * wykres.ActualWidth / 3;
                            else
                                X = i / (double)ilePróbek * wykres.ActualWidth * 2;
                            Y = wykres.ActualHeight - zespolone[i].Magnitude * wykres.ActualHeight * 3;
                            // AForge.Math.FourierTransform.FFT

                            linia.Points.Add(new Point(X, Y));
                        }
                        wykres.Children.Add(linia);

                    }
                    catch { }
                });
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            wid = null;
            timer = null;
        }

        public static Thread timer;

        int częstotliwość { get { return (int)(plik.Hz / ilepr); } }
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
        bool logaryticzna = false;
        private void skalLog_Checked(object sender, RoutedEventArgs e)
        {
            logaryticzna = true;
            rysujSkale();
        }

        private void skalLin_Checked(object sender, RoutedEventArgs e)
        {
            logaryticzna = false;
            rysujSkale();
        }

        private void szybk_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ilepr = (int)Math.Pow(2, Math.Round((sender as suwak).Value));
        }
    }
}
