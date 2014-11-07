using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Syntezator_Krawczyka
{
    public class sound : Dictionary<string, moduł>
    {
        public soundStart sekw;
        public string nazwa;
        public Instrument UI;
        public sound() { }
        public XmlNode xml;
        public sound(string nazwa, XmlNode n)
        {
            this.nazwa = nazwa;
            xml = n;
        }
    }
}
