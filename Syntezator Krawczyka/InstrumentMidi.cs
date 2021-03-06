﻿using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Syntezator_Krawczyka
{
   public class InstrumentMidi:soundStart, IDisposable
    {

       public UserControl UI;
       public void działaj(nuta input)
       {
           if ((sbyte)(Math.Round(funkcje.ton(input.ilepróbek) * 2)) + 48 < 0)
               return;
           var wiadomość = new NoteEvent(10, kanał, MidiCommandCode.NoteOn, (sbyte)(Math.Round(funkcje.ton(input.ilepróbek) * 2)) + 48, 127);

           urządzenie.Send(wiadomość.GetAsShortMessage());
       }
       public void pusc(nuta input)
       {
           if ((sbyte)(Math.Round(funkcje.ton(input.ilepróbek) * 2)) + 48 < 0)
               return;
           var wiadomość = new NoteEvent(10, kanał, MidiCommandCode.NoteOff, (sbyte)(Math.Round(funkcje.ton(input.ilepróbek) * 2)) + 48, 127);

           urządzenie.Send(wiadomość.GetAsShortMessage());
       }

        public bool czyWłączone
        {
            get {
                return MidiOut.NumberOfDevices >= nrUrządzenia;
            }
        }

        public long symuluj(long p)
        {
            throw new NotImplementedException();
        }
        int _nrUrządzenia = 0;
        public int nrUrządzenia
        {
            get { return _nrUrządzenia; }
            set
            {
                if (value == _nrUrządzenia)
                    return;
                urządzenie = new MidiOut(nrUrządzenia);
                urządzenie.Volume = 0xfffffff;
                instrument = instrument;
                _nrUrządzenia = value;
            }
        }
        public byte kanał = 1;
        MidiOut urządzenie;
        public InstrumentMidi()
        {
            urządzenie = new MidiOut(nrUrządzenia);
            urządzenie.Volume = 0xfffffff;
            UI = new InstrumentMidiUI(this);
        }
        byte _instrument=1;
        public byte instrument
        {
            get { return _instrument; }
            set
            {
            urządzenie.Send((MidiMessage.ChangePatch(value, 1)).RawData);
            _instrument = value;
        } }

        public void Dispose()
        {
            if (urządzenie != null)
                urządzenie.Dispose();
        }
    }
}
