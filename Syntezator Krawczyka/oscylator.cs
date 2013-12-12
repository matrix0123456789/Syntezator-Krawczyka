using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Globalization;

namespace Syntezator_Krawczyka.Synteza
{
    public class oscylator : moduł
    {
        public UserControl UI
        {
            get { return _UI; }
        }
        UserControl _UI;
        public List<Typ> wejście { get; set; }
        public Typ[] wyjście
        {
            get { return _wyjście; }
        }
        Typ[] _wyjście;
        public Dictionary<string, string> ustawienia
        {
            get { return _ustawienia; }
        }
        Dictionary<string, string> _ustawienia;
        public XmlNode XML { get; set; }
        enum typFali : byte { sinusoidalna, trójkątna, prostokątka, piłokształtna }
        public oscylator()
        {
            wejście = new List<Typ>();
            _wyjście = new Typ[2];
            _wyjście[0] = new Typ();
            _wyjście[1] = new Typ();
            _ustawienia = new Dictionary<string, string>();
            _ustawienia.Add("typ", "trójkątna");
            _ustawienia.Add("ilefal", "1");
            _ustawienia.Add("gladkosc", "1");
            _ustawienia.Add("A", "0");
            _ustawienia.Add("D", "0");
            _ustawienia.Add("S", "0.2");
            _ustawienia.Add("R", "0");
            _UI = new oscylatorUI(this);
        }
        /// <summary>
        /// Zawiera wynik funkcji generujJedenPrzebieg zapisany w celu optymalizacji
        /// </summary>
        Dictionary<long, float[]> zapisanePojedyńczePrzebiegi = new Dictionary<long, float[]>();
        public static float[] generujJedenPrzebiegStatyczny(string typ, long ilePróbek, float gladkość)
        {

            float[] jedenPrzebieg;
            if (typ == "trójkątna")
                jedenPrzebieg = trójkątna((float)ilePróbek, ilePróbek);
            else if (typ == "prostokątna")
                jedenPrzebieg = prostokątna((float)ilePróbek, ilePróbek, gladkość);
            else if (typ == "sinusoidalna")
                jedenPrzebieg = sinusoidalna((float)ilePróbek, ilePróbek);
            else if (typ == "piłokształtna2x")
                jedenPrzebieg = piłokształtna2x((float)ilePróbek, ilePróbek, 1, gladkość);
            else if (typ == "piłokształtna")
                jedenPrzebieg = piłokształtna((float)ilePróbek, ilePróbek, 1, gladkość);
            else
                jedenPrzebieg = prostokątna((float)ilePróbek, ilePróbek, gladkość);
            //zapisanePojedyńczePrzebiegi.Add(typ + ilePróbek.ToString() + ' ' + gladkość.ToString(), jedenPrzebieg);
            return jedenPrzebieg;

        }
        public float[] generujJedenPrzebieg(string typ, long ilePróbek, float gladkość)
        {
            /*if (zapisanePojedyńczePrzebiegi.ContainsKey(ilePróbek))
            {
                return zapisanePojedyńczePrzebiegi[ilePróbek];
            }
            else
            {*/
            float[] jedenPrzebieg = generujJedenPrzebiegStatyczny(typ, ilePróbek, gladkość);
            // zapisanePojedyńczePrzebiegi.Add(ilePróbek, jedenPrzebieg);
            return jedenPrzebieg;
            // }
        }

        public long symuluj(long p)
        {
            return wyjście[0].DrógiModół.symuluj(p + (long)(float.Parse(_ustawienia["R"], CultureInfo.InvariantCulture) * plik.kHz));
        }
        public /*unsafe*/ void działaj(nuta input)
        {

            {
                nuta n = input;
                var A = float.Parse(_ustawienia["A"], CultureInfo.InvariantCulture);
                var D = float.Parse(_ustawienia["D"], CultureInfo.InvariantCulture);
                var S = float.Parse(_ustawienia["S"], CultureInfo.InvariantCulture);
                var R = float.Parse(_ustawienia["R"], CultureInfo.InvariantCulture);
                var gladkosc = float.Parse(_ustawienia["gladkosc"], CultureInfo.InvariantCulture);
                {
                    lock (n)
                    {
                        n.ilepróbek = Math.Floor(n.ilepróbek);
                        n.długość = (long)(Math.Floor(n.długość / n.ilepróbek) * n.ilepróbek);
                        if (wyjście[0].DrógiModół != null && (D != 0 || S != 0))
                        {
                            object[] wy = new object[2];
                            float[] jedenPrzebieg = generujJedenPrzebieg(_ustawienia["typ"], (long)Math.Floor(n.ilepróbek), gladkosc);
                            long długośćCała = (int)(Math.Floor((n.długość) / n.ilepróbek) * n.ilepróbek + R * plik.kHz);
                            if (n.długość + R * plik.kHz - n.generujOd > 0)
                            {
                                if (n.generujDo < 0)
                                    n.dane = new float[(int)(Math.Floor((n.długość) / n.ilepróbek) * n.ilepróbek + R * plik.kHz)];
                                else if (n.generujDo - n.generujOd < (int)(Math.Floor((n.długość) / n.ilepróbek) * n.ilepróbek + R * plik.kHz))
                                    n.dane = new float[n.generujDo - n.generujOd];
                                else
                                    n.dane = new float[(int)(Math.Floor((n.długość) / n.ilepróbek) * n.ilepróbek + R * plik.kHz)];


                            }
                            else
                                n.dane = new float[0];//koniec wykonywania
                            float aProcent, dProcent;
                            float rProcent = 1;
                            aProcent = 0;
                            var aMax = A * plik.kHz;
                            var dMax = D * plik.kHz;
                            var rMax = R * plik.kHz;
                            var s = S;
                            aProcent = 1;
                            var Max1 = długośćCała - n.generujOd - rMax;
                            var Max2 = aMax - (int)n.generujOd;
                            var Max3 = dMax - (int)n.generujOd;
                            var Acc1 = dMax * (1 - s);
                            for (int i = 0; i < n.dane.Length; i++)
                            {
                                if (Max1 > i)
                                    if (Max2 > i)
                                        aProcent = (i + (int)n.generujOd) / aMax;
                                    else
                                        aProcent = 1;
                                else
                                {
                                    rProcent = (długośćCała - i - n.generujOd) / rMax;
                                    //aProcent = 1;
                                }
                                if (Max3 > i)
                                {
                                    dProcent = s + (Max3 - i ) / Acc1;
                                }
                                else
                                    dProcent = s;
                                if (dProcent == 0)
                                    break;//sprawdzić, czi nie powoduje problemów

                                n.dane[i] = jedenPrzebieg[(i + (int)n.generujOd) % jedenPrzebieg.Length] * aProcent * rProcent * dProcent;

                            }
                            wyjście[0].DrógiModół.działaj(n);
                        }
                    }
                }
            }

        }
        public void działaj(nuta input, float[] jak)
        {
            nuta n = input;

            {
                lock (n)
                {
                    n.ilepróbek = Math.Floor(n.ilepróbek);
                    n.długość = (long)(Math.Floor(n.długość / n.ilepróbek) * n.ilepróbek);
                    if (wyjście[0].DrógiModół != null && (float.Parse(_ustawienia["D"], CultureInfo.InvariantCulture) != 0 || float.Parse(_ustawienia["S"], CultureInfo.InvariantCulture) != 0))
                    {
                        object[] wy = new object[2];
                        long długośćCała = (int)(Math.Floor((n.długość) / n.ilepróbek) * n.ilepróbek + float.Parse(_ustawienia["R"], CultureInfo.InvariantCulture) * plik.kHz);
                        if (n.długość + float.Parse(_ustawienia["R"], CultureInfo.InvariantCulture) * plik.kHz - n.generujOd > 0)
                        {
                            if (n.generujDo < 0)
                                n.dane = new float[(int)(Math.Floor((n.długość) / n.ilepróbek) * n.ilepróbek + float.Parse(_ustawienia["R"], CultureInfo.InvariantCulture) * plik.kHz)];
                            else if (n.generujDo - n.generujOd < (int)(Math.Floor((n.długość) / n.ilepróbek) * n.ilepróbek + float.Parse(_ustawienia["R"], CultureInfo.InvariantCulture) * plik.kHz))
                                n.dane = new float[n.generujDo - n.generujOd];
                            else
                                n.dane = new float[(int)(Math.Floor((n.długość) / n.ilepróbek) * n.ilepróbek + float.Parse(_ustawienia["R"], CultureInfo.InvariantCulture) * plik.kHz)];


                        }
                        else
                            n.dane = new float[0];//koniec wykonywania
                        float aProcent, dProcent;
                        float rProcent = 1;
                        aProcent = 0;
                        var aMax = float.Parse(_ustawienia["A"], CultureInfo.InvariantCulture) * plik.kHz;
                        var dMax = float.Parse(_ustawienia["D"], CultureInfo.InvariantCulture) * plik.kHz;
                        var rMax = float.Parse(_ustawienia["R"], CultureInfo.InvariantCulture) * plik.kHz;
                        var s = float.Parse(_ustawienia["S"], CultureInfo.InvariantCulture);
                        aProcent = 1;

                        var typ = _ustawienia["typ"];
                        typFali jakaFala = typFali.trójkątna;
                        if (typ == "trójkątna")
                            jakaFala = typFali.trójkątna;
                        else if (typ == "prostokątna")
                            jakaFala = typFali.prostokątka;
                        else if (typ == "sinusoidalna")
                            jakaFala = typFali.sinusoidalna;
                        else if (typ == "piłokształtna")
                            jakaFala = typFali.piłokształtna;
                        else
                            jakaFala = typFali.prostokątka;
                        var gladkosc = float.Parse(_ustawienia["gladkosc"], CultureInfo.InvariantCulture);
                        var niski = -2 * gladkosc;
                        var wysoki = 2f + niski;

                        float granica = (float)(n.ilepróbek * gladkosc);
                        float pozycja = 0;//przy podziale nuty zrobić zapamiętywanie
                        for (int i = 0; i < n.dane.Length; i++)
                        {
                            if (długośćCała - i - n.generujOd > rMax)
                                if (aMax > i + (int)n.generujOd)
                                    aProcent = (i + (int)n.generujOd) / aMax;
                                else
                                    aProcent = 1;
                            else
                            {
                                rProcent = (długośćCała - i - n.generujOd) / rMax;
                                //aProcent = 1;
                            }
                            if (dMax > i + (int)n.generujOd)
                            {
                                dProcent = s + (dMax - i - (int)n.generujOd) / dMax * (1 - s);
                            }
                            else
                                dProcent = s;
                            pozycja += jak[(i + (int)n.generujOd)%jak.Length] + 1;

                            if (jakaFala == typFali.sinusoidalna)
                                n.dane[i] = (float)Math.Sin(pozycja / n.ilepróbek * 2 * Math.PI) * (aProcent * rProcent * dProcent);
                            else if (jakaFala == typFali.trójkątna)
                            {
                                var z = (float)(pozycja % n.ilepróbek / n.ilepróbek);
                                if (z < 0.25)
                                    n.dane[i] = z * 4 * (aProcent * rProcent * dProcent);
                                else if (z < 0.75)
                                    n.dane[i] = (0.5f - z) * 4f * (aProcent * rProcent * dProcent);
                                else
                                    n.dane[i] = (1 - z) * -4 * (aProcent * rProcent * dProcent);
                            }
                            else if (jakaFala == typFali.prostokątka)
                            {

                                if (pozycja % n.ilepróbek < n.ilepróbek * gladkosc)
                                    n.dane[i] = wysoki * (aProcent * rProcent * dProcent);
                                else
                                    n.dane[i] = niski * (aProcent * rProcent * dProcent);

                            }
                            else if (jakaFala == typFali.piłokształtna)
                            {
                                float a = (float)(((n.ilepróbek / 3) + i) % (n.ilepróbek));
                                if (a < granica)
                                    n.dane[i] += ((a / granica) * 2 - 1);
                                else
                                    n.dane[i] += (float)((a - granica) / (n.ilepróbek - granica) * -2 + 1);
                            }

                        }
                        wyjście[0].DrógiModół.działaj(n);
                    }
                }
            }
        }
        public static float[] prostokątna(float ilepróbek, long długość, float gladkosc)
        {
            float[] ret = new float[długość];
            var wysoki = 2f - 2 * gladkosc;
            var niski = 0 - 2 * gladkosc;
            for (long i = 0; i < długość; i++)
            {
                if (i % ilepróbek < ilepróbek * gladkosc)
                    ret[i] = wysoki;
                else
                    ret[i] = niski;
            }
            return ret;
        }
        public static float[] trójkątna(float ilepróbek, long długość)
        {
            float[] ret = new float[długość];
            for (long i = 0; i < długość; i++)
            {
                var z = i % ilepróbek / ilepróbek;
                if (z < 0.25)
                    ret[i] = z * 4;
                else if (z < 0.75)
                    ret[i] = (0.5f - z) * 4f;
                else
                    ret[i] = (1 - z) * -4;
                /*if (i % ilepróbek < ilepróbek / 2)
                    ret[i] = i % (ilepróbek / 2) / ilepróbek * 4 - 1;
                else
                    ret[i] = i % (ilepróbek / 2) / ilepróbek * -4 + 1;*/
            }
            /*for (long i = długość; i < długość + 10000; i++)
            {
                ret[i]=ret[długość - 1] * -((double)(i - długość - 10000) / 10000);
            }*/
            return ret;
        }
        public static float[] piłokształtna(float ilepróbek, long długość, short ilenut, float gladkosc)
        {
            float[] ret = new float[długość];
            
                float granica = ilepróbek * gladkosc;
                for (long i = 0; i < długość; i++)
                {
                    float a = ((ilepróbek  / 3) + i) % (ilepróbek);
                    if (a < granica)
                        ret[i] += ((a / granica) * 2 - 1);
                    else
                        ret[i] += ((a - granica) / (ilepróbek - granica) * -2 + 1);
                }
           
            return ret;
        }
        public static float[] piłokształtna2x(float ilepróbek, long długość, short ilenut, float gladkosc)
        {
            float[] ret = new float[długość];
            for (short i2 = 0; i2 < ilenut; i2++)
            {
                for (long i = 0; i < długość; i++)
                {
                    if (i % ilepróbek < ilepróbek * gladkosc)
                        ret[i] += ((ilepróbek / ilenut * i2 / 25) + i) % (ilepróbek * (1 - gladkosc)) / ilepróbek * 2 - 1;
                    else
                        ret[i] += ((ilepróbek / ilenut * i2 / 25) + i) % (ilepróbek * gladkosc) / ilepróbek * 2 - 1;
                }
            }
            return ret;
        }
        public static float[] sinusoidalna(double ilepróbek, long długość)
        {
            float[] ret = new float[długość];
            for (long i = 0; i < długość; i++)
            {
                ret[i] = (float)Math.Sin(i / ilepróbek * 2 * Math.PI);
            }
            return ret;
        }
    }
}
