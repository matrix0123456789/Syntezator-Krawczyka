using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace Syntezator_Krawczyka
{
    /// <summary>
    /// Interaction logic for nowyInstrument.xaml
    /// </summary>
    public partial class nowyFiltr : Window
    {
        public nowyFiltr()
        {
            InitializeComponent();
        }

        public static void laduj(String plik)
        {
            var xml = new XmlDocument();
            xml.LoadXml(plik);
            laduj(xml);
        }
        public static void laduj(XmlDocument xml)
        {

            if (Statyczne.otwartyplik != null)
            {


                var soundList = xml.GetElementsByTagName("sound");
                var sound = soundList[0];
                var id = sound.Attributes.GetNamedItem("id").Value;
                for (var i = 1; true; i++)
                {
                    if (!Statyczne.otwartyplik.moduły.ContainsKey(id + i.ToString()))
                    {
                        id = id + i.ToString();
                        break;
                    }

                }
                sound.Attributes.GetNamedItem("id").Value = id;
                var klon = funkcje.klonujXML(Statyczne.otwartyplik.xml, sound);
                Statyczne.otwartyplik.xml.GetElementsByTagName("file")[0].AppendChild(klon);

                Statyczne.otwartyplik.dekoduj();//poprawić na nową referencję

            }
        }



        private void dodaj_click(object sender, RoutedEventArgs e)
        {

            var document = Statyczne.otwartyplik.xml;
            var sound = document.CreateElement("sound");
            var id = "filter";
            for (var i = 1; true; i++)
            {
                if (!Statyczne.otwartyplik.moduły.ContainsKey(id + i.ToString()))
                {
                    id = id + i.ToString();
                    break;
                }

            }
            sound.Attributes.Append(document.CreateAttribute("id"));
            sound.Attributes.GetNamedItem("id").Value = id;
            sound.Attributes.Append(document.CreateAttribute("type"));
            sound.Attributes.GetNamedItem("type").Value = "syntezator-krawczyka-filter";
            Statyczne.otwartyplik.xml.GetElementsByTagName("file")[0].AppendChild(sound);
            sound.AppendChild(mod("sekwencerB", "sekwencerB", "rozdzielacz0"));

            var P = 0;

            var K = 0;
            if (LFONaGł.IsChecked.Value)
            {
                if (LFONaFil.IsChecked.Value || FilADSR.IsChecked.Value || filtr.IsChecked.Value)
                    sound.AppendChild(mod("lfo", "P" + P + "nr", "P" + (P + 1) + "nr" + " PG" + P + "nr" ));
                else
                    sound.AppendChild(mod("lfo", "P" + P + "nr", "K" + K + " PG" + P + "nr"));
                sound.AppendChild(mod("glosnosc", "PG" + P + "nr", ""));
                P++;
            }

            if (LFONaFil.IsChecked.Value)
            {
                if (FilADSR.IsChecked.Value || filtr.IsChecked.Value)
                    sound.AppendChild(mod("lfo", "P" + P + "nr", "P" + (P + 1) + "nr" + " PG" + P + "nr"));
                else
                    sound.AppendChild(mod("lfo", "P" + P + "nr", "K" + K + " PG" + P + "nr"));
                sound.AppendChild(mod("cutoff", "PG" + P + "nr", ""));
                P++;
            }

            if (FilADSR.IsChecked.Value)
            {
                if (filtr.IsChecked.Value)
                    sound.AppendChild(mod("generatorObwiedniFiltru", "P" + P + "nr", "P" + (P + 1) + "nr"  + " PG" + P + "nr" ));
                else
                    sound.AppendChild(mod("generatorObwiedniFiltru", "P" + P + "nr", "K" + K + " PG" + P + "nr" ));
                sound.AppendChild(mod("cutoff", "PG" + P + "nr" , ""));
                P++;
            }
            if (filtr.IsChecked.Value)
            {
                sound.AppendChild(mod("cutoff", "P" + P + "nr" , "K" + K));
                P++;
            }



            sound.AppendChild(mod("mikser", "K" + K, "K" + (K + 1)));
            K++;
            if (flanger.IsChecked.Value)
            {
                sound.AppendChild(mod("rozdzielacz", "K" + K, "Fl1 Fl2 Fl3 Fl4 Fl5 Fl6 Fl7 Fl8"));
                for (byte i2 = 1; i2 <= 8; i2++)
                {
                    var flang = mod("flanger", "Fl" + i2, "K" + (K + 1));
                    var atr1 = Statyczne.otwartyplik.xml.CreateAttribute("przesuniecie");
                    atr1.Value = (rand.NextDouble() * 2).ToString(CultureInfo.InvariantCulture);
                    flang.Attributes.SetNamedItem(atr1);
                    var atr2 = Statyczne.otwartyplik.xml.CreateAttribute("czestotliwosc");
                    atr2.Value = (rand.NextDouble() * 2).ToString(CultureInfo.InvariantCulture);
                    flang.Attributes.SetNamedItem(atr2);
                    sound.AppendChild(flang);
                } K++;
            }
            /*if (rekonst.IsChecked.Value)
            {
                sound.AppendChild(mod("rekonstruktor", "K" + K, "K" + (K + 1)));
                K++;
            }*/
            sound.AppendChild(mod("glosnosc", "K" + K, "K" + (K + 1)));
            K++;
            sound.AppendChild(mod("poglos", "K" + K, "K" + (K + 1)));
            K++;
            sound.AppendChild(mod("granie", "K" + K, ""));

            Statyczne.otwartyplik.dekoduj();//poprawić na nową referencję
            Close();
        }
        Random rand = new Random();
        private static XmlNode mod(string p1, string p2, string p3)
        {
            var ret = Statyczne.otwartyplik.xml.CreateElement("module");
            var type = Statyczne.otwartyplik.xml.CreateAttribute("type");
            type.Value = p1;
            ret.Attributes.Append(type);
            var id = Statyczne.otwartyplik.xml.CreateAttribute("id");
            id.Value = p2;
            ret.Attributes.Append(id);
            if (p3 != "")
            {
                var output = Statyczne.otwartyplik.xml.CreateAttribute("output");
                output.Value = p3;
                ret.Attributes.Append(output);
            }
            return ret;
        }

    }
}
