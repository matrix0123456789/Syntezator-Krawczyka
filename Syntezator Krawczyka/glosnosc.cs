using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Globalization;

namespace Syntezator_Krawczyka.Synteza
{
    public class glosnosc : filtr
    {
        public UserControl UI
        {
            get { return _UI; }
        }
        public void akt() { }
        public long symuluj(long p)
        {
            return wyjście[0].DrógiModół.symuluj(p);
        }
        public XmlNode XML { get; set; }
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
        public glosnosc()
        {
            _wyjście = new Typ[1];
            _wyjście[0] = new Typ();
            wejście = new List<Typ>();
            _ustawienia = new Dictionary<string, string>();
            _ustawienia.Add("głośność", "1.0");
            _ustawienia.Add("ucinanie", "false");
            _ustawienia.Add("ucinanieWartość", "1.0");
            _UI = new glosnoscUI(this);
        }
        public void działaj(nuta input)
        {
            var głośność = float.Parse(ustawienia["głośność"], CultureInfo.InvariantCulture);
           // bool ucinanie;
            if (ustawienia["ucinanie"] == "true")//uważać, czy nie jest po pogłosie
            {
                var ucinanieWartość = float.Parse(ustawienia["ucinanieWartość"], CultureInfo.InvariantCulture);
                var ucinanieWartośćMinus=-ucinanieWartość;
                var ucinaniePomnożone = ucinanieWartość * głośność;
                var ucinaniePomnożoneMinus = -ucinaniePomnożone;
                for (var i = 0; i < input.dane.Length; i++)
                {
                    if (ucinaniePomnożone <= input.dane[i])
                        input.dane[i] = ucinanieWartość;
                    else if (ucinaniePomnożoneMinus >= input.dane[i])
                        input.dane[i] = ucinanieWartośćMinus;
                    else
                        input.dane[i] = input.dane[i] * głośność;
                }
            }
            else if(głośność!=1)
            {
                if (wyjście[0].DrógiModół.GetType() == typeof(granie))
                {
                    input.głośność = input.głośność * głośność;
                    /*for (var i = 0; i < input.dane.Length; i++)
                    {
                        input.dane[i] = input.dane[i];
                    }*/
                }
                else
                //input.głośność = input.głośność * 0.99f;
                {
                    for (var i = 0; i < input.dane.Length; i++)
                    {
                        input.dane[i] = input.dane[i] * głośność;
                    }
                }
            }
            wyjście[0].DrógiModół.działaj(input);
        }
        public nuta działaj(nuta input, float[] jak)
        {
            var głośność = float.Parse(ustawienia["głośność"], CultureInfo.InvariantCulture);
            var iJak = input.generujOd;
            for (var i = 0; i < input.dane.Length; i++)
            {
                input.dane[i] = input.dane[i] * głośność*jak[(iJak+i)%jak.Length];
               
            }

            return input;
        }
    }
}