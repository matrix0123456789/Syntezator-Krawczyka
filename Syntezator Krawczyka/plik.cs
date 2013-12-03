using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Syntezator_Krawczyka.Synteza;
using System.Globalization;
using System.Windows;
namespace Syntezator_Krawczyka
{
    public class plik
    {
        public string URL;
        public XmlDocument xml;
        public Dictionary<string, sound> moduły = new Dictionary<string, sound>();
        public List<sciezka> sciezki = new List<sciezka>();
        public Dictionary<string, sciezka> scieżkiZId = new Dictionary<string, sciezka>();
        public static float tempo = 120;
        public static float kHz = 48f;
        public static float Hz = kHz * 1000;
        public plik(string a)
        {
            if (a != "")
            {
                Statyczne.otwartyplik.URL = URL = a;
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
                this.URL = URL = a;
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
            if(Statyczne.otwartyplik!=null)
            {
                moduły.Clear();
                sciezki.Clear();
                scieżkiZId.Clear();
                for (var i = 0; i < MainWindow.thi.pokaz.Children.Count;i++ )
                {
                    if (MainWindow.thi.pokaz.Children[i].GetType() != typeof(KlawiaturaKomputeraUI))
                    {
                        MainWindow.thi.pokaz.Children.Remove((UIElement)MainWindow.thi.pokaz.Children[i]);
                    }
                }
            }
            try
            {
                var granieLista = new List<granie>();
                dekoduj(xml.GetElementsByTagName("sound"));
                var doSkopiowania = new List<object[]>();
                foreach (XmlNode n in xml.GetElementsByTagName("track"))
                {
                    sciezka scie = new sciezka(n.ChildNodes.Count.ToString());
                    sciezki.Add(scie);
                    if (n.Attributes.GetNamedItem("id") != null)
                        scieżkiZId.Add(n.Attributes.GetNamedItem("id").Value, scie);
                    if (n.Attributes.GetNamedItem("copy") != null)
                    {
                        var ob = new object[2];
                        ob[0] = scie;
                        ob[1] = n;
                        doSkopiowania.Add(ob);
                    }
                    foreach (XmlNode nutax in n.ChildNodes)
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
                    if (n.Attributes.GetNamedItem("sound") != null)
                        foreach (string sound in n.Attributes.GetNamedItem("sound").Value.Split(' '))
                        {

                            try
                            {
                                // ((sekwencer)moduły[sound]["<sekwencer"]).sciezkaa = scie;
                                scie.sekw = (moduły[sound].sekw);
                                break;
                            }
                            catch { }
                        }


                }
                foreach (var n in doSkopiowania)
                {
                    if ((n[1] as XmlElement).Attributes.GetNamedItem("copy") != null)
                        if (scieżkiZId.ContainsKey((n[1] as XmlElement).Attributes.GetNamedItem("copy").Value))
                        {
                            int delay;
                            if ((n[1] as XmlElement).Attributes.GetNamedItem("delay") != null)
                                delay = (int)(float.Parse((n[1] as XmlElement).Attributes.GetNamedItem("delay").Value, CultureInfo.InvariantCulture) * plik.Hz * 60 / tempo);
                            else
                                delay = 0;

                            foreach (var x in scieżkiZId[(n[1] as XmlElement).Attributes.GetNamedItem("copy").Value].nuty)
                            {
                                var xx = x.Clone() as nuta;
                                xx.id = nuta.nowyid;
                                xx.opuznienie += delay;
                                (n[0] as sciezka).nuty.Add(xx);
                            }
                        }
                }
                foreach (sound z in moduły.Values)
                {
                    z.UI = new Instrument(z.nazwa);
                    foreach (moduł zz in z.Values)
                    {
                        try
                        {
                            z.UI.Children.Add(zz.UI);
                        }
                        catch (System.ArgumentException e)
                        {

                        }
                    }
                    MainWindow.thi.pokaz.Children.Add(z.UI);
                }
                for (var i = 0; i < granieLista.Count; i++)
                    granieLista[i].analizujIleNutMusiByć();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString(), "Błąd przy przetwarzaniu pliku", MessageBoxButton.OK, MessageBoxImage.Error);
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
        public void dekoduj(XmlNodeList a)
        {
             foreach (XmlNode n in a)
                {
                    if (n.Attributes.GetNamedItem("type").Value == "syntezator-krawczyka")
                    {
                        moduły.Add(n.Attributes.GetNamedItem("id").Value, new sound(n.Attributes.GetNamedItem("id").Value));
                        foreach (XmlNode nn in n.ChildNodes)
                        {
                            if (nn.Name == "module")
                            {
                                switch (nn.Attributes.GetNamedItem("type").Value)
                                {
                                    case "sekwencer":

                                        moduły[n.Attributes.GetNamedItem("id").Value].sekw = new sekwencer();
                                        moduły[n.Attributes.GetNamedItem("id").Value].Add(nn.Attributes.GetNamedItem("id").Value, moduły[n.Attributes.GetNamedItem("id").Value].sekw as sekwencer);
                                        // moduły[n.Attributes.GetNamedItem("id").Value]["<sekwencer"] = moduły[n.Attributes.GetNamedItem("id").Value][nn.Attributes.GetNamedItem("id").Value];
                                        break;
                                    case "player":


                                        moduły[n.Attributes.GetNamedItem("id").Value].Add(nn.Attributes.GetNamedItem("id").Value, new player());
                                        moduły[n.Attributes.GetNamedItem("id").Value]["<player"] = moduły[n.Attributes.GetNamedItem("id").Value][nn.Attributes.GetNamedItem("id").Value];
                                        break;
                                    case "granie":
                                        var gr = new granie();

                                        moduły[n.Attributes.GetNamedItem("id").Value].Add(nn.Attributes.GetNamedItem("id").Value, gr);
                                       // granieLista.Add(gr);
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
                                    case "generatorObwiedniFiltru":


                                        moduły[n.Attributes.GetNamedItem("id").Value].Add(nn.Attributes.GetNamedItem("id").Value, new generatorObwiedniFiltru());
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
                    else if (n.Attributes.GetNamedItem("type").Value == "samples")
                    {
                        moduły.Add(n.Attributes.GetNamedItem("id").Value, new sound(n.Attributes.GetNamedItem("id").Value));
                        moduły[n.Attributes.GetNamedItem("id").Value].sekw = new sampler();
                        if (n.Attributes.GetNamedItem("volume") != null)
                            (moduły[n.Attributes.GetNamedItem("id").Value].sekw as sampler).głośność = float.Parse(n.Attributes.GetNamedItem("volume").Value, CultureInfo.InvariantCulture);
                        foreach (XmlNode nn in n.ChildNodes)
                        {
                            if (nn.Name == "sample")
                            {
                                var sam = new sample();
                                if (nn.Attributes.GetNamedItem("file") != null)
                                    sam.plik = nn.Attributes.GetNamedItem("file").Value;
                                if (nn.Attributes.GetNamedItem("note") != null)
                                    sam.note = float.Parse(nn.Attributes.GetNamedItem("note").Value, CultureInfo.InvariantCulture);
                                if (nn.Attributes.GetNamedItem("accept") != null)
                                    sam.accept = float.Parse(nn.Attributes.GetNamedItem("accept").Value, CultureInfo.InvariantCulture);
                                (moduły[n.Attributes.GetNamedItem("id").Value].sekw as sampler).sample.Add(sam);

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

                                if (nn.Attributes.GetNamedItem("output") != null)
                                {
                                    string[] exp = nn.Attributes.GetNamedItem("output").Value.Split(' ');
                                    for (int az = 0; az < exp.Length; az++)
                                    {
                                        try
                                        {
                                            moduły[n.Attributes.GetNamedItem("id").Value][nn.Attributes.GetNamedItem("id").Value].wyjście[az].DrógiModół = moduły[n.Attributes.GetNamedItem("id").Value][exp[az]];
                                            moduły[n.Attributes.GetNamedItem("id").Value][exp[az]].wejście.Add(new Typ(moduły[n.Attributes.GetNamedItem("id").Value][nn.Attributes.GetNamedItem("id").Value]));
                                        }
                                        catch { }
                                    }
                                }

                            }
                        }
                    }

                }
        }
    }
}
