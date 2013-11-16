using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Syntezator_Krawczyka
{
    public class sound : Dictionary<string, moduł>
    {
        public soundStart sekw;
        public string nazwa;
        public Instrument UI;
        public sound() { }
        public sound(string nazwa) { this.nazwa = nazwa; }
    }
}
