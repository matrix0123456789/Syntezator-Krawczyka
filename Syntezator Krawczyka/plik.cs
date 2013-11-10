using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Syntezator_Krawczyka.Synteza;
using System.Globalization;
namespace Syntezator_Krawczyka
{
    public class plik
    {
        public string URL;
        public XmlDocument xml;
        public Dictionary<string, Dictionary<string, moduł>> moduły = new Dictionary<string, Dictionary<string, moduł>>();
        public List<sciezka> sciezki = new List<sciezka>();
        public float tempo = 120;
        public static float kHz = 48f;
        public static float Hz = kHz*1000;
        public plik(string a)
        {
            if (a != "")
            {
                URL = a;
                xml = new XmlDocument();
                try
                {
                    xml.Load(URL);
                }
                catch (System.Xml.XmlException e)
                {
                    System.Windows.MessageBox.Show("Błąd w otwieranym pliku\n" + e.ToString(), "Błąd", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                catch (System.IO.FileNotFoundException e)
                {
                    System.Windows.MessageBox.Show("Plik nie istnieje\n" + e.ToString(), "Błąd", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show("Błąd przy otwieraniu pliku\n" + e.ToString(), "Błąd", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                try
                {
                    tempo = float.Parse(xml.GetElementsByTagName("file")[0].Attributes.GetNamedItem("tempo").Value, CultureInfo.InvariantCulture);
                }
                    
                catch { }
                dekoduj();
            }
        }
        public plik(string a, bool czyXML)
        {
            if (a != "")
            {
                URL = a;
                xml = new XmlDocument();
                try
                {
                    xml.LoadXml(a);
                }
                catch (System.Xml.XmlException e)
                {
                    System.Windows.MessageBox.Show("Błąd w otwieranym pliku\n" + e.ToString(), "Błąd", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                catch (System.IO.FileNotFoundException e)
                {
                    System.Windows.MessageBox.Show("Plik nie istnieje\n" + e.ToString(), "Błąd", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show("Błąd przy otwieraniu pliku\n" + e.ToString(), "Błąd", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                try
                {
                    tempo = float.Parse(xml.GetElementsByTagName("file")[0].Attributes.GetNamedItem("tempo").Value, CultureInfo.InvariantCulture);
                }
                catch { }
                dekoduj();
            }
        }
        void dekoduj()
        {
            foreach (XmlNode n in xml.GetElementsByTagName("sound"))
            {
                if (n.Attributes.GetNamedItem("type").Value == "syntezator-krawczyka")
                {
                    moduły.Add(n.Attributes.GetNamedItem("id").Value, new Dictionary<string, moduł>());
                    foreach (XmlNode nn in n.ChildNodes)
                    {
                        if (nn.Name == "module")
                        {
                            switch (nn.Attributes.GetNamedItem("type").Value)
                            {
                                case "sekwencer":


                                    moduły[n.Attributes.GetNamedItem("id").Value].Add(nn.Attributes.GetNamedItem("id").Value, new sekwencer());
                                    moduły[n.Attributes.GetNamedItem("id").Value]["<sekwencer"] = moduły[n.Attributes.GetNamedItem("id").Value][nn.Attributes.GetNamedItem("id").Value];
                                    break;
                                case "player":


                                    moduły[n.Attributes.GetNamedItem("id").Value].Add(nn.Attributes.GetNamedItem("id").Value, new player());
                                    moduły[n.Attributes.GetNamedItem("id").Value]["<player"] = moduły[n.Attributes.GetNamedItem("id").Value][nn.Attributes.GetNamedItem("id").Value];
                                    break;
                                case "granie":


                                    moduły[n.Attributes.GetNamedItem("id").Value].Add(nn.Attributes.GetNamedItem("id").Value, new granie());
                                    break;
                                case "oscylator":


                                    moduły[n.Attributes.GetNamedItem("id").Value].Add(nn.Attributes.GetNamedItem("id").Value, new oscylator());
                                    break;
                                case "flanger":


                                    moduły[n.Attributes.GetNamedItem("id").Value].Add(nn.Attributes.GetNamedItem("id").Value, new flanger());
                                    break;
                                case "rozdzielacz":


                                    moduły[n.Attributes.GetNamedItem("id").Value].Add(nn.Attributes.GetNamedItem("id").Value, new rozdzielacz());
                                    break;
                                case "mikser":


                                    moduły[n.Attributes.GetNamedItem("id").Value].Add(nn.Attributes.GetNamedItem("id").Value, new mikser());
                                    break;
                                case "lfo":


                                    moduły[n.Attributes.GetNamedItem("id").Value].Add(nn.Attributes.GetNamedItem("id").Value, new lfo());
                                    break;
                                case "zmianaWysokości":


                                    moduły[n.Attributes.GetNamedItem("id").Value].Add(nn.Attributes.GetNamedItem("id").Value, new zmianaWysokości());
                                    break;
                                case "glosnosc":


                                    moduły[n.Attributes.GetNamedItem("id").Value].Add(nn.Attributes.GetNamedItem("id").Value, new glosnosc());
                                    break;
                                case "poglos":


                                    moduły[n.Attributes.GetNamedItem("id").Value].Add(nn.Attributes.GetNamedItem("id").Value, new pogłos());
                                    break;
                                case "cutoff":


                                    moduły[n.Attributes.GetNamedItem("id").Value].Add(nn.Attributes.GetNamedItem("id").Value, new cutoff());
                                    break;
                                default:
                                    continue;
                            }
                            moduły[n.Attributes.GetNamedItem("id").Value][nn.Attributes.GetNamedItem("id").Value].XML = nn;
                            if (moduły[n.Attributes.GetNamedItem("id").Value][nn.Attributes.GetNamedItem("id").Value].ustawienia == null)
                            { }
                            modułFunkcje.czytajXML(moduły[n.Attributes.GetNamedItem("id").Value][nn.Attributes.GetNamedItem("id").Value].ustawienia, nn);
                        }
                    }
                }

            }
            foreach (XmlNode n in xml.GetElementsByTagName("sound"))
            {
                if (n.Attributes.GetNamedItem("type").Value == "syntezator-krawczyka")
                {
                    foreach (XmlNode nn in n.ChildNodes)
                    {
                        if (nn.Name == "module")
                        {
                            try
                            {
                                string[] exp = nn.Attributes.GetNamedItem("output").Value.Split(' ');
                                for (int az = 0; az < exp.Length; az++)
                                {
                                    try { moduły[n.Attributes.GetNamedItem("id").Value][nn.Attributes.GetNamedItem("id").Value].wyjście[az].DrógiModół = moduły[n.Attributes.GetNamedItem("id").Value][exp[az]]; }
                                    catch { }
                                }
                            }
                            catch (NullReferenceException e) { }

                        }
                    }
                }

            }

            foreach (XmlNode n in xml.GetElementsByTagName("track"))
            {
                sciezka scie = new sciezka(n.ChildNodes.Count.ToString());
                sciezki.Add(scie);
                foreach (XmlElement nutax in n.ChildNodes)
                {
                    if (nutax.Name == "nute")
                    {
                        try
                        {
                            nuta nu = new nuta(plik.Hz / funkcje.częstotliwość(short.Parse(nutax.Attributes.GetNamedItem("octave").Value, CultureInfo.InvariantCulture), float.Parse(nutax.Attributes.GetNamedItem("note").Value, CultureInfo.InvariantCulture)), (long)(float.Parse(nutax.Attributes.GetNamedItem("duration").Value, CultureInfo.InvariantCulture) * plik.Hz * 60 / tempo), (long)(float.Parse(nutax.Attributes.GetNamedItem("delay").Value, CultureInfo.InvariantCulture) * plik.Hz * 60 / tempo));
                            scie.nuty.Add(nu);
                        }
                        catch { }
                    }
                }
                foreach (string sound in n.Attributes.GetNamedItem("sound").Value.Split(' '))
                {

                    try
                    {
                       // ((sekwencer)moduły[sound]["<sekwencer"]).sciezkaa = scie;
                        scie.sekw = ((sekwencer)moduły[sound]["<sekwencer"]);
                        break;
                    }
                    catch { }
                }


            }

            foreach (Dictionary<string, moduł> z in moduły.Values)
            {
                var ins = new Instrument();
                foreach (moduł zz in z.Values)
                {
                    try
                    {
                        ins.Children.Add(zz.UI);
                    }
                    catch (System.ArgumentException e)
                    {

                    }
                }
                MainWindow.thi.pokaz.Children.Add(ins);
            }
        }
        public plik()
        {
            URL = null;
            xml = new XmlDocument();
            xml.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?><file></file>");
        }
        public void zapisz()
        {
            foreach (var x in moduły)
            {
                foreach (var y in x.Value)
                {
                    modułFunkcje.zapiszXML(y.Value.ustawienia, y.Value.XML);
                }
            }
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "Plik XML|*.xml|Plik Syntezatora Krawczyka|*.synkra";
            dialog.ShowDialog();
            if (dialog.FileName != "")
            {
                System.IO.StreamWriter zapis = new System.IO.StreamWriter(dialog.FileName);
                zapis.Write(xml.OuterXml);
                zapis.Close();
            }
        }
    }
}
