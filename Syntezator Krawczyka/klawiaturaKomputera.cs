using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Syntezator_Krawczyka.Synteza;
using System.Windows.Input;
using System.Threading;
using System.Windows;

namespace Syntezator_Krawczyka
{
    /// <summary>
    /// Umożliwia granie na żywo bez klawiatury midi za pomocą klawiatury komputerowej
    /// </summary>
    public class klawiaturaKomputera : wejście
    {
        public UIElement UI
        {
            get
            {
                if (_UI == null)
                    _UI = new KlawiaturaKomputeraUI(this);
                return _UI;

            }
        }
        UIElement _UI;
        public static long przetwarzaW = 0;
        /// <summary>
        /// Sekwencer, do którego mają być przekazywane grane nuty.
        /// </summary>
        /// <seealso cref="sekwencer"/>
        public soundStart sekw { get; set; }
        /// <summary>
        /// uruchamia metodę akt()
        /// </summary>
        /// <seealso cref="akt"/>
        Timer akttimer;
        Dictionary<short, nuta> nuty = new Dictionary<short, nuta>();
        public static List<nuta> wszytskieNuty = new List<nuta>();

        public klawiaturaKomputera()
        {
            System.Threading.ThreadPool.QueueUserWorkItem((o) =>
                    {
                        while (Statyczne.otwartyplik == null)
                            Thread.Sleep(100);
                        if (Statyczne.otwartyplik.moduły.Count > 0)
                            sekw = Statyczne.otwartyplik.moduły.ElementAt(0).Value.sekw;
                    });
            /*akttimer = new Timer((object o) =>
            {
                // akt();
                //MainWindow.dispat.BeginInvoke(System.Windows.Threading.DispatcherPriority.Input, (ThreadStart)delegate() { akt(); });
            }, null, 10, 10);*/
            //new Timer((object o) => { akt(); }, null, 0, 10);
        }
        public void działaj()
        {

        }
        public void klawisz(KeyEventArgs e, bool down)
        {
            lock(nuty)
            {
                short t;
                switch (e.Key)
                {
                    case Key.CapsLock:
                        t = -2;
                        break;
                    case Key.LeftShift:
                        t = -1;
                        break;
                    case Key.Z:
                        t = 0;
                        break;
                    case Key.S:
                       t=1;
                        break;
                    case Key.X:
                        t=2;
                        break;
                    case Key.D:
                        t = 3;
                        break;
                    case Key.C:
                        t = 4;
                        break;
                    case Key.V:
                        t = 5;
                        break;
                    case Key.G:
                        t = 6;
                        break;
                    case Key.B:
                        t = 7;
                        break;
                    case Key.H:
                        t = 8;
                        break;
                    case Key.N:
                        t = 9;
                        break;
                    case Key.J:
                        t = 10;
                        break;
                    case Key.M:
                        t = 11;
                        break;
                    case Key.OemComma:
                        t = 12;
                        break;
                    case Key.L:
                        t = 13;
                        break;
                    case Key.OemPeriod:
                        t = 14;
                        break;
                    case Key.Oem1:
                        t = 15;
                        break;
                    case Key.Oem2:
                        t = 16;
                        break;
                    case Key.RightShift:
                        t = 17;
                        break;
                    case Key.Enter:
                        t = 18;
                        break;
                   /* case 19:
                        klawisz = Key.T;
                        klawisz2 = Key.T;
                        break;
                    case 20:
                        klawisz = Key.D6;
                        klawisz2 = Key.D6;
                        break;
                    case 21:
                        klawisz = Key.Y;
                        klawisz2 = Key.Y;
                        break;
                    case 22:
                        klawisz = Key.D7;
                        klawisz2 = Key.D7;
                        break;
                    case 23:
                        klawisz = Key.U;
                        klawisz2 = Key.U;
                        break;
                    case 24:
                        klawisz = Key.I;
                        klawisz2 = Key.I;
                        break;
                    case 25:
                        klawisz = Key.D9;
                        klawisz2 = Key.D9;
                        break;
                    case 26:
                        klawisz = Key.O;
                        klawisz2 = Key.O;
                        break;
                    case 27:
                        klawisz = Key.D0;
                        klawisz2 = Key.D0;
                        break;
                    case 28:
                        klawisz = Key.P;
                        klawisz2 = Key.P;
                        break;
                    case 29:
                        klawisz = Key.Oem4;
                        klawisz2 = Key.Oem4;
                        break;
                    case 30:
                        klawisz = Key.OemPlus;
                        klawisz2 = Key.OemPlus;
                        break;
                    case 31:
                        klawisz = Key.Oem6;
                        klawisz2 = Key.Oem6;
                        break;*/
                    default:
                        return;
                }
                if (down)
                {
                    if (sekw != null)
                    {

                        if (sekw.czyWłączone)
                        {
                            short oktawa = 0;

                            nuta prz;
                            if (nuty.ContainsKey(t))
                                prz = nuty[t];
                            else
                            {
                                prz = new nuta();
                                ///// try
                                //{
                                nuty.Add(t, prz);
                                /*}
                                catch (Exception ezz)
                                {

                                    if (nuty.ContainsKey(t))
                                        prz = nuty[t];
                                    else
                                        throw new Exception("Błąd ze słownikami");
                                }*/
                                lock (wszytskieNuty)
                                    wszytskieNuty.Add(prz);
                            }
                            prz.ilepróbek = prz.ilepróbekNaStarcie = plik.Hz / funkcje.częstotliwość(0, t / 2f);
                            prz.długość = int.MaxValue / 16;
                            prz.sekw = sekw;
                            //object[] tabl = new object[1];
                            //tabl[0] = prz;
                            /*System.Threading.ThreadPool.QueueUserWorkItem((o) =>
                            {
                            lock (granie.grają) { sekw.wyjście[0].DrógiModół.działaj(prz); }
                            });*/
                        }
                    }
                }
                else
                {

                    if (nuty.ContainsKey(t))
                    {
                        nuty[t].długość = (long)((nuty[t].start.ElapsedMilliseconds) * plik.kHz);
                        nuty.Remove(t);
                    }
                }
            }
        }
        /// <summary>
        /// spprawdza, które klawisze są aktualnie wciśnięte
        /// </summary>
        /// <seealso cref="akttimer"/>
        void akt()
        {
            //lock (granie.grają)
            {
                if (sekw != null)
                {
                    MainWindow.dispat.BeginInvoke(System.Windows.Threading.DispatcherPriority.Input, (ThreadStart)delegate()
                    {
                        
                        
                    });
                }
            }
        }
    }
}
