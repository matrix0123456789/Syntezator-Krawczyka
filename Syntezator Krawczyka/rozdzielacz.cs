using System;
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
        Dictionary<nuta, nuta[]> referencjeNut = new Dictionary<nuta, nuta[]>();
        public rozdzielacz()
        {
            _wejście = new Typ[1];
            _wejście[0] = new Typ();
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
            _UI = new UserControl();
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
                Dictionary<moduł, List<Typ>> flangery = new Dictionary<moduł, List<Typ>>();
                for (var i = 0; i < 8; i++)
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
                foreach (var x in flangery)
                {
                    var dane = new float[input.dane.Length];
                    foreach (var xx in x.Value)
                    {
                        dane = (xx.DrógiModół as flanger).działaj(input, dane);
                    }
                    input.dane=dane;
                    x.Key.działaj(input);
                }
                for (var i = 0; i < 8; i++)
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