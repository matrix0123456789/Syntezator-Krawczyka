using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Xml;
using System.Globalization;

namespace Syntezator_Krawczyka.Synteza
{
    public class granie : moduł
    {
        public XmlNode XML { get; set; }
        static public int o = 4410;
        static System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
        public static Dictionary<long, gra> grają = new Dictionary<long, gra>();
        static bool jest = false;
        public UserControl UI
        {
            get { return _UI; }
        }
        UserControl _UI;
        public Typ[] wejście
        {
            get { return _wejście; }
        }
        Typ[] _wejście;
        public Typ[] wyjście
        {
            get { return _wyjście; }
        }
        Typ[] _wyjście;
        public Dictionary<string, string> ustawienia
        {
            get { return _ustawienia; }
        }
        public static bool grateraz = false;
        Dictionary<string, string> _ustawienia;
        public static void grajcale(bool graj)
        {
            grateraz = false;
            long oz = 0;
            gra[] zz;
            lock (grają)
            {
                zz = grają.Values.ToArray();
            } foreach (gra dospr in zz)
            {
                if (dospr != null)
                {
                    dospr.zagrano = -dospr.nuta.opuznienie;
                    if (dospr.dźwięk.Length - dospr.zagrano > oz)
                        oz = dospr.dźwięk.Length - dospr.zagrano;
                }
            }
            double[] fala = new double[oz];
            for (int x = 0; x < grają.Count; x++)
            {
                
                    long i = 0;
                    if (zz[x].zagrano < 0 && -zz[x].zagrano < oz)
                        i = -zz[x].zagrano;
                    else if (zz[x].zagrano < 0)
                        i = o;
                    for (; i < oz && i + zz[x].zagrano < zz[x].dźwięk.LongLength; i++)
                    {

                        if (zz[x].zagrano + i >= 0)
                        {
                            fala[i] += zz[x].dźwięk[i + zz[x].zagrano];
                        }
                    }
                    zz[x].zagrano += oz;
                
            }

            //System.Threading.ThreadPool.QueueUserWorkItem((Action) =>
            //{
            /*try
            {
                if (graj)
                    funkcje.graj(fala, double.Parse(ustawienia["głośność"]));
                else
                    funkcje.zapisz(fala, double.Parse(ustawienia["głośność"]), "C:\\Users\\Mateusz\\Desktop\\xml\\a.wav");
            }
            catch (FormatException a)
            {*/
            if (graj)
                funkcje.graj(fala, 1);
            else
            {
                var dialog = new SaveFileDialog();
                dialog.Filter = "Plik muzyczny|*.wav;*.wave";
                dialog.ShowDialog();
                if (dialog.FileName != "")
                    funkcje.zapisz(fala, 1, dialog.FileName);
            }
            można = true;
            //}
            //});
        }
        DateTime data;
        public static int liczbaGenerowanych = 0;
        public static int liczbaGenerowanychMax = 1;
        public static object liczbaGenerowanychBlokada = new object();
        public static object obLock = new object();
        public static bool można = true;
        public granie()
        {
            if (!jest)
            {
                // MainWindow.WasapiWyjście.Stop();
                //new System.Threading.Timer((object ozzz) => { MainWindow.WasapiWyjście.Play(); }, null,100,0);
                //MainWindow.bufor.AddSamples(new byte[10000], 0, 10000);
                jest = true;
                //object[] wejs = new object[2];
                //wejs[0] = _ustawienia;
                //wejs[1] = grają;

                data = DateTime.Now;
                t = new System.Threading.Timer((action) =>
                {
                    //System.Threading.Thread.Sleep(100);
                    //object[] act = (object[])action;
                    //Dictionary<string, string> ustawienia = (Dictionary<string, string>)act[0];
                    //List<gra> grają = (List<gra>)act[1];
                    //if (!grateraz)
                    var dataTeraz = DateTime.Now;
                    o = (int)((dataTeraz - data).TotalSeconds * plik.Hz);
                    
                    if (o > 10000)
                        o = 10000;
                    data = dataTeraz;
                    if (można && liczbaGenerowanych == 0)
                    lock (grają)
                    {
                        lock (klawiaturaKomputera.wszytskieNuty)
                        {
                            var wszystNuty = klawiaturaKomputera.wszytskieNuty.ToArray();
                            for (var i = 0; i < wszystNuty.Length; i++)
                            {
                                if (grają.ContainsKey(wszystNuty[i].id))
                                {
                                    if (grają[wszystNuty[i].id].zagrano < 0)
                                        wszystNuty[i].generujOd = 0;
                                    else if (grają[wszystNuty[i].id].zagrano < 256)
                                    {
                                        wszystNuty[i].generujOd = 0;
                                        wszystNuty[i].grajOd = grają[wszystNuty[i].id].zagrano;
                                    }
                                    else
                                    {
                                        wszystNuty[i].generujOd = grają[wszystNuty[i].id].zagrano - 256;
                                        wszystNuty[i].grajOd = 256;
                                    }
                                    wszystNuty[i].generujDo = grają[wszystNuty[i].id].zagrano + o+256;
                                }
                                else
                                    wszystNuty[i].generujDo = o + 256;
                                wszystNuty[i].grajDo = o+256;
                                wszystNuty[i].sekw.wyjście[0].DrógiModół.działaj(wszystNuty[i]);
                            }
                            var i2 = 0;
                            while (i2 < klawiaturaKomputera.wszytskieNuty.Count)
                            {
                                if (klawiaturaKomputera.wszytskieNuty[i2].dane.Length == 0)
                                {
                                    klawiaturaKomputera.wszytskieNuty.RemoveAt(i2);
                                }
                                else
                                    i2++;
                            }
                        }


                        
                        
                            {
                            dodatkowy:
                                double[] fala = new double[o];

                                gra[] zz = grają.Values.ToArray();
                                var liczIle = 0;
                                for (int x = 0; x < zz.Length; x++)
                                {
                                    
                                        if (zz[x].zagrano > zz[x].nuta.dane.Length + zz[x].nuta.generujOd)
                                            grają.Remove(zz[x].nuta.id);

                                        else
                                        {
                                            liczIle++;
                                            long i = 0;
                                            if (zz[x].zagrano < 0 && -zz[x].zagrano < o)
                                                i = -zz[x].zagrano;
                                            else if (zz[x].zagrano < 0)
                                                i = o;
                                            //else
                                            // i = zz[x].zagrano - zz[x].nuta.generujOd;
                                            var opt1 = zz[x].zagrano - zz[x].nuta.generujOd;
                                            var opt2 = zz[x].dźwięk.LongLength - opt1;
                                            long opt3;
                                            if (o < opt2 && o < fala.Length)
                                                opt3 = o;
                                            else if (opt2 < fala.LongLength)
                                                opt3 = opt2;
                                            else
                                                opt3 = fala.LongLength;
                                            for (; i < opt3; i++)
                                            {

                                                if (i + opt1 >= 0)
                                                    {
                                                        fala[i] += zz[x].dźwięk[i + opt1];
                                                    }
                                                
                                            }
                                            zz[x].zagrano += o;
                                        }
                                    

                                }

                                //System.Threading.ThreadPool.QueueUserWorkItem((Action) =>
                                //{
                                try
                                {
                                    if (grają.Count > 0)
                                        if (funkcje.graj(fala, double.Parse(ustawienia["głośność"], CultureInfo.InvariantCulture)))
                                            goto dodatkowy;
                                }
                                catch (FormatException a) { funkcje.graj(fala, 1); }
                                //});

                            }
                    }
                }, data, 0, (int)(10));
            }
            _UI = new GranieUI(this);
            _wejście = new Typ[1];
            _wyjście = new Typ[0];
            _wejście[0] = new Typ();
            _ustawienia = new Dictionary<string, string>();
            _ustawienia.Add("głośność", "1.0");
        }
        public void działaj(nuta input)
        {

            lock (grają)
            {
                if (grają.ContainsKey(input.id))
                {
                    grają[input.id].dźwięk = (input).dane;
                    grają[input.id].nuta = input;
                }
                else
                    grają.Add(input.id, new gra(input));
            }
            //try { funkcje.graj((double[])input[0], double.Parse(_ustawienia["głośność"])); }
            //catch { }
        }
        public static System.Threading.Timer t;
    }
}
