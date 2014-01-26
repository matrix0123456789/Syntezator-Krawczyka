using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Globalization;

namespace Syntezator_Krawczyka.Synteza
{
    public class pogłos:moduł
    {
         public UserControl UI
        {
            get { return _UI; }
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
            _UI = new pogłosUI(this);
        }
        public long symuluj(long wej)
        {
            if (wyjście[0].DrógiModół != null)
            {

                var czas = (long)(float.Parse(_ustawienia["czas"], CultureInfo.InvariantCulture) * plik.Hz);
                var zmniejszenie = float.Parse(_ustawienia["zmniejszenie"], CultureInfo.InvariantCulture);
                var ilejest = float.Parse(_ustawienia["glosnosc"], CultureInfo.InvariantCulture);

                var ix = 0;
                while (ilejest * zmniejszenie > 0.02)
                {
                    wej += czas;
                    ilejest *= zmniejszenie;
                }
                return wyjście[0].DrógiModół.symuluj(wej);
            }
            else return 0;
        }
        public void działaj(nuta input)
        {
            
                    if (wyjście[0].DrógiModół != null)
                    {

                        var czas = (long)(float.Parse(_ustawienia["czas"], CultureInfo.InvariantCulture) * plik.Hz);
                        var zmniejszenie = float.Parse(_ustawienia["zmniejszenie"], CultureInfo.InvariantCulture);
                        var ilejest = float.Parse(_ustawienia["glosnosc"], CultureInfo.InvariantCulture);
                        
                        long czasjest = 0;
                        var ix = 0;
                        bool parzyste = false;
                        while (ilejest * zmniejszenie > 0.01)
                        {
                            czasjest += czas;
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
                            else{
                            inp.dane=new float[input.dane.Length];
                            int max; 
                            if (inp.dane.Length < inp.grajDo)
                                max = inp.dane.Length;
                            else
                                max = (int)inp.grajDo;
                            for (int i = (int)inp.grajOd; max > i; i++)
                            {
                                inp.dane[i] = input.dane[i] * ilejest;
                            }
                            } var Balans = float.Parse(_ustawienia["balans"], CultureInfo.InvariantCulture);
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
