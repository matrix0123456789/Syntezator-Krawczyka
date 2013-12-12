using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NAudio.Midi;
using Syntezator_Krawczyka.Synteza;
using System.Windows;

namespace Syntezator_Krawczyka
{
    class MidiIn:wejście
    {
        NAudio.Midi.MidiIn urządzenie;
        public UIElement UI { get; set; }
        public soundStart sekw { get; set; }
        public MidiIn(int numerUrządzenia)
        {
            urządzenie = new NAudio.Midi.MidiIn(numerUrządzenia);
            urządzenie.MessageReceived += urządzenie_MessageReceived;
            urządzenie.Start();
        }

        void urządzenie_MessageReceived(object sender, MidiInMessageEventArgs e)
        {
            if (MidiEvent.IsNoteOn(e.MidiEvent))
            {
                klawiaturaKomputera.wszytskieNuty.Add(new nuta(440,2000));
            }
        }
        public void działaj()
        {

        }
    }
}
