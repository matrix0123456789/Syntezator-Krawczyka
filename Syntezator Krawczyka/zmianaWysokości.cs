﻿using System;
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
            get
            {
                if (_UI == null)
                    _UI = new zmianaWysokościUI(this);
                return _UI;
            }
        }
        public void akt()
        {
             oktawy = float.Parse(_ustawienia["oktawy"], CultureInfo.InvariantCulture);
             tony = float.Parse(_ustawienia["tony"], CultureInfo.InvariantCulture) + (float.Parse(_ustawienia["czestotliwosc"], CultureInfo.InvariantCulture) / 2);
        }
        public long symuluj(long p)
        {
            return wyjście[0].DrógiModół.symuluj(p);
        }
        UserControl _UI;
        public List<Typ> wejście { get; set; }
        public Typ[] wyjście
        {
            get { return _wyjście; }set { _wyjście = value; }
        }
        Typ[] _wyjście;
        public Dictionary<string, string> ustawienia
        {
            get { return _ustawienia; }
        }
        Dictionary<string, string> _ustawienia;
        private float oktawy;
        private float tony;
        public zmianaWysokości()
        {
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
            o.ilepróbek = o.ilepróbek / Math.Pow(2, oktawy + (tony / 6));

            wyjście[0].DrógiModół.działaj(o);
        }
    }
}
