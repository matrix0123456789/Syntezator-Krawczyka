using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Globalization;
using AForge.Math;

namespace Syntezator_Krawczyka.Synteza
{
    public class rekonstruktor : moduł
    {
        public UserControl UI
        {
            get
            {
                if (_UI == null)

                    _UI = new rekonstruktorUI(this);
                return _UI;
            }
        }
        public void akt()
        {
            dlugosc = int.Parse(ustawienia["dlugosc"], CultureInfo.InvariantCulture);
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
            get { return _wyjście; }set { _wyjście = value; }
        }
        Typ[] _wyjście;
        public Dictionary<string, string> ustawienia
        {
            get { return _ustawienia; }
        }
        Dictionary<string, string> _ustawienia;
        public rekonstruktor()
        {
            _wyjście = new Typ[1];
            _wyjście[0] = new Typ();
            wejście = new List<Typ>();
            _ustawienia = new Dictionary<string, string>();
            _ustawienia.Add("dlugosc", "1024");
        }
        int dlugosc = 1024;
        public void działaj(nuta input)
        {


            var zespolone = new Complex[dlugosc];
            for (int i = 0; i < input.dane.Length;i+=dlugosc )
            {
                var gran=dlugosc;
                int gran2=gran;
                if(gran>input.dane.Length-i)
                    gran2 = input.dane.Length - i;
                int j = 0;
                for (; j < gran2; j++)
                {
                    zespolone[j].Re = input.dane[i + j];
                    zespolone[j].Im = 0;
                }
                for (; j < gran; j++)
                {
                    zespolone[j].Re = 0;
                    zespolone[j].Im = 0;
                }
                AForge.Math.FourierTransform.FFT(zespolone, FourierTransform.Direction.Forward);
               /* for (j = 16; j < gran; j++)
                {
                    if (j % 16 == 0)
                    {
                        for(int z=1;z<16;z++)
                        {
                            zespolone[j].Re += zespolone[j + z].Re;
                            zespolone[j].Im += zespolone[j + z].Im;
                        }
                    }
                    else
                    {
                        zespolone[j].Re = 0;
                        zespolone[j].Im = 0;
                    }
                }
                for (j = 32; j < gran; j++)
                {
                    zespolone[j].Re = 0;
                    zespolone[j].Im = 0;
                }*/
                    AForge.Math.FourierTransform.FFT(zespolone, FourierTransform.Direction.Backward);
                    for ( j = 0; j < gran2; j++)
                    {
                        input.dane[i + j] = (float)zespolone[j].Re;
                    }
            }
            wyjście[0].DrógiModół.działaj(input);
        }

        public float głośność { get; set; }
    }
}