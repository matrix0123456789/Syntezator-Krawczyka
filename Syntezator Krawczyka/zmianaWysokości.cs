using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;

namespace Syntezator_Krawczyka.Synteza
{
    public class zmianaWysokości : moduł
    {
        public XmlNode XML { get; set; }
        public UserControl UI
        {
            get { return _UI; }
        }
        public void akt() { }
        public long symuluj(long p)
        {
            return wyjście[0].DrógiModół.symuluj(p);
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
        public zmianaWysokości()
        {
            _UI = new zmianaWysokościUI(this);
            wejście = new List<Typ>();
            _wyjście = new Typ[1];
            _wyjście[0] = new Typ();
            _ustawienia = new Dictionary<string, string>();
            _ustawienia.Add("oktawy", "0");
            _ustawienia.Add("tony", "0");
            _ustawienia.Add("czestotliwosc", "0");
        }
        public void działaj(nuta o)
        {
            var oktawy = float.Parse(_ustawienia["oktawy"], CultureInfo.InvariantCulture);
            var tony = float.Parse(_ustawienia["tony"], CultureInfo.InvariantCulture)+(float.Parse(_ustawienia["czestotliwosc"], CultureInfo.InvariantCulture)/2);
            o.ilepróbek=o.ilepróbek/Math.Pow(2, oktawy + (tony / 6));

            wyjście[0].DrógiModół.działaj(o);
        }
    }
}
