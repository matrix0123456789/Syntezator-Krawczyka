using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Syntezator_Krawczyka
{
    public class gra
    {
        public float[] dźwięk;
        public long zagrano=0;
        public nuta nuta;
        public gra(float[] dźwięk)
        { this.dźwięk = dźwięk; }
        public gra(nuta nuta)
        {
            this.nuta = nuta;
            this.dźwięk = nuta.dane;
            zagrano = -nuta.opuznienie;
        }
        public string ToString()
        {
            return nuta.ToString();
        }
    }
}
