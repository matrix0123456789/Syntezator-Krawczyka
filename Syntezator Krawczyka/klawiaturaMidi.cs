using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Syntezator_Krawczyka.Synteza;
using System.Windows.Input;
using System.Threading;
using System.Windows;
using NAudio.Midi;

namespace Syntezator_Krawczyka
{
    /// <summary>
    /// Umożliwia granie na żywo z urzyciem urządzenia MIDI np. Keyboard
    /// </summary>
    public class KlawiaturaMidi : wejście
    {
        public UIElement UI { get; set; }
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
        NAudio.Midi.MidiIn wej;
        public KlawiaturaMidi(int i)
        {
            wej = new NAudio.Midi.MidiIn(i);
            var info = NAudio.Midi.MidiIn.DeviceInfo(i);
            UI = new KlawiaturaMidiUI(this, info.Manufacturer.ToString() + ' ' + info.ProductName);
            this.wej = wej;
            wej.Start();

            wej.MessageReceived += wej_MessageReceived;
            akttimer = new Timer((object o) =>
            {
                // akt();
                //MainWindow.dispat.BeginInvoke(System.Windows.Threading.DispatcherPriority.Input, (ThreadStart)delegate() { akt(); });
            }, null, 10, 10);
            //new Timer((object o) => { akt(); }, null, 0, 10);
        }

        private void wej_MessageReceived(object sender, NAudio.Midi.MidiInMessageEventArgs e)
        {
            switch (e.MidiEvent.CommandCode)
            {
                case MidiCommandCode.NoteOn:
                    try
                    {
                        var note = (NoteEvent)e.MidiEvent;
                        if (sekw != null)
                        {

                            if (sekw.czyWłączone)
                            {
                                short t = (short)(note.NoteNumber - 48);

                                nuta prz;
                                if (nuty.ContainsKey(t))
                                    prz = nuty[t];
                                else
                                {
                                    prz = new nuta();
                                    nuty.Add(t, prz);
                                    lock (wszytskieNuty)
                                        wszytskieNuty.Add(prz);
                                }
                                prz.ilepróbek = prz.ilepróbekNaStarcie = plik.Hz / funkcje.częstotliwość(0, t / 2f);
                                prz.długość = int.MaxValue / 16;
                                prz.sekw = sekw;
                            }
                        }
                    }
                    catch
                    {

                    }
                    break;
                case MidiCommandCode.NoteOff:
                    try
                    {
                        var note = (NoteEvent)e.MidiEvent;
                        if (sekw != null)
                        {

                            if (sekw.czyWłączone)
                            {
                                short t = (short)(note.NoteNumber - 48);
                                if (nuty.ContainsKey(t))
                                {
                                    nuty[t].długość = (long)((nuty[t].start.ElapsedMilliseconds) * plik.kHz);
                                    nuty.Remove(t);
                                }
                            }
                        }
                    }
                    catch { }
                    break;
                default: break;
            }
        }

        public void działaj()
        {

        }
        public void klawisz(KeyEventArgs e, bool down)
        {
            lock (nuty)
            {

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
