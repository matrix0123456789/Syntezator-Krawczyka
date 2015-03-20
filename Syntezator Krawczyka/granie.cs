using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Xml;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;

namespace Syntezator_Krawczyka.Synteza
{
    public class granie : moduł
    {
        public static DateTime wygenerowano;
        public static bool[] generować = { true };
        public XmlNode XML { get; set; }
        static public int o = 4410;
        static System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
        public static Dictionary<long, gra> grają = new Dictionary<long, gra>();
        static bool jest = false;
        static public float[,] wynik = null;
        static public int graniePrzy = 0, granieMax = 0, granieI = 0;
        static public bool graniePlay = false;
        static public nuta[] granieNuty;

        public void akt() { }
        public UserControl UI
        {
            get
            {
                if (_UI == null)
                    _UI = new GranieUI(this);
                return _UI;
            }
        }
        UserControl _UI;
        public long symuluj(long wej)
        {
            return wej;
        }
        public List<Typ> wejście { get; set; }
        public Typ[] wyjście
        {
            get { return _wyjście; }
            set { _wyjście = value; }
        }
        Typ[] _wyjście;
        public Dictionary<string, string> ustawienia
        {
            get { return _ustawienia; }
        }
        public static bool grateraz = false;
        Dictionary<string, string> _ustawienia;
        static float[] pustaTablica = new float[0];
        public static void grajcale(bool graj)
        {
            grateraz = false;
            long oz = 0;
            gra[] zz;
            float[,] fala;
            if (wynik == null)
            {
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
                fala = new float[2, oz];
                for (int x = 0; x < zz.Length; x++)
                {

                    if (zz[x].nuta.głośność == 0 || (zz[x].nuta.balans0 == 0 && zz[x].nuta.balans1 == 0))
                    {

                        zz[x].nuta.dane = zz[x].dźwięk = null;
                        continue;
                    }
                    long i = 0;
                    if (zz[x].zagrano < 0 && -zz[x].zagrano < oz)
                        i = -zz[x].zagrano;
                    else if (zz[x].zagrano < 0)
                        i = o;
                    long max;
                    if (oz < zz[x].dźwięk.LongLength - zz[x].zagrano)
                        max = oz;
                    else
                        max = zz[x].dźwięk.LongLength - zz[x].zagrano;
                    if (zz[x].nuta.głośność == 1 && zz[x].nuta.balans0 == 1 && zz[x].nuta.balans1 == 1)
                    {
                        for (; i < max; i++)
                        {


                            fala[0, i] += zz[x].dźwięk[i + zz[x].zagrano];
                            fala[1, i] += zz[x].dźwięk[i + zz[x].zagrano];

                        }
                    }
                    else
                    {
                        var mn0 = zz[x].nuta.głośność * zz[x].nuta.balans0;
                        var mn1 = zz[x].nuta.głośność * zz[x].nuta.balans1;
                        for (; i < oz && i + zz[x].zagrano < zz[x].dźwięk.LongLength; i++)
                        {


                            fala[0, i] += zz[x].dźwięk[i + zz[x].zagrano] * mn0;
                            fala[1, i] += zz[x].dźwięk[i + zz[x].zagrano] * mn1;

                        }
                    }
                    zz[x].zagrano += oz;
                    if (zz[x].zagrano >= zz[x].dźwięk.Length)
                        zz[x].nuta.dane = zz[x].dźwięk = null;

                }
            }
            else
            {
                fala = wynik;
                wynik = null;
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
            grają.Clear();
            if (graj)
                funkcje.graj(fala);
            else
            {
                while (PlikDoZapisu == null)
                {
                    Thread.Sleep(50);
                }
                if (PlikDoZapisu != "")
                    funkcje.zapisz(fala, PlikDoZapisu);
            }
            można = true;
            //}
            //});
        }
        static DateTime data;
        public static int liczbaGenerowanych = 0;
        public static int liczbaGenerowanychMax = 0;
        public static object liczbaGenerowanychBlokada = new object();
        public static object obLock = new object();
        public static bool można = true;
        public granie()
        {
            graniestart();
            wejście = new List<Typ>();
            _wyjście = new Typ[0];
            _ustawienia = new Dictionary<string, string>();
            _ustawienia.Add("głośność", "1.0");
        }
        static public void graniestart()
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



                t = new Thread(() =>
                {
                    while (true)
                    {
                        while (((DateTime.Now - data).TotalSeconds * plik.Hz) < 480)
                            Thread.Sleep(1);
                        grajRazCale();
                    }
                });
                t.Priority = ThreadPriority.Normal;
                t.Start();


            }
        }
        public static object grajRazLock = new object();
        public static bool teraz = false;
        public static void grajRazCale()
        {
            //Thread.Sleep(10);
            //System.Threading.Thread.Sleep(100);
            //object[] act = (object[])action;
            //Dictionary<string, string> ustawienia = (Dictionary<string, string>)act[0];
            //List<gra> grają = (List<gra>)act[1];
            //if (!grateraz)
            for (byte i = 0; liczbaGenerowanych > 0 || teraz && i < 10; i++)
            {
                if (i == 10)
                    return;
                Thread.Sleep(1);
            }
            var dataTeraz = DateTime.Now;
            o = (int)((dataTeraz - data).TotalSeconds * plik.Hz);

            if (o > 10000)
                o = 10000;
            data = dataTeraz;
            if (można && liczbaGenerowanych == 0 && (klawiaturaKomputera.wszytskieNuty.Count > 0))
                lock (grają)
                {
                    teraz = true;
                    lock (grajRazLock)
                    {
                        nuta[] wszystNuty;
                        lock (klawiaturaKomputera.wszytskieNuty)
                        {
                            wszystNuty = klawiaturaKomputera.wszytskieNuty.ToArray();
                        }
                        liczbaGenerowanych += wszystNuty.Length + 1;
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
                                wszystNuty[i].generujDo = grają[wszystNuty[i].id].zagrano + o + 256;
                            }
                            else
                                wszystNuty[i].generujDo = o + 256;
                            wszystNuty[i].grajDo = o + 256;
                            wszystNuty[i].ilepróbek = wszystNuty[i].ilepróbekNaStarcie;
                            System.Threading.ThreadPool.QueueUserWorkItem((Action) =>
                            {
                                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                                (Action as nuta).sekw.działaj((Action as nuta));
                                lock (zmianaLiczGenLock) { liczbaGenerowanych--; }
                                if (liczbaGenerowanych == 0)

                                    grajRaz();
                            }, wszystNuty[i]);
                        }
                        /*if (graniePlay)
                        {
                            graniePrzy += o;
                            while (granieNuty.Length > granieI && granieNuty[granieI].opuznienie <= graniePrzy+2*o)
                            {
                                granieNuty[granieI].opuznienie -= graniePrzy;
                                lock (zmianaLiczGenLock) { liczbaGenerowanych++; }
                                System.Threading.ThreadPool.QueueUserWorkItem((Action) =>
                                {
                                    if ((Action as nuta).sekw != null)
                                    {
                                        (Action as nuta).sekw.działaj((Action as nuta));
                                        lock (zmianaLiczGenLock) { liczbaGenerowanych--; }
                                        if (liczbaGenerowanych == 0)

                                            grajRaz();
                                    }
                                }, granieNuty[granieI]);
                                granieI++;
                            }
                        }*/
                        lock (zmianaLiczGenLock) { liczbaGenerowanych--; }
                        if (liczbaGenerowanych == 0)

                            grajRaz();
                    }
                }
            if (liczbaGenerowanych == 0 || grają.Count > 0)

                grajRaz();
        }
        public static void grajRaz()
        {

            if (można && liczbaGenerowanych == 0)
            {
                float[,] fala;
                lock (grają)
                {
                    lock (klawiaturaKomputera.wszytskieNuty)
                    {
                        var i2 = 0;
                        while (i2 < klawiaturaKomputera.wszytskieNuty.Count)
                        {
                            if (klawiaturaKomputera.wszytskieNuty[i2].dane != null)
                            {
                                if (klawiaturaKomputera.wszytskieNuty[i2].dane.Length == 0)
                                {
                                    klawiaturaKomputera.wszytskieNuty.RemoveAt(i2);
                                }
                                else
                                    i2++;
                            }
                            else if (klawiaturaKomputera.wszytskieNuty[i2].sekw.GetType() == typeof(InstrumentMidi))
                            {
                                klawiaturaKomputera.wszytskieNuty.RemoveAt(i2);
                            }
                            else
                                i2++;
                        }
                    }




                    {
                    dodatkowy:
                        try
                        {

                            var falaLength = o;
                            fala = new float[2, falaLength];
                            /*if (MainWindow.gpgpu)
                            {
                                    

                                    fixed (float* _fala = &fala[0])
                                    {
                                        square_array(_fala, fala.Length);
                                    }
                                    
                            }
                            else*/
                            {

                                gra[] zz = grają.Values.ToArray();
                                var liczIle = 0;
                                for (int x = 0; x < zz.Length; x++)
                                {

                                    if (zz[x].nuta.dane==null||zz[x].zagrano > zz[x].nuta.dane.Length + zz[x].nuta.generujOd)
                                    {
                                        zz[x].nuta.dane = null;
                                        zz[x].dźwięk = null;
                                        grają.Remove(zz[x].nuta.id);
                                    }

                                    else
                                    {
                                        liczIle++;

                                        if (zz[x].nuta.głośność == 0 || (zz[x].nuta.balans0 == 0 && zz[x].nuta.balans1 == 0))
                                        {

                                            zz[x].zagrano += o;
                                            continue;
                                        }
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
                                        if (o < opt2 && o < falaLength)
                                            opt3 = o;
                                        else if (opt2 < falaLength)
                                            opt3 = opt2;
                                        else
                                            opt3 = falaLength;
                                        if (zz[x].nuta.głośność == 1 && zz[x].nuta.balans0 == 1 && zz[x].nuta.balans1 == 1)
                                        {
                                            if (i < -opt1)
                                                i = -opt1;
                                            for (; i < opt3; i++)
                                            {

                                                {
                                                    fala[0, i] += zz[x].dźwięk[i + opt1];
                                                    fala[1, i] += zz[x].dźwięk[i + opt1];
                                                }

                                            }
                                        }
                                        else
                                        {
                                            if (i < -opt1)
                                                i = -opt1;
                                            var mn0 = zz[x].nuta.głośność * zz[x].nuta.balans0;
                                            var mn1 = zz[x].nuta.głośność * zz[x].nuta.balans1;
                                            for (; i < opt3; i++)
                                            {

                                                {
                                                    fala[0, i] += zz[x].dźwięk[i + opt1] * mn0;
                                                    fala[1, i] += zz[x].dźwięk[i + opt1] * mn1;
                                                }

                                            }
                                        }
                                        zz[x].zagrano += o;
                                    }


                                }
                            }
                        }
                        catch { fala = new float[0, 0]; }
                        //System.Threading.ThreadPool.QueueUserWorkItem((Action) =>
                        //{
                        //});

                    }
                }
                try
                {
                    if (grają.Count > 0)
                        if (funkcje.graj(fala))
                        { }// goto dodatkowy;
                }
                catch (FormatException a) { funkcje.graj(fala); }
                // grajRazCale();
            }

            teraz = false;
        }
        public static bool liveGraj()
        {
            bool czyJeszczeRaz = false; if (graniePlay)
            {
                lock (wynik)
                {
                    long wygenerowanoDo = wynik.Length / 2;
                    foreach (var x in granieNuty)
                    {
                        if (!x.czyGotowe && x.sekw != null && wygenerowanoDo > x.opuznienie)
                        {
                            wygenerowanoDo = x.opuznienie;
                            czyJeszczeRaz = true;
                        }

                    }
                    var dl = wygenerowanoDo - graniePrzy;


                    if (dl > 0)
                    {
                        if ((Statyczne.bufor.BufferLength - Statyczne.bufor.BufferedBytes) / 4 < dl)
                        {
                            czyJeszczeRaz = true;
                            dl = (Statyczne.bufor.BufferLength - Statyczne.bufor.BufferedBytes) / 4;
                        }
                        var falaT = new float[2, dl];
                        for (var i = 0; i < dl; i++)
                        {
                            falaT[0, i] = wynik[0, i + graniePrzy];
                            falaT[1, i] = wynik[1, i + graniePrzy];
                        }
                        try
                        {
                            if (funkcje.graj(falaT))
                            { }// goto dodatkowy;
                        }
                        catch (FormatException a) { System.Windows.MessageBox.Show("błąd"); }
                        graniePrzy += (int)dl;
                    }
                }
                /*if(czyJeszczeRaz)
                {
                    //Thread.Sleep(100);
                    jedenTimer = new Timer((o) => { liveGraj(); }, null, 100, 0);
                
                }*/

            }
            return czyJeszczeRaz;
        }
        static Timer jedenTimer;
        public static void Działaj(nuta input)
        {
            if (wynik == null)
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
            else
            {


                long i = input.opuznienie + input.generujOd;
                var opt1 = -input.generujOd - input.opuznienie;

                var opt3 = input.dane.Length - opt1;
                try
                {
                    if (input.głośność == 1)

                        if (input.balans0 == 1 && input.balans1 == 1)
                            for (; i < opt3; i++)
                            {
                                wynik[0, i] += input.dane[i + opt1];
                                wynik[1, i] += input.dane[i + opt1];
                            }
                        else
                            for (; i < opt3; i++)
                            {
                                wynik[0, i] += input.dane[i + opt1] * input.balans0;
                                wynik[1, i] += input.dane[i + opt1] * input.balans1;
                            }
                    else
                    {
                        var mn0 = input.głośność * input.balans0;
                        var mn1 = input.głośność * input.balans1;
                        for (; i < opt3; i++)
                        {
                            wynik[0, i] += input.dane[i + opt1] * mn0;
                            wynik[1, i] += input.dane[i + opt1] * mn1;
                        }
                    }
                }
                catch (IndexOutOfRangeException) { }

                input.dane = null;
            }
            //try { funkcje.graj((double[])input[0], double.Parse(_ustawienia["głośność"])); }
            //catch { }
        }
        public unsafe static void Działaj(float* input, long length, long inputopuznienie, long inputgenerujOd)
        {
            if (wynik == null)
                lock (grają)
                {//tynczasowo
                    nuta nut = new nuta();
                    nut.dane = new float[length];
                    for (int i = 0; i < length;i++ )
                    {
                        nut.dane[i]=input[i*2];
                    }
                        grają.Add(nuta.nowyid, new gra(nut)); 
                   /* if (grają.ContainsKey(input.id))
                    {
                        grają[input.id].dźwięk = (input).dane;
                        grają[input.id].nuta = input;
                    }
                    else
                        grają.Add(input.id, new gra(input));*/
                }
            else
            {


                long i = inputopuznienie + inputgenerujOd;
                var opt1 = -inputgenerujOd - inputopuznienie;

                var opt3 = length - opt1;
                try
                {

                    for (; i < opt3; i++)
                    {
                        wynik[0, i] += input[(i + opt1) * 2];
                        wynik[1, i] += input[(i + opt1) * 2 + 1];
                    }
                }
                catch (IndexOutOfRangeException) { }
                Marshal.FreeHGlobal((IntPtr)input);
            }
            //try { funkcje.graj((double[])input[0], double.Parse(_ustawienia["głośność"])); }
            //catch { }
        }
        public static System.Threading.Thread t;
        short ileNutMusiByć = 0;
        static short analizujIleNutMusiByć(moduł wej)
        {
            if (typeof(sekwencer) == wej.GetType())
            {
                return 1;
            }

            else
            {
                var rozdzielacze = new List<int>();
                short ret = 0;
                for (int i = 0; i < wej.wejście.Count; i++)
                {
                    if (wej.wejście[i].DrógiModół.GetType() == typeof(flanger))
                        for (var x = 0; x < wej.wejście[x].DrógiModół.wejście.Count; x++)
                        {
                            if (wej.wejście[i].DrógiModół.wejście[x].DrógiModół.GetType() == typeof(rozdzielacz))
                            {
                                if (!rozdzielacze.Contains(wej.wejście[i].DrógiModół.wejście[x].DrógiModół.GetHashCode()))
                                    rozdzielacze.Add(wej.wejście[i].DrógiModół.wejście[x].DrógiModół.GetHashCode());



                            }
                            else
                                ret += analizujIleNutMusiByć(wej.wejście[i].DrógiModół.wejście[x].DrógiModół);

                        }
                    else
                        ret += analizujIleNutMusiByć(wej.wejście[i].DrógiModół);
                }
                ret += (short)rozdzielacze.Count;
                if (ret > 2)
                { }
                return ret;
            }
        }
        public void analizujIleNutMusiByć()
        {
            //ileNutMusiByć = analizujIleNutMusiByć(this);
        }
        Dictionary<long, Object[]> grupowane = new Dictionary<long, object[]>();
        public void działaj(nuta n)
        {
            //if (ileNutMusiByć == 0||n.czyPogłos||!MainWindow.czyGC)
            Działaj(n);
            /*else
            {
                lock (grupowane)
                {
                    if (!grupowane.ContainsKey(n.idOryginalne))
                    {
                        object[] tablica = new object[ileNutMusiByć + 1];
                        tablica[0] = 1;
                        tablica[1] = n;
                        grupowane.Add(n.idOryginalne, tablica);
                    }
                    else
                    {
                        (grupowane[n.idOryginalne][0]) = ((int)grupowane[n.idOryginalne][0]) + 1;
                        grupowane[n.idOryginalne][((int)grupowane[n.idOryginalne][0])] = n;
                    }
                    if (((int)grupowane[n.idOryginalne][0]) == grupowane[n.idOryginalne].Length - 1)
                    {
                        long opuznienie = 0;
                        long długość = 0;
                        for (int i = 1; i < grupowane[n.idOryginalne].Length; i++)
                        {
                            if ((grupowane[n.idOryginalne][i] as nuta).opuznienie + (grupowane[n.idOryginalne][i] as nuta).generujOd < opuznienie)
                                opuznienie = (grupowane[n.idOryginalne][i] as nuta).opuznienie + (grupowane[n.idOryginalne][i] as nuta).generujOd;
                            if ((grupowane[n.idOryginalne][i] as nuta).opuznienie + (grupowane[n.idOryginalne][i] as nuta).dane.Length > długość)
                                długość = (grupowane[n.idOryginalne][i] as nuta).opuznienie + (grupowane[n.idOryginalne][i] as nuta).dane.Length;
                        }
                        var dane = new float[długość - opuznienie];
                        for (int i = 1; i < grupowane[n.idOryginalne].Length; i++)
                        {
                            long opuznienieAkt = -opuznienie + (grupowane[n.idOryginalne][i] as nuta).opuznienie;
                            for (long x = (grupowane[n.idOryginalne][i] as nuta).generujOd; x < (grupowane[n.idOryginalne][i] as nuta).dane.Length; x++)
                            {
                                dane[x + opuznienieAkt] += (grupowane[n.idOryginalne][i] as nuta).dane[x] * (grupowane[n.idOryginalne][i] as nuta).głośność;
                            }
                        }
                        var nutaKoniec = (grupowane[n.idOryginalne][1] as nuta);
                        nutaKoniec.id = nutaKoniec.idOryginalne;
                        nutaKoniec.dane = dane;
                        nutaKoniec.generujOd = 0;
                        nutaKoniec.opuznienie = opuznienie;
                        Działaj(nutaKoniec);

                        grupowane.Remove(n.idOryginalne);
                    }
                }
            }*/
        }

        public static object zmianaLiczGenLock = new object();

        public static string PlikDoZapisu = null;


        public static byte bity = 16;
        public static System.Net.Sockets.TcpListener LANSocket;
    }
}
