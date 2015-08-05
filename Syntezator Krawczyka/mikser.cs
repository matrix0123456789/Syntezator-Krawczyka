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
            get { 
                if(_UI ==null)
                    _UI = new UserControl();
                return _UI; }
        }
        int ilewej = 0;
        public void akt()
        {
            ilewej = this.liczWejścia();
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
        Dictionary<string, string> _ustawienia;
        public mikser()
        {
            wyjście = new Typ[1];
            wyjście[0] = new Typ();
            wejście = new List<Typ>();
            _ustawienia = new Dictionary<string, string>();
            
        }
        Dictionary<long, List<nuta>> nuty = new Dictionary<long, List<nuta>>();
        public void działaj(nuta input)
        {
            List<nuta> a;
            if (nuty.ContainsKey(input.idOryginalne))
                a= nuty[input.idOryginalne];
            else
            {
                a = new List<nuta>();
                nuty[input.idOryginalne] = a;
            }
            lock (a)
            {
                a.Add(input);
                if (a.Count >= ilewej)//todo dla opuźnienia
                {
                    long dlugosc = 0;

                    foreach (var x in a)
                    {
                        if (x.dane.LongLength > dlugosc)
                            dlugosc = x.dane.LongLength;
                    }
                    bool balans = true;
                    var bal = a[0].balans0 / a[0].balans1;
                    foreach (var x in a)
                    {
                        if (x.balans0 / x.balans1 != bal)
                        {
                            balans = false;
                            break;
                        }
                    }
                    if (balans)
                    {
                        var n = (nuta)a[0].Clone();
                        n.id = a[0].idOryginalne;

                        /*n.opuznienie = a[0].opuznienie;
                        n.balans0 = a[0].balans0;
                        n.balans1 = a[0].balans1;
                        n.id = a[0].idOryginalne;
                        n.grajDo = a[0].grajDo;
                        n.grajOd = a[0].grajOd;
                        n.generujDo = a[0].generujDo;
                        n.generujOd = a[0].generujOd;*/
                        n.dane = new float[dlugosc];
                        foreach (var x in a)
                        {
                            if (x.głośność == 1)
                                for (var i = 0; i < x.dane.Length; i++)
                                {
                                    n.dane[i] += x.dane[i];
                                }
                            else
                                for (var i = 0; i < x.dane.Length; i++)
                                {
                                    n.dane[i] += x.dane[i] * x.głośność;
                                }
                        }

                        wyjście[0].DrógiModół.działaj(n);
                    }
                    else
                    {
                        var nl = (nuta)a[0].Clone();
                        nl.id = a[0].idOryginalne;
                        var np = (nuta)a[0].Clone();
                        np.id = a[0].idOryginalne * 256;
                        nl.balans0 = 1;
                        nl.balans1 = 0;
                        nl.głośność = 1;
                        np.balans0 = 0;
                        np.balans1 = 1;
                        np.głośność = 1;
                        /*n.opuznienie = a[0].opuznienie;
                        n.balans0 = a[0].balans0;
                        n.balans1 = a[0].balans1;
                        n.id = a[0].idOryginalne;
                        n.grajDo = a[0].grajDo;
                        n.grajOd = a[0].grajOd;
                        n.generujDo = a[0].generujDo;
                        n.generujOd = a[0].generujOd;*/
                        nl.dane = new float[dlugosc];
                        np.dane = new float[dlugosc];
                        foreach (var x in a)
                        {
                            var gll = x.głośność * x.balans0;
                            var glp = x.głośność * x.balans1;
                                for (var i = 0; i < x.dane.Length; i++)
                                {
                                    nl.dane[i] += x.dane[i] * gll;
                                    np.dane[i] += x.dane[i] * glp;
                                }
                        }

                        wyjście[0].DrógiModół.działaj(nl);
                        wyjście[0].DrógiModół.działaj(np);
                    }
                a.Clear();
                }
            }
            //wyjście[0].DrógiModół.działaj(input);
        }
    }
}