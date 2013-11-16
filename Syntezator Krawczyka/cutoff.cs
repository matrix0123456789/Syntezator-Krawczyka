﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Globalization;

namespace Syntezator_Krawczyka.Synteza
{
    public class cutoff : filtr
    {

        public UserControl UI
        {
            get { return _UI; }
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
            get { return _wyjście; }
        }
        Typ[] _wyjście;
        public Dictionary<string, string> ustawienia
        {
            get { return _ustawienia; }
        }
        Dictionary<string, string> _ustawienia;
        public cutoff()
        {
            wejście = new List<Typ>();
            _wyjście = new Typ[1];
            _wyjście[0] = new Typ();
            _ustawienia = new Dictionary<string, string>();
            _ustawienia.Add("moc", "1");
            _ustawienia.Add("gladkosc", "0");
            _ustawienia.Add("przesuniecie", (0).ToString());
            _UI = new cutoffUI(this);
        }
        public void działaj(nuta input)
        {

            if (input.dane.Length > 0)
            {
                var moc = float.Parse(_ustawienia["moc"], CultureInfo.InvariantCulture);
                var gladkosc = float.Parse(_ustawienia["gladkosc"], CultureInfo.InvariantCulture);
                input.dane[0] = moc * input.dane[0];
                if (gladkosc == 0f)
                {
                    for (var i = 1; i < input.dane.Length; i++)
                    {
                        input.dane[i] = (input.dane[i] - input.dane[i - 1]) * moc + input.dane[i - 1];
                    }

                    if (wyjście[0].DrógiModół != null)
                    {
                        wyjście[0].DrógiModół.działaj(input);
                    }
                }
                else
                {
                    var ostatniaZmiana = 0f;
                    var i = 1;
                    if (i < input.dane.Length)
                    {
                        input.dane[1] = (input.dane[1] - input.dane[0]) * moc + input.dane[0];
                    }
                    i=2;
                    for (; i < input.dane.Length; i++)
                    {
                        input.dane[i] = ((input.dane[i] - input.dane[i - 1]) * moc) * (1 - gladkosc) + (input.dane[i - 1]-input.dane[i - 2]) * gladkosc + input.dane[i - 1];
                    }

                    if (wyjście[0].DrógiModół != null)
                    {
                        wyjście[0].DrógiModół.działaj(input);
                    }
                }
            }
            
        }
        public nuta działaj(nuta input, float[] jak)
        {

            if(jak.Length>0&&input.dane.Length>0)
            {
                var moc = float.Parse(_ustawienia["moc"], CultureInfo.InvariantCulture);
            input.dane[0] = moc * jak[0] * input.dane[0];
            var iJak = input.generujOd % jak.Length;
            for (var i = 1; i < input.dane.Length; i++)
            {
                input.dane[i] = (input.dane[i] - input.dane[i - 1]) * (moc+jak[iJak%jak.Length]) + input.dane[i - 1];
                iJak++;
            }
            }
            return input;
        }
    }
}