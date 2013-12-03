using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Globalization;

namespace Syntezator_Krawczyka.Synteza
{
    public class lfo : moduł
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
        public lfo()
        {
            wejście = new List<Typ>();
            _wyjście = new Typ[2];
            _wyjście[0] = new Typ();
            _wyjście[1] = new Typ();
            _ustawienia = new Dictionary<string, string>();
            _ustawienia.Add("moc", "0");
            _ustawienia.Add("czestotliwosc", "0");
            _ustawienia.Add("typ", "sinusoidalna");
            _ustawienia.Add("gladkosc", "0");
            _ustawienia.Add("kwantyzacja", "0");
            _ustawienia.Add("nowanuta", "false");
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
                    var jedenPrzebieg = oscylator.generujJedenPrzebiegStatyczny(ustawienia["typ"], (long)(1 / float.Parse(_ustawienia["czestotliwosc"], CultureInfo.InvariantCulture) * plik.Hz), float.Parse(_ustawienia["gladkosc"], CultureInfo.InvariantCulture));
                    var kwantyzacja = float.Parse(_ustawienia["kwantyzacja"], CultureInfo.InvariantCulture);
                    if (wyjście[0].DrógiModół.GetType() == typeof(oscylator) && bool.Parse(_ustawienia["nowanuta"]))
                    {//do optymalizacji
                        var pozA = 0f;//sprawdzanie, co było wcześniej
                        var pozB = 0f;
                        //long opuzn, opuzn2;
                        //opuzn=0;
                        for (int i = 0; i < n.długość; i++)
                        {
                            var miejsce = (i + n.generujOd) % jedenPrzebieg.Length;
                            //jak[i] = n.dane[i] * (1 + (-jedenPrzebieg[miejsce] * 0.5f - 0.5f) * moc);
                            pozB = (float)Math.Round((1 + (-jedenPrzebieg[(i + n.generujOd) % jedenPrzebieg.Length] * 0.5f - 0.5f)) / kwantyzacja) * kwantyzacja * moc;
                            if (pozB != pozA)
                            {
                                pozA = pozB;
                                var nowaNuta = input.Clone() as nuta;
                                nowaNuta.id = nuta.nowyid;
                                nowaNuta.ilepróbek = input.ilepróbek / Math.Pow(2, pozA);//sprawdzić, czy to ma sens
                                nowaNuta.opuznienie += i;
                                nowaNuta.długość = (long)nowaNuta.ilepróbek;
                                //generowanie nuty
                                wyjście[0].DrógiModół.działaj(nowaNuta);
                                //opuzn = i;
                            }
                        }
                    }
                    else
                    {

                        if (kwantyzacja == 0)
                        {
                            if (n.opuznienie == 0)
                                for (int i = 0; i < jedenPrzebieg.Length; i++)
                                {
                                    //jak[i] = n.dane[i] * (1 + (-jedenPrzebieg[miejsce] * 0.5f - 0.5f) * moc);
                                    jedenPrzebieg[i] = 1 + (-jedenPrzebieg[(i + n.opuznienie) % jedenPrzebieg.Length] * 0.5f - 0.5f) * moc;
                                }
                            else
                            {
                                float[] jedenPrzebieg2 = new float[jedenPrzebieg.Length];
                                for (int i = 0; i < jedenPrzebieg.Length; i++)
                                {
                                    //jak[i] = n.dane[i] * (1 + (-jedenPrzebieg[miejsce] * 0.5f - 0.5f) * moc);
                                    jedenPrzebieg2[i] = 1 + (-jedenPrzebieg[(i + n.opuznienie) % jedenPrzebieg.Length] * 0.5f - 0.5f) * moc;
                                }
                                jedenPrzebieg = jedenPrzebieg2;
                            }
                        }
                        else
                        {
                            if (n.opuznienie == 0)
                                for (int i = 0; i < jedenPrzebieg.Length; i++)
                                {
                                    //jak[i] = n.dane[i] * (1 + (-jedenPrzebieg[miejsce] * 0.5f - 0.5f) * moc);
                                    jedenPrzebieg[i] = (float)Math.Round((1 + (-jedenPrzebieg[(i + n.opuznienie) % jedenPrzebieg.Length] * 0.5f - 0.5f)) / kwantyzacja) * kwantyzacja * moc;
                                }
                            else
                            {
                                float[] jedenPrzebieg2 = new float[jedenPrzebieg.Length];
                                for (int i = 0; i < jedenPrzebieg.Length; i++)
                                {
                                    //jak[i] = n.dane[i] * (1 + (-jedenPrzebieg[miejsce] * 0.5f - 0.5f) * moc);
                                    jedenPrzebieg2[i] = (float)Math.Round((1 + (-jedenPrzebieg[(i + n.opuznienie) % jedenPrzebieg.Length] * 0.5f - 0.5f)) / kwantyzacja) * kwantyzacja * moc;
                                }
                                jedenPrzebieg = jedenPrzebieg2;
                            }
                        }

                        if (wyjście[0].DrógiModół != null)
                        {
                            if (wyjście[1].DrógiModół != null)
                                wyjście[0].DrógiModół.działaj(((filtr)wyjście[1].DrógiModół).działaj(input, jedenPrzebieg));
                            else if (wyjście[0].DrógiModół.GetType() == typeof(oscylator))
                            {
                                (wyjście[0].DrógiModół as oscylator).działaj(input, jedenPrzebieg);

                            }
                        }
                    }
                }
            }


        }
    }
}
