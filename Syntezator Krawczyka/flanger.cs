using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Globalization;

namespace Syntezator_Krawczyka.Synteza
{
    public class flanger : moduł
    {
        public UserControl UI
        {
            get
            {
                if (_UI == null)

                    _UI = new flangerUI(this);
                return _UI;
            }
        }
        float czestotliwosc, przesunięciea;
        public void akt()
        {
            przesunięciea = float.Parse(_ustawienia["przesuniecie"], CultureInfo.InvariantCulture);
            czestotliwosc = float.Parse(_ustawienia["czestotliwosc"], CultureInfo.InvariantCulture);
        }
        public long symuluj(long p)
        {
            return wyjście[0].DrógiModół.symuluj(p);
        }
        public XmlNode XML { get; set; }
        UserControl _UI;
        public List<Typ> wejście { get; set; }
        public Typ[] wyjście
        {
            get;
            set;
        }
        public Dictionary<string, string> ustawienia
        {
            get { return _ustawienia; }
        }
        static Dictionary<double, double> sinusy = new Dictionary<double, double>();
        /*static public double sin(double wej)
        {
            try
            {
                return sinusy[wej];
            }
            catch
            {
                sinusy.Add(wej, Math.Sin(wej * Math.PI));
                return sinusy[wej];
            }
        }*/
        Dictionary<string, string> _ustawienia;
        public flanger()
        {
            wejście = new List<Typ>();
            wyjście = new Typ[1];
            wyjście[0] = new Typ();
            _ustawienia = new Dictionary<string, string>();
            _ustawienia.Add("czestotliwosc", (0).ToString());
            _ustawienia.Add("przesuniecie", (0).ToString());
            akt();
        }
        public float[] działaj(nuta input, float[] dane)
        {

            if (przesunięciea == 0 || czestotliwosc == 0)
            {
                for (int i = 0; i < dane.Length; i++)
                    dane[i] += input.dane[i];
                    return dane;
            }
            else
            {
                var przesunięcie = przesunięciea * plik.kHz;
                var ileNaCykl = 1 / czestotliwosc * plik.Hz / Math.PI / 2;
                var losIGenerujOd = input.los + input.generujOd;
                double z;
                for (int i = 0; i < dane.Length; i++)
                {

                    z = przesunięcie * Math.Sin((i + losIGenerujOd) / ileNaCykl);
                    var x = i + (int)Math.Floor(z);


                    var proporcje = z - Math.Floor(z);
                    if (input.dane.Length > x + 1 && x >= 0)
                        dane[i] = ((float)(input.dane[x] * (1 - proporcje) + input.dane[x + 1] * proporcje) / 2) + dane[i];
                    /* else { } if (i > 2000)
                         if (dane[i] == 0 && dane[i - 1] == 0)
                         { }*/


                }
                return dane;
            }
        }
        public void działaj(nuta input)
        {
            var przesunięciea = float.Parse(_ustawienia["przesuniecie"], CultureInfo.InvariantCulture);
            var czestotliwosc = float.Parse(_ustawienia["czestotliwosc"], CultureInfo.InvariantCulture);

            if (przesunięciea == 0 || czestotliwosc == 0)
            {
                if (wyjście[0].DrógiModół != null)
                {
                    wyjście[0].DrógiModół.działaj(input);
                }
            }
            else
            {
                var przesunięcie = przesunięciea * plik.kHz;
                var ileNaCykl = 1 / czestotliwosc * plik.Hz;
                float[] noweDane = new float[input.dane.Length];
                var losIGenerujOd = input.los + input.generujOd;
                double z;
                for (int i = 0; i < input.dane.Length; i++)
                {
                    var zx = (i + losIGenerujOd) / ileNaCykl;

                    z = przesunięcie * Math.Sin((i + losIGenerujOd) / ileNaCykl);
                    var x = i + (int)Math.Floor(z);


                    var proporcje = z - Math.Floor(z);
                    if (input.dane.Length > x + 1 && x >= 0)
                        noweDane[i] = ((float)(input.dane[x] * (1 - proporcje) + input.dane[x + 1] * proporcje) / 2);
                    //noweDane[i] = z / 500;
                    /*else { }
                    if(i>0)
                        if (noweDane[i] == 0 && noweDane[i - 1] == 0)
                        { }*/
                }
                input.dane = noweDane;
                if (wyjście[0].DrógiModół != null)
                {
                    wyjście[0].DrógiModół.działaj(input);
                }
            }
        }
        public List<int> gpgpuGeneruj()
        {
            if (wyjście[0].DrógiModół == null)
            {
                return null;
            }
            var dane = new List<int>();
            dane.Add((int)ModułyEnum.flanger);
            dane.Add(3);
            dane.Add(czestotliwosc.GetHashCode());
            dane.Add(przesunięciea.GetHashCode());
            return dane;
        }
    }
}
