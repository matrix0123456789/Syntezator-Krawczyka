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
            get { return _UI; }
        }
        public XmlNode XML { get; set; }
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
        static Dictionary<double, double> sinusy = new Dictionary<double, double>();
        static public double sin(double wej)
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
        }
        Dictionary<string, string> _ustawienia;
        public flanger()
        {
            _wejście = new Typ[1];
            _wejście[0] = new Typ();
            _wyjście = new Typ[1];
            _wyjście[0] = new Typ();
            _ustawienia = new Dictionary<string, string>();
            _ustawienia.Add("czestotliwosc", (0).ToString());
            _ustawienia.Add("przesuniecie", (0).ToString());
            _UI = new flangerUI(this);
        }
        public float[] działaj(nuta input, float[] dane)
        {
            var przesunięciea = float.Parse(_ustawienia["przesuniecie"], CultureInfo.InvariantCulture);
            var czestotliwosc = float.Parse(_ustawienia["czestotliwosc"], CultureInfo.InvariantCulture);

            if (przesunięciea == 0 || czestotliwosc == 0)
                return dane;
            else
            {
                var przesunięcie = przesunięciea * plik.kHz;
                var ileNaCykl = 1 / czestotliwosc * plik.Hz;
                var losIGenerujOd = input.los + input.generujOd;
                float z;
                for (int i = 0; i < dane.Length; i++)
                {
                    var zx = (i + losIGenerujOd) % ileNaCykl / ileNaCykl;
                    if (zx < 0.25)
                        z = zx * 4 * przesunięcie;
                    else if (zx < 0.75)
                        z = (0.5f - zx) * 4f * przesunięcie;
                    else
                        z = (1f - zx) * -4f * przesunięcie;
                    var x = i + (int)Math.Floor(z);
                    /*if (input.generujOd > 0 && x < 0)
                    { }*/
                    
                    var proporcje = z - (float)Math.Floor(z);
                    if (input.dane.Length > x + 1 && x >= 0)
                        dane[i] = (input.dane[x] * (1 - proporcje) + input.dane[x + 1] * proporcje) / 2 + dane[i];
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
                float z;
                for (int i = 0; i < input.dane.Length; i++)
                {
                    var zx = (i + losIGenerujOd) % ileNaCykl / ileNaCykl;
                    if (zx < 0.25)
                        z = zx * 4 * przesunięcie;
                    else if (zx < 0.75)
                        z = (0.5f - zx) * 4f * przesunięcie;
                    else
                        z = (1f - zx) * -4f * przesunięcie;
                    var x = i + (int)Math.Floor(z);
                   /* if (input.generujOd > 0 && x < 0)
                    { }*/
                    var proporcje = z - (float)Math.Floor(z);
                    if (input.dane.Length > x + 1 && x >= 0)
                        noweDane[i] = (input.dane[x] * (1 - proporcje) + input.dane[x + 1] * proporcje) / 2;
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
    }
}
