using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Globalization;

namespace Syntezator_Krawczyka.Synteza
{
    public class pogłos : moduł
    {
        public UserControl UI
        {
            get
            {
                if (_UI == null)

                    _UI = new pogłosUI(this);
                return _UI;
            }
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
        public pogłos()
        {
            _wyjście = new Typ[1];
            _wyjście[0] = new Typ();
            wejście = new List<Typ>();
            _ustawienia = new Dictionary<string, string>();
            _ustawienia.Add("czas", "0");
            _ustawienia.Add("zmniejszenie", "0");

            _ustawienia.Add("glosnosc", "1");
            _ustawienia.Add("balans", "1");
            akt();
        }
        public long symuluj(long wej)
        {
            if (wyjście[0].DrógiModół != null)
            {

                var czas = (long)(float.Parse(_ustawienia["czas"], CultureInfo.InvariantCulture) * plik.Hz);
                var zmniejszenie = float.Parse(_ustawienia["zmniejszenie"], CultureInfo.InvariantCulture);
                var ilejest = float.Parse(_ustawienia["glosnosc"], CultureInfo.InvariantCulture);

                var ix = 0;
                while (ilejest * zmniejszenie > 0.01)
                {
                    wej += czas;
                    ilejest *= zmniejszenie;
                }
                return wyjście[0].DrógiModół.symuluj(wej);
            }
            else return 0;
        }
        float czas, zmniejszenie, głośność, Balans;
        long czas2;
        public void akt()
        {
            czas = float.Parse(_ustawienia["czas"], CultureInfo.InvariantCulture);
            czas2 = (long)(czas * plik.Hz);
            zmniejszenie = float.Parse(_ustawienia["zmniejszenie"], CultureInfo.InvariantCulture);
            głośność = float.Parse(_ustawienia["glosnosc"], CultureInfo.InvariantCulture);
            Balans = float.Parse(_ustawienia["balans"], CultureInfo.InvariantCulture);
        }
        public void działaj(nuta input)
        {

            if (wyjście[0].DrógiModół != null)
            {
                //var ilejest = głośność * .33333333333333333f;
                var ilejest = głośność;
                if (granie.wynik != null && wyjście[0].DrógiModół.GetType() == typeof(granie))
                {
                    long czasjest = 0;
                    bool parzyste = false;
                    do
                    {

                        lock (input)
                        {
                            if (czasjest == 0)
                            {
                                var op = input.opuznienie + czasjest;
                                var mn0 = input.balans0 * input.głośność;
                                var mn1 = input.balans1 * input.głośność;
                                for (long i = 0; i < input.dane.LongLength; i++)
                                {
                                    granie.wynik[0, i + op] += input.dane[i] * mn0;
                                    granie.wynik[1, i + op] += input.dane[i] * mn1;
                                   // if (granie.wynik[1, i + op] > 3)
                                 //       (5).ToString();
                                }
                            }
                            else if (Balans == 1)
                            {
                                if (parzyste)
                                {
                                    var op = input.opuznienie + czasjest;
                                    var mn0 = input.balans0 * input.głośność * ilejest;
                                    for (long i = 0; i < input.dane.LongLength; i++)
                                    {
                                        granie.wynik[0, i + op] += input.dane[i] * mn0;
                                    }
                                }
                                else
                                {
                                    var op = input.opuznienie + czasjest;
                                    var mn1 = input.balans1 * input.głośność * ilejest;
                                    for (long i = 0; i < input.dane.LongLength; i++)
                                    {
                                        granie.wynik[1, i + op] += input.dane[i] * mn1;
                                    }
                                }
                            }
                            else if (parzyste)
                            {
                                var op = input.opuznienie + czasjest;
                                var mn0 = input.balans0 * input.głośność * ilejest;
                                var mn1 = input.balans1 * input.głośność * ilejest * (1 - Balans);
                                for (long i = 0; i < input.dane.LongLength; i++)
                                {
                                    granie.wynik[0, i + op] += input.dane[i] * mn0;
                                    granie.wynik[1, i + op] += input.dane[i] * mn1;
                                }
                            }
                            else
                            {
                                var op = input.opuznienie + czasjest;
                                var mn0 = input.balans0 * input.głośność * ilejest * (1 - Balans);
                                var mn1 = input.balans1 * input.głośność * ilejest;
                                for (long i = 0; i < input.dane.LongLength; i++)
                                {
                                    granie.wynik[0, i + op] += input.dane[i] * mn0;
                                    granie.wynik[1, i + op] += input.dane[i] * mn1;
                                }
                            }

                            parzyste = !parzyste;

                            czasjest += czas2;
                            ilejest *= zmniejszenie;
                        }
                    }
                    while (ilejest * zmniejszenie > 0.01);
                    input.dane = null;
                    

                }
                else
                {


                    long czasjest = 0;
                    var ix = 0;
                    bool parzyste = false;
                    while (ilejest * zmniejszenie > 0.01)
                    {
                        czasjest += czas2;
                        ilejest *= zmniejszenie;
                        var inp = (nuta)input.Clone();
                        inp.id = inp.id * 256 + input.kopiaInnaId++;
                        inp.opuznienie += czasjest;
                        inp.generujOd = 0;
                        inp.czyPogłos = true;
                        if (wyjście[0].DrógiModół.GetType() == typeof(granie) || (wyjście[0].DrógiModół.GetType() == typeof(glosnosc) && wyjście[0].DrógiModół.wyjście[0].DrógiModół.GetType() == typeof(granie)))
                        {
                            inp.dane = input.dane;
                            inp.głośność = inp.głośność * ilejest;
                        }
                        else
                        {
                            inp.dane = new float[input.dane.Length];
                            int max;
                            if (inp.dane.Length < inp.grajDo)
                                max = inp.dane.Length;
                            else
                                max = (int)inp.grajDo;
                            for (int i = (int)inp.grajOd; max > i; i++)
                            {
                                inp.dane[i] = input.dane[i] * ilejest;
                            }
                        }
                        if (parzyste)
                            inp.balans1 *= (1 - Balans);
                        else
                            inp.balans0 *= (1 - Balans);

                        parzyste = !parzyste;
                        wyjście[0].DrógiModół.działaj(inp);
                    }
                    wyjście[0].DrógiModół.działaj(input);
                }


            }

        }
    }
}
