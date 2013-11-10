using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using NAudio.Midi;
using System.Globalization;

namespace Syntezator_Krawczyka
{
    class plikmidi:plik
    {
        public plikmidi(string a)
        {
            NAudio.Midi.MidiFile z = new NAudio.Midi.MidiFile(a);
                var file = xml.DocumentElement;
                for (int i1 = 0; i1 < z.Events.Tracks; i1++)
                {
                    var track = xml.CreateElement("track");
                    track.Attributes.Append(xml.CreateAttribute("sound"));
                    track.Attributes.Item(0).Value = "midi-1";
                    xml.DocumentElement.AppendChild(track);
                    for (int i2 = 0; i2 < z.Events[i1].Count; i2++)
                    {
                        if (z.Events[i1][i2].CommandCode==MidiCommandCode.NoteOn)
                        {
                            var nuta = xml.CreateElement("nute");
                            nuta.Attributes.Append(xml.CreateAttribute("delay"));
                            nuta.Attributes.Append(xml.CreateAttribute("note"));
                            nuta.Attributes.Append(xml.CreateAttribute("octave"));
                            nuta.Attributes.Append(xml.CreateAttribute("duration"));
                            nuta.Attributes.Item(0).Value = ((double)(z.Events[i1][i2] as NoteOnEvent).AbsoluteTime / z.DeltaTicksPerQuarterNote).ToString(CultureInfo.InvariantCulture);
                            nuta.Attributes.Item(1).Value = ((float)(z.Events[i1][i2] as NoteOnEvent).NoteNumber / 2 - 30).ToString(CultureInfo.InvariantCulture);
                            nuta.Attributes.Item(2).Value = "0";
                            var durat = ((double)(z.Events[i1][i2] as NoteOnEvent).DeltaTime / z.DeltaTicksPerQuarterNote);
                            //if (durat < 0.125)
                            //    durat = 0.125;
                            nuta.Attributes.Item(3).Value = durat.ToString(CultureInfo.InvariantCulture);
                            track.AppendChild(nuta);
                        }
                    }
                }
        }
    }
}
