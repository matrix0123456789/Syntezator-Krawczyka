using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Syntezator_Krawczyka.Synteza;
using System.Globalization;
using System.Windows;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Win32;
namespace Syntezator_Krawczyka
{
    public class plik
    {
        public static string URLStatyczne;
        public string URL;
        public XmlDocument xml;
        public Dictionary<string, sound> moduły = new Dictionary<string, sound>();
        public List<sciezka> sciezki = new List<sciezka>();
        public Dictionary<string, sciezka> scieżkiZId = new Dictionary<string, sciezka>();
        public static float tempo = 120;
        public static float kHz = 48f;
        int pusteID = 0;
        public static float Hz = kHz * 1000;
        public List<DrumJeden> DrumLista = new List<DrumJeden>();
        public Dictionary<string, FalaNiestandardowa> fale = new Dictionary<string, FalaNiestandardowa>();
        private List<jedenSample> sameSample = new List<jedenSample>();
        public plik(string a)
        {
            if (a != "")
            {
                URL = a;
                URLStatyczne = a;
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
                this.URL = URLStatyczne = a;
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
        public void dekoduj()
        {
            Statyczne.otwartyplik = this;
            if (Statyczne.otwartyplik != null)
            {
                moduły.Clear();
                sciezki.Clear();
                scieżkiZId.Clear();
                MainWindow.dispat.BeginInvoke(DispatcherPriority.Send, (ThreadStart)delegate()
                    {
                        for (var i = MainWindow.thi.pokaz.Children.Count - 1; i >= 0; i--)
                        {
                            if (MainWindow.thi.pokaz.Children[i].GetType() != typeof(KlawiaturaKomputeraUI))
                            {
                                MainWindow.thi.pokaz.Children.Remove((UIElement)MainWindow.thi.pokaz.Children[i]);
                            }
                        }
                    });
            }
            MainWindow.ileScierzekWyswietla = 0;
            lock (Statyczne.otwartyplik)
                try
                {
                    var listaWave = xml.GetElementsByTagName("wave");
                    for (var i = 0; i < listaWave.Count; i++)
                    {
                        if (listaWave.Item(i).Attributes.GetNamedItem("type").Value == "skladoweharmoniczne" && !fale.ContainsKey(listaWave.Item(i).Attributes.GetNamedItem("name").Value))
                            fale.Add(listaWave.Item(i).Attributes.GetNamedItem("name").Value, new SkładoweHarmoniczne(listaWave.Item(i)));
                    }
                    var granieLista = new List<granie>();
                    dekoduj(xml.GetElementsByTagName("sound"));
                    var doSkopiowania = new List<object[]>();
                    foreach (XmlNode n in xml.GetElementsByTagName("track"))
                    {
                        bool kopia = false;
                        string id;
                        if (n.Attributes.GetNamedItem("copy") != null)
                        {
                            kopia = true;
                            id = n.Attributes.GetNamedItem("copy").Value;
                        }
                        else if (n.Attributes.GetNamedItem("id") == null)
                        {
                            id = "track" + (pusteID++);
                        }
                        else
                            id = n.Attributes.GetNamedItem("id").Value;
                        sciezka scie = new sciezka(id, n, kopia);
                        sciezki.Add(scie);
                        if (n.Attributes.GetNamedItem("delay") != null)
                            scie.delay = (int)(float.Parse(n.Attributes.GetNamedItem("delay").Value, CultureInfo.InvariantCulture) * plik.Hz * 60 / tempo);
                        if (kopia)
                        {
                            var ob = new object[2];
                            ob[0] = scie;
                            ob[1] = n;
                            doSkopiowania.Add(ob);
                        }
                        else
                            scieżkiZId.Add(id, scie);
                        foreach (XmlNode nutax in n.ChildNodes)
                        {
                            if (nutax.Name == "nute")
                            {
                                try
                                {
                                    nuta nu = new nuta(plik.Hz / funkcje.częstotliwość(short.Parse(nutax.Attributes.GetNamedItem("octave").Value, CultureInfo.InvariantCulture), float.Parse(nutax.Attributes.GetNamedItem("note").Value, CultureInfo.InvariantCulture)), (long)(float.Parse(nutax.Attributes.GetNamedItem("duration").Value, CultureInfo.InvariantCulture) * plik.Hz * 60 / tempo), (long)(float.Parse(nutax.Attributes.GetNamedItem("delay").Value, CultureInfo.InvariantCulture) * plik.Hz * 60 / tempo) + scie.delay);
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
                                (n[0] as sciezka).kopia = true;
                                long delay;
                                if ((n[1] as XmlElement).Attributes.GetNamedItem("delay") != null)
                                    delay = (long)(float.Parse((n[1] as XmlElement).Attributes.GetNamedItem("delay").Value, CultureInfo.InvariantCulture) * plik.Hz * 60 / tempo) - scieżkiZId[(n[1] as XmlElement).Attributes.GetNamedItem("copy").Value].delay;
                                else
                                    delay = -scieżkiZId[(n[1] as XmlElement).Attributes.GetNamedItem("copy").Value].delay;

                                (n[0] as sciezka).oryginał = scieżkiZId[(n[1] as XmlElement).Attributes.GetNamedItem("copy").Value];
                                foreach (var x in scieżkiZId[(n[1] as XmlElement).Attributes.GetNamedItem("copy").Value].nuty)
                                {
                                    var xx = x.Clone() as nuta;
                                    xx.id = nuta.nowyid;
                                    xx.opuznienie += delay;
                                    (n[0] as sciezka).nuty.Add(xx);
                                }
                            }
                    }
                    MainWindow.dispat.BeginInvoke(DispatcherPriority.Send, (ThreadStart)delegate()
                    {
                        foreach (sound z in moduły.Values)
                        {

                            z.UI = new Instrument(z.nazwa, z);
                            if (z.sekw.GetType() == typeof(InstrumentMidi))
                            {
                                z.UI.wewnętrzny.Children.Add((z.sekw as InstrumentMidi).UI);
                            }
                            else
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
                            if (!MainWindow.thi.pokaz.Children.Contains(z.UI))
                            {

                                MainWindow.thi.pokaz.Children.Add(z.UI);
                            }
                        }
                    });
                    for (var i = 0; i < granieLista.Count; i++)
                        granieLista[i].analizujIleNutMusiByć();




                    foreach (XmlNode n in xml.GetElementsByTagName("drum"))
                    {
                        var dr = new DrumJeden(n);
                        DrumLista.Add(dr);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Błąd przy przetwarzaniu pliku", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            MainWindow.ileScierzekWyswietla = 0;
        }


        public plik()
        {
            URL = null;
            xml = new XmlDocument();
            xml.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?><file></file>");
        }
        /// <summary>
        /// Wykorzystywane w matodzie zapisz()
        /// </summary>
        void uaktualnij()
        {
            foreach (var x in moduły)
            {
                foreach (var y in x.Value)
                {
                    modułFunkcje.zapiszXML(y.Value.ustawienia, y.Value.XML);
                }
            }
            var listaWave = xml.GetElementsByTagName("wave");
            for (var i = listaWave.Count - 1; i >= 0; i--)
            {
                listaWave.Item(i).ParentNode.RemoveChild(listaWave.Item(i));
            }
            foreach (var x in fale)
            {
                xml.DocumentElement.AppendChild(x.Value.xml);
            }
        }
        public void zapisz()
        {
            uaktualnij();
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "Plik Syntezatora Krawczyka|*.synkra";
            dialog.ShowDialog();
            if (dialog.FileName != "")
            {
                System.IO.StreamWriter zapis = new System.IO.StreamWriter(dialog.FileName);
                zapis.Write(xml.OuterXml);
                zapis.Close();
            }
        }
        public void zapisz(string path)
        {
            uaktualnij();

            System.IO.StreamWriter zapis = new System.IO.StreamWriter(path);
            zapis.Write(xml.OuterXml);
            zapis.Close();

        }
        public byte[] zapiszDoZmiennej()
        {
            uaktualnij();
            return System.Text.Encoding.UTF8.GetBytes(xml.OuterXml);
        }
        public void dekoduj(XmlNodeList a)
        {
            foreach (XmlNode n in a)
            {
                dekoduj1(n);

            }
            foreach (XmlNode n in xml.GetElementsByTagName("sound"))
            {
                dekoduj2(n);

            }
        }
        public void dekoduj(XmlNode a)
        {
            dekoduj1(a);

            dekoduj2(a);


        }
        void dekoduj1(XmlNode n)
        {
            if (n.Attributes.GetNamedItem("type").Value == "syntezator-krawczyka")
            {
                moduły.Add(n.Attributes.GetNamedItem("id").Value, new sound(n.Attributes.GetNamedItem("id").Value, n));
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
                        moduły[n.Attributes.GetNamedItem("id").Value][nn.Attributes.GetNamedItem("id").Value].akt();
                    }
                }
            }
            else if (n.Attributes.GetNamedItem("type").Value == "samples")
            {
                moduły.Add(n.Attributes.GetNamedItem("id").Value, new sound(n.Attributes.GetNamedItem("id").Value, n));
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
            else if (n.Attributes.GetNamedItem("type").Value == "midi")
            {
                moduły.Add(n.Attributes.GetNamedItem("id").Value, new sound(n.Attributes.GetNamedItem("id").Value, n));
                moduły[n.Attributes.GetNamedItem("id").Value].sekw = new InstrumentMidi();
                if (n.Attributes.GetNamedItem("volume") != null)
                    (moduły[n.Attributes.GetNamedItem("id").Value].sekw as sampler).głośność = float.Parse(n.Attributes.GetNamedItem("volume").Value, CultureInfo.InvariantCulture);

            }
        }
        void dekoduj2(XmlNode n)
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

        internal void nowaScierzka()
        {
            var scierzkaXML = Statyczne.otwartyplik.xml.CreateElement("track");
            var atrybut1 = Statyczne.otwartyplik.xml.CreateAttribute("id");
            string id;
            do
            {
                id = atrybut1.Value = "track" + (pusteID++);
            } while (scieżkiZId.ContainsKey(id));
            scierzkaXML.Attributes.SetNamedItem(atrybut1);
            var atrybut2 = Statyczne.otwartyplik.xml.CreateAttribute("delay");
            atrybut2.Value = "0";
            scierzkaXML.Attributes.SetNamedItem(atrybut2);
            Statyczne.otwartyplik.xml.DocumentElement.AppendChild(scierzkaXML);

            sciezka scie = new sciezka(id, scierzkaXML, false);
            sciezki.Add(scie);
            scieżkiZId.Add(id, scie);
        }
        internal void duplikujScierzke(sciezka org)
        {
            var scierzkaXML = Statyczne.otwartyplik.xml.CreateElement("track");
            var atrybut1 = Statyczne.otwartyplik.xml.CreateAttribute("copy");
            var id = atrybut1.Value = org.nazwa;
            scierzkaXML.Attributes.SetNamedItem(atrybut1);
            var atrybut2 = Statyczne.otwartyplik.xml.CreateAttribute("delay");
            atrybut2.Value = "0";
            scierzkaXML.Attributes.SetNamedItem(atrybut2);
            Statyczne.otwartyplik.xml.DocumentElement.AppendChild(scierzkaXML);

            sciezka scie = new sciezka(id, scierzkaXML, true);
            scie.oryginał = org;
            sciezki.Add(scie);
        }
        internal void grajStart()
        {
            granie.graniePrzy = 0;



            granie.generować[0] = false;
            granie.generować = new bool[1];
            granie.generować[0] = true;
            granie.liczbaGenerowanychMax = granie.liczbaGenerowanych = 0;
            granie.można = false;
            granie.grają.Clear();
            long długość = 0;
            foreach (var x in Statyczne.otwartyplik.sciezki)
            {
                if (x.sekw != null)
                {
                    long długośćStart = 0;
                    for (var i = 0; i < x.nuty.Count; i++)
                    {

                        if (długośćStart < x.nuty[i].opuznienie + x.nuty[i].długość)
                            długośćStart = x.nuty[i].opuznienie + x.nuty[i].długość;

                    }

                    long długośćTeraz = x.sekw.symuluj(długośćStart);
                    if (długośćTeraz > długość)
                        długość = długośćTeraz;
                }
            }
            granie.PlikDoZapisu = null;
            granie.granieMax = (int)długość;
            granie.wynik = new float[2, długość];
            List<nuta> lista = new List<nuta>();
            foreach (var x in Statyczne.otwartyplik.sciezki)
            {
                foreach (var nuta in x.nuty)
                {
                    nuta.sekw = x.sekw;
                    lista.Add(nuta);
                }
            }
            lista.Sort(Syntezator_Krawczyka.nuta.sortuj);
            granie.granieNuty = lista.ToArray();
            granie.graniePlay = true;
            granie.liczbaGenerowanych += granie.granieNuty.Length;
            granie.liczbaGenerowanychMax += granie.granieNuty.Length;
            foreach (var x in granie.granieNuty)
            {
                lock (granie.grają)
                {

                    var tabl = (nuta)x.Clone();
                    tabl.grajDo = long.MaxValue;
                    System.Threading.ThreadPool.QueueUserWorkItem((o) =>
                    {

                        if (((bool[])o)[0] && x.sekw != null)
                        {
                            x.sekw.działaj(tabl);
                            x.czyGotowe = true;
                            // granie.liveGraj();
                        }

                        lock (granie.liczbaGenerowanychBlokada)
                        {
                            granie.liczbaGenerowanych--;
                            //if (!granie.można && granie.liczbaGenerowanych == 0)

                            //granie.grajcale(false);
                        }
                    }, granie.generować);
                    var watek = new Thread(() => { var gen = granie.generować; while (granie.liveGraj() && gen[0]) { Thread.Sleep(1000); } });
                    watek.Start();

                }
            }
        }

        public void generuj()
        {
            try
            {
                granie.liczbaGenerowanychMax = granie.liczbaGenerowanych = 0;
                granie.można = false;
                granie.grają.Clear();
                long długość = 0;
                foreach (var x in Statyczne.otwartyplik.sciezki)
                {
                    if (x.sekw != null)
                    {
                        long długośćStart = 0;
                        for (var i = 0; i < x.nuty.Count; i++)
                        {

                            if (długośćStart < x.nuty[i].opuznienie + x.nuty[i].długość)
                                długośćStart = x.nuty[i].opuznienie + x.nuty[i].długość;

                        }

                        long długośćTeraz = x.sekw.symuluj(długośćStart);
                        if (długośćTeraz > długość)
                            długość = długośćTeraz;
                    }
                }
                granie.PlikDoZapisu = null;
                granie.wynik = new float[2, długość];
                foreach (var x in Statyczne.otwartyplik.sciezki)
                {
                    x.działaj();
                    //akt(null);
                }
                foreach (var x in Statyczne.otwartyplik.sameSample)
                {
                    x.działaj();
                
                }
                var dialog = new SaveFileDialog();
                dialog.Filter = "Plik muzyczny|*.wav;*.wave";
                dialog.ShowDialog();
                granie.PlikDoZapisu = dialog.FileName;
            }
            catch (Exception e1) { MessageBox.Show("Błąd przy zapisie dźwięku", e1.ToString()); }
        }


    }
}
