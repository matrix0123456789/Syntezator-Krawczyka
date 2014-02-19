using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Globalization;

namespace Syntezator_Krawczyka.Synteza
{
    public class generatorObwiedniFiltru : moduł
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
        public XmlNode XML { get; set; }
        public generatorObwiedniFiltru()
        {
            wejście = new List<Typ>();
            _wyjście = new Typ[2];
            _wyjście[0] = new Typ();
            _wyjście[1] = new Typ();
            _ustawienia = new Dictionary<string, string>();
            _ustawienia.Add("A", "200");
            _ustawienia.Add("D", "0");
            _ustawienia.Add("S", (0.5).ToString());
            _ustawienia.Add("R", "1000");
            _UI = new generatorObwiedniFiltruUI(this);
        }
        public void działaj(nuta input)
        {
            float[] jak;
            if (wyjście[0].DrógiModół != null)
            {
                float aProcent, dProcent;
                float rProcent = 1;
                aProcent = 0;
                var aMax = float.Parse(_ustawienia["A"], CultureInfo.InvariantCulture) * plik.kHz;
                var dMax = float.Parse(_ustawienia["D"], CultureInfo.InvariantCulture) * plik.kHz;
                var rMax = float.Parse(_ustawienia["R"], CultureInfo.InvariantCulture) * plik.kHz;
                    var s = float.Parse(_ustawienia["S"], CultureInfo.InvariantCulture);
                if (aMax == 0 && dMax == 0 & rMax == 0)
                {
                    jak = new float[1];
                    jak[0] = s;
                }
                else
                {
                    if (input.dane == null)
                    {
                        jak = new float[input.długość+(int)rMax];
                    
                    }
                    else
                    jak = new float[input.dane.Length];
                    aProcent = 1;
                    long długośćCała = (int)(Math.Floor((input.długość) / input.ilepróbek) * input.ilepróbek + float.Parse(_ustawienia["R"], CultureInfo.InvariantCulture) * plik.kHz);

                    for (int i = 0; i < jak.Length; i++)
                    {
                        if (długośćCała - i - input.generujOd > rMax)
                            if (aMax > i + (int)input.generujOd)
                                aProcent = (i + (int)input.generujOd) / aMax;
                            else
                                aProcent = 1;
                        else
                        {
                            rProcent = (długośćCała - i - input.generujOd) / rMax;
                            if (rProcent < 0)
                                rProcent = 0;
                            //aProcent = 1;
                        }
                        if (dMax > i + (int)input.generujOd)
                        {
                            dProcent = s + (dMax - i - (int)input.generujOd) / dMax * (1 - s);
                        }
                        else
                            dProcent = s;
                        jak[i] = aProcent * rProcent * dProcent;

                    }
                }

                if (wyjście[0].DrógiModół != null)
                {
                    if (wyjście[1].DrógiModół != null)
                        wyjście[0].DrógiModół.działaj(((filtr)wyjście[1].DrógiModół).działaj(input, jak));
                    else if (wyjście[0].DrógiModół.GetType() == typeof(oscylator))
                    {
                        (wyjście[0].DrógiModół as oscylator).działaj(input, jak);

                    }
                }
            }

        }
    }
}
