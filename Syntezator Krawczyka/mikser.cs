using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Globalization;

namespace Syntezator_Krawczyka.Synteza
{
    public class mikser : moduł
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
        public mikser()
        {
            _wyjście = new Typ[1];
            _wyjście[0] = new Typ();
            _wejście = new Typ[8];
            _wejście[0] = new Typ();
            _wejście[1] = new Typ();
            _wejście[2] = new Typ();
            _wejście[3] = new Typ();
            _wejście[4] = new Typ();
            _wejście[5] = new Typ();
            _wejście[6] = new Typ();
            _wejście[7] = new Typ();
            _ustawienia = new Dictionary<string, string>();
            _UI = new UserControl();
        }
        public void działaj(nuta input)
        {
            wyjście[0].DrógiModół.działaj(input);
        }
    }
}