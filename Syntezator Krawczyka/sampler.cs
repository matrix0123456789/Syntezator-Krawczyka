using Syntezator_Krawczyka.Synteza;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Syntezator_Krawczyka
{
    class sampler : soundStart
    {
        public bool czyWłączone { get { return true; } }
        public float głośność = 1;
        public void działaj(nuta input)
        {
            for (var i = 0; i < sample.Count; i++)
            {
                if (funkcje.ilepróbek(0, sample[i].accept) == input.ilepróbek)
                {
                    var zmianaCzęstotliwości = plik.Hz/sample[i].częstotliwość ;
                    if (zmianaCzęstotliwości == 1)
                        input.dane = sample[i].fala;
                    else if((int)Math.Ceiling(sample[i].fala.Count() / zmianaCzęstotliwości) - input.generujOd>0)
                    {
                        input.dane = new float[(int)Math.Ceiling(sample[i].fala.Count() * zmianaCzęstotliwości) - input.generujOd+256];
                        for (var i2 = 0; input.dane.Count() > i2 - input.generujOd; i2++)
                    {
                        var dz = (i2+input.generujOd) / zmianaCzęstotliwości;
                        if (dz+1 < sample[i].fala.Length)
                            input.dane[i2] = ((sample[i].fala[(int)Math.Floor(dz)] * ((i2 / zmianaCzęstotliwości) % 1)) + (sample[i].fala[(int)Math.Ceiling(dz)] * (1 - (i2 / zmianaCzęstotliwości) % 1)))*głośność;

                        else if((int)Math.Floor(dz)+1<sample[i].fala.Length)
                        {
                            input.dane[i2] = (sample[i].fala[(int)Math.Floor(dz)])*głośność;
                        }
                            //debugowanie
                        
                        
                    }
                    }
                    else
                    {
                        input.dane = new float[0];
                    }
                    granie.Działaj(input);
                    break;
                }
            }
        }
        public long symuluj(long p)
        {
            return p;
        }
        public List<sample> sample = new List<sample>();
        public sampler()
        {
            granie.graniestart();
        }
        public sampler(float głośność):this()
    {
        this.głośność = głośność;
    }
    }
}
