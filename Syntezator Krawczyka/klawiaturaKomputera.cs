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
   public enum typKlawiaturyKomputera:byte { dolna, górna }
    /// <summary>
    /// Umożliwia granie na żywo bez klawiatury midi za pomocą klawiatury komputerowej
    /// </summary>
    public class klawiaturaKomputera : wejście, IDisposable
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

        public klawiaturaKomputera(typKlawiaturyKomputera t)
        {
            typ = t;
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
            lock (nuty)
            {
                short t;
                if (typ == typKlawiaturyKomputera.dolna)
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
                            t = 1;
                            break;
                        case Key.X:
                            t = 2;
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
                        default:
                            return;
                    }
                else
                    switch (e.Key)
                    {
                        case Key.OemTilde:
                            t = -2;
                            break;
                        case Key.Tab:
                            t = -1;
                            break;
                        case Key.Q:
                            t = 0;
                            break;
                        case Key.D2:
                            t = 1;
                            break;
                        case Key.W:
                            t = 2;
                            break;
                        case Key.D3:
                            t = 3;
                            break;
                        case Key.E:
                            t = 4;
                            break;
                        case Key.R:
                            t = 5;
                            break;
                        case Key.D5:
                            t = 6;
                            break;
                        case Key.T:
                            t = 7;
                            break;
                        case Key.D6:
                            t = 8;
                            break;
                        case Key.Y:
                            t = 9;
                            break;
                        case Key.D7:
                            t = 10;
                            break;
                        case Key.U:
                            t = 11;
                            break;
                        case Key.I:
                            t = 12;
                            break;
                        case Key.D9:
                            t = 13;
                            break;
                        case Key.O:
                            t = 14;
                            break;
                        case Key.D0:
                            t = 15;
                            break;
                        case Key.P:
                            t = 16;
                            break;
                        case Key.Oem4:
                            t = 17;
                            break;
                        case Key.OemPlus:
                            t = 18;
                            break;
                        case Key.Oem6:
                            t = 19;
                            break;
                        case Key.Back:
                            t = 20;
                            break;
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
                            prz.ilepróbek = prz.ilepróbekNaStarcie = (plik.Hz / funkcje.częstotliwość(0, t / 2f)) / Math.Pow(2, oktawy + (tony / 6));
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

        public typKlawiaturyKomputera typ;
        public short oktawy;
        public double tony;

        public void Dispose()
        {
            if (_UI != null)
                (_UI as KlawiaturaKomputeraUI).Dispose();
            if (akttimer != null)
                (akttimer).Dispose();
            _UI = null;
        }
    }
}
