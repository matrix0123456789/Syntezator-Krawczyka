using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Globalization;

namespace Syntezator_Krawczyka.Synteza
{
    public class lfo:moduł
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
        Dictionary<string, string> _ustawienia;
        public lfo()
        {
            _wejście = new Typ[2];
            _wejście[0] = new Typ();
            _wejście[1] = new Typ();
            _wyjście = new Typ[2];
            _wyjście[0] = new Typ();
            _wyjście[1] = new Typ();
            _ustawienia = new Dictionary<string, string>();
            _ustawienia.Add("moc", "0");
            _ustawienia.Add("czestotliwosc", "0");
            _ustawienia.Add("typ", "sinusoidalna");
            _ustawienia.Add("gladkosc", "0");
            _UI = new lfoUI(this);
        }
        public void działaj(nuta input)
        {
            if (float.Parse(_ustawienia["moc"], CultureInfo.InvariantCulture) == 0 || float.Parse(_ustawienia["czestotliwosc"], CultureInfo.InvariantCulture) == 0)
                {
                    if (wyjście[0].DrógiModół != null)
                    {
                        wyjście[0].DrógiModół.działaj(input);
                    }
                }
                else
            {
                {

                    nuta n = input;
                    var moc = float.Parse(_ustawienia["moc"], CultureInfo.InvariantCulture);
                    var jedenPrzebieg = oscylator.generujJedenPrzebieg(ustawienia["typ"], (long)(1 / float.Parse(_ustawienia["czestotliwosc"], CultureInfo.InvariantCulture) * plik.Hz), float.Parse(_ustawienia["gladkosc"], CultureInfo.InvariantCulture));
                    float[] jak = new float[n.dane.Length];
                    for (int i = 0; i < n.dane.Length; i++)
                    {
                        var miejsce = (i+n.generujOd) % jedenPrzebieg.Length;
                        //jak[i] = n.dane[i] * (1 + (-jedenPrzebieg[miejsce] * 0.5f - 0.5f) * moc);
                        jak[i] = 1 + (-jedenPrzebieg[miejsce] * 0.5f - 0.5f) * moc;
                    }
                    if (wyjście[0].DrógiModół != null && wyjście[1].DrógiModół != null)
                    {
                        wyjście[0].DrógiModół.działaj((wyjście[1].DrógiModół as filtr).działaj(input, jak));
                    }
                }
            }
           

        }
    }
}
