﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Globalization;

namespace Syntezator_Krawczyka.Synteza
{
    public class rozdzielacz : moduł
    {
        public UserControl UI
        {
            get
            {
                if (_UI == null)
                    _UI = new UserControl();
                return _UI;
            }
        }
        public XmlNode XML { get; set; }
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
        Dictionary<nuta, nuta[]> referencjeNut = new Dictionary<nuta, nuta[]>();
        Dictionary<moduł, List<Typ>> flangery = null;
        Object flangeryBlock = new object();
        public void akt() { }
        public void aktt()
        {
            lock (flangeryBlock)
            {
                flangery = new Dictionary<moduł, List<Typ>>();
                for (var i = 0; i < wyjście.Length; i++)
                {
                    if (wyjście[i].DrógiModół != null)
                    {
                        if (wyjście[i].DrógiModół.GetType() == typeof(flanger))
                        {
                            if (flangery.ContainsKey(wyjście[i].DrógiModół.wyjście[0].DrógiModół))
                            {
                                flangery[wyjście[i].DrógiModół.wyjście[0].DrógiModół].Add(wyjście[i]);
                            }
                            else
                            {
                                var lista = new List<Typ>();
                                lista.Add(wyjście[i]);
                                flangery.Add(wyjście[i].DrógiModół.wyjście[0].DrógiModół, lista);
                            }

                        }

                    }
                }
            }
        }
        public rozdzielacz()
        {
            wejście = new List<Typ>();
            _wyjście = new Typ[8];
            _wyjście[0] = new Typ();
            _wyjście[1] = new Typ();
            _wyjście[2] = new Typ();
            _wyjście[3] = new Typ();
            _wyjście[4] = new Typ();
            _wyjście[5] = new Typ();
            _wyjście[6] = new Typ();
            _wyjście[7] = new Typ();
            _ustawienia = new Dictionary<string, string>();
        }
        public long symuluj(long wej)
        {
            long ret = 0;
            for (var i = 0; i < wejście.Count; i++)
            {
                if (wyjście[i].DrógiModół != null)
                {
                    long t = wyjście[i].DrógiModół.symuluj(wej);
                    if (t > ret)
                        ret = t;
                }
            }
            return ret;
        }
        public void działaj(nuta input)
        {
            /*if (referencjeNut.ContainsKey(input))
            {
                for (var i = 0; i < 8; i++)
                {
                    if (wyjście[i].DrógiModół != null)
                    {
                        var nutka = referencjeNut[input][i];
                        nutka.dane = input.dane;
                        nutka.długość = input.długość;
                        nutka. = input.dane;
                        wyjście[i].DrógiModół.działaj(nutka);
                    }
                }
            }
            else
            {
                var refer = new nuta[8];
                for(var i=0;i<8;i++)
                {
                    if (wyjście[i].DrógiModół != null)
                    {
                        var klon = (nuta)input.Clone();
                        wyjście[i].DrógiModół.działaj(klon);
                        refer[i] = klon;
                    }
                }
                referencjeNut.Add(input, refer);
            }*/
            //lock (granie.obLock)
            {
                if (flangery == null)
                    aktt();
                foreach (var x in flangery)
                {
                    //var dane = input.dane;
                    var dane = new float[input.dane.Length];
                    List<Typ> xval;
                    lock (flangeryBlock)
                    {
                        xval = x.Value;
                    }
                    foreach (var xx in xval)
                    {
                        dane = (xx.DrógiModół as flanger).działaj(input, dane);
                    }
                    input.dane = dane;
                    x.Key.działaj(input);
                }
                for (var i = wyjście.Length-1; i >= 0; i--)
                {
                    if (wyjście[i].DrógiModół != null)
                    {
                        if (wyjście[i].DrógiModół.GetType() != typeof(flanger))
                        {
                            if (i == 0)
                                wyjście[i].DrógiModół.działaj(input);
                            else
                            {
                                var klon = (nuta)input.Clone();
                                klon.id = klon.id * 0x1000 + i;
                                //klon.dane = new double[klon.dane.Length];
                                /*for (var ei = 0; ei < klon.dane.Length; ei++)
                                {
                                    klon.dane[ei] = input.dane[ei];
                                }*/
                                wyjście[i].DrógiModół.działaj(klon);
                                //refer[i] = klon;
                            }
                        }
                    }
                }
            }
        }
    }
}