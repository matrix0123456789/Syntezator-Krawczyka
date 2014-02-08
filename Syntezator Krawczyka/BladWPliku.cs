using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Syntezator_Krawczyka
{
    class BladWPliku:Exception
    {
        BladWPlikuTyp typ;
        public BladWPliku(BladWPlikuTyp message)
        {
            
        }
    }
    public enum BladWPlikuTyp { }
}
