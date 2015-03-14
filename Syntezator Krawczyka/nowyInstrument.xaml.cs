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
    public partial class nowyInstrument : Window
    {
        public nowyInstrument()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            laduj(Properties.Resources.przyklad);
            Close();
        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            laduj(Properties.Resources.przyklad2);
            Close();
        }
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            laduj(Properties.Resources.przyklad3);
            Close();
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            laduj(Properties.Resources.przykladMidi);
            Close();

        }


        private void dodaj_click(object sender, RoutedEventArgs e)
        {
            uint ileOs;
            try
            {
                ileOs = uint.Parse(ileOsc.Text);
            }
            catch
            {
                MessageBox.Show("nieprawidłowa wartość w polu ilość oscylatorów", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var document = Statyczne.otwartyplik.xml;
            var sound = document.CreateElement("sound");
            var id = "sound";
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
            sound.Attributes.GetNamedItem("type").Value = "syntezator-krawczyka";
            Statyczne.otwartyplik.xml.GetElementsByTagName("file")[0].AppendChild(sound);
            sound.AppendChild(mod("sekwencer", "sekwencer", "rozdzielacz0"));
            var roz0Out = "zmianaWysokości1";
            for (uint i = 2; i <= ileOs; i++)
            {
                roz0Out += " zmianaWysokości" + i;
            }
            sound.AppendChild(mod("rozdzielacz", "rozdzielacz0", roz0Out));
            int K = 1;
            for (uint i = 1; i <= ileOs; i++)
            {
                if (LFONaWys.IsChecked.Value)
                {
                    sound.AppendChild(mod("zmianaWysokości", "zmianaWysokości" + i, "LFO" + i));
                    sound.AppendChild(mod("lfo", "LFO" + i, "oscylator" + i));
                }
                else
                    sound.AppendChild(mod("zmianaWysokości", "zmianaWysokości" + i, "oscylator" + i));
                int P = 1;
                XmlNode osc;
                if (LFONaGł.IsChecked.Value || LFONaFil.IsChecked.Value || FilADSR.IsChecked.Value || filtr.IsChecked.Value)
                    osc=(mod("oscylator", "oscylator" + i, "P" + P + "nr" + i));
                else
                    osc=(mod("oscylator", "oscylator" + i, "K" + K));
                if(i>1)
                {
                    var atr = Statyczne.otwartyplik.xml.CreateAttribute("S");
                    atr.Value = "0";
                    osc.Attributes.Append(atr);
                }
                sound.AppendChild(osc);

                if (LFONaGł.IsChecked.Value)
                {
                    if (LFONaFil.IsChecked.Value || FilADSR.IsChecked.Value || filtr.IsChecked.Value)
                        sound.AppendChild(mod("lfo", "P" + P + "nr" + i, "P" + (P + 1) + "nr" + i + " PG" + P + "nr" + i));
                    else
                        sound.AppendChild(mod("lfo", "P" + P + "nr" + i, "K" + K + " PG" + P + "nr" + i));
                    sound.AppendChild(mod("glosnosc", "PG" + P + "nr" + i, ""));
                    P++;
                }

                if (LFONaFil.IsChecked.Value)
                {
                    if (FilADSR.IsChecked.Value || filtr.IsChecked.Value)
                        sound.AppendChild(mod("lfo", "P" + P + "nr" + i, "P" + (P + 1) + "nr" + i + " PG" + P + "nr" + i));
                    else
                        sound.AppendChild(mod("lfo", "P" + P + "nr" + i, "K" + K + " PG" + P + "nr" + i));
                    sound.AppendChild(mod("cutoff", "PG" + P + "nr" + i, ""));
                    P++;
                }

                if (FilADSR.IsChecked.Value)
                {
                    if (filtr.IsChecked.Value)
                        sound.AppendChild(mod("generatorObwiedniFiltru", "P" + P + "nr" + i, "P" + (P + 1) + "nr" + i + " PG" + P + "nr" + i));
                    else
                        sound.AppendChild(mod("generatorObwiedniFiltru", "P" + P + "nr" + i, "K" + K + " PG" + P + "nr" + i));
                    sound.AppendChild(mod("cutoff", "PG" + P + "nr" + i, ""));
                    P++;
                }
                if (filtr.IsChecked.Value)
                {
                    sound.AppendChild(mod("cutoff", "P" + P + "nr" + i, "K" + K));
                    P++;
                }


            }
            sound.AppendChild(mod("mikser", "K" + K, "K"+(K+1)));
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
            if (p3!="")
            {
                var output = Statyczne.otwartyplik.xml.CreateAttribute("output");
                output.Value = p3;
                ret.Attributes.Append(output);
            }
            return ret;
        }

        private void osc_Checked(object sender, RoutedEventArgs e)
        {
            ileOsc.IsEnabled = true;
            if (Mik!=null)
            Mik.IsChecked = false;
        }

        private void Mik_Checked(object sender, RoutedEventArgs e)
        {

            ileOsc.IsEnabled = false;
            osc.IsChecked = false;
        }
    }
}
