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
        public UIElement UI { get; set; }
        public static long przetwarzaW = 0;
        /// <summary>
        /// Sekwencer, do którego mają być przekazywane grane nuty.
        /// </summary>
        /// <seealso cref="sekwencer"/>
        public sekwencer sekw { get; set; }
        /// <summary>
        /// uruchamia metodę akt()
        /// </summary>
        /// <seealso cref="akt"/>
        Timer akttimer;
        Dictionary<short, nuta> nuty = new Dictionary<short, nuta>();
        public static List<nuta> wszytskieNuty = new List<nuta>();

        public klawiaturaKomputera()
        {
            UI = new KlawiaturaKomputeraUI(this);
            akttimer = new Timer((object o) =>
            {
                akt();
                //MainWindow.dispat.BeginInvoke(System.Windows.Threading.DispatcherPriority.Input, (ThreadStart)delegate() { akt(); });
            }, null, 10, 10);
            //new Timer((object o) => { akt(); }, null, 0, 10);
        }
        public void działaj()
        {

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
                        for (short ton = -1; ton <= 31; ton++)
                        {
                            Key klawisz;
                            Key klawisz2;
                            switch (ton)
                            {
                                case -1:
                                    klawisz = Key.LeftShift;
                                    klawisz2 = Key.LeftShift;
                                    break;
                                case 0:
                                    klawisz = Key.Z;
                                    klawisz2 = Key.Z;
                                    break;
                                case 1:
                                    klawisz = Key.S;
                                    klawisz2 = Key.S;
                                    break;
                                case 2:
                                    klawisz = Key.X;
                                    klawisz2 = Key.X;
                                    break;
                                case 3:
                                    klawisz = Key.D;
                                    klawisz2 = Key.D;
                                    break;
                                case 4:
                                    klawisz = Key.C;
                                    klawisz2 = Key.C;
                                    break;
                                case 5:
                                    klawisz = Key.V;
                                    klawisz2 = Key.V;
                                    break;
                                case 6:
                                    klawisz = Key.G;
                                    klawisz2 = Key.G;
                                    break;
                                case 7:
                                    klawisz = Key.B;
                                    klawisz2 = Key.B;
                                    break;
                                case 8:
                                    klawisz = Key.H;
                                    klawisz2 = Key.H;
                                    break;
                                case 9:
                                    klawisz = Key.N;
                                    klawisz2 = Key.N;
                                    break;
                                case 10:
                                    klawisz = Key.J;
                                    klawisz2 = Key.J;
                                    break;
                                case 11:
                                    klawisz = Key.M;
                                    klawisz2 = Key.M;
                                    break;
                                case 12:
                                    klawisz = Key.OemComma;
                                    klawisz2 = Key.Q;
                                    break;
                                case 13:
                                    klawisz = Key.L;
                                    klawisz2 = Key.D2;
                                    break;
                                case 14:
                                    klawisz = Key.OemPeriod;
                                    klawisz2 = Key.W;
                                    break;
                                case 15:
                                    klawisz = Key.Oem1;
                                    klawisz2 = Key.D3;
                                    break;
                                case 16:
                                    klawisz = Key.Oem2;
                                    klawisz2 = Key.E;
                                    break;
                                case 17:
                                    klawisz = Key.RightShift;
                                    klawisz2 = Key.R;
                                    break;
                                case 18:
                                    klawisz = Key.Enter;
                                    klawisz2 = Key.D5;
                                    break;
                                case 19:
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
                                    break;
                                default:
                                    continue;
                            }
                            if (Keyboard.IsKeyDown(klawisz)||Keyboard.IsKeyDown(klawisz2))
                            {

                                if (sekw.wyjście[0].DrógiModół != null)
                                {
                                    short oktawa = 0;

                                    nuta prz;
                                    if (nuty.ContainsKey(ton))
                                        prz = nuty[ton];
                                    else
                                    {
                                        prz = new nuta();
                                        nuty.Add(ton, prz);
                                        lock (wszytskieNuty)
                                            wszytskieNuty.Add(prz);
                                    }
                                    prz.ilepróbek = plik.Hz / funkcje.częstotliwość((short)(oktawa+short.Parse(sekw.ustawienia["oktawy"])), ton / 2f);
                                    prz.długość = (long)((prz.start.ElapsedMilliseconds + 100) * plik.kHz);
                                    prz.sekw = sekw;
                                    //object[] tabl = new object[1];
                                    //tabl[0] = prz;
                                    /*System.Threading.ThreadPool.QueueUserWorkItem((o) =>
                                    {
                                    lock (granie.grają) { sekw.wyjście[0].DrógiModół.działaj(prz); }
                                    });*/
                                }
                            }
                            else
                            {
                                if (nuty.ContainsKey(ton))
                                    nuty.Remove(ton);
                            }
                        }
                    });
                }
            }
        }
    }
}
