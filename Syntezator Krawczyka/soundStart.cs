using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Syntezator_Krawczyka
{
    public interface soundStart
    {
        void działaj(nuta input);
        bool czyWłączone{get;}

        long symuluj(long p);
    }
}
