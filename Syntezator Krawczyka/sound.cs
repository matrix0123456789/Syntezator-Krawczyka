using Syntezator_Krawczyka.Synteza;
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
        /* public int liczWejścia(moduł m)//TODO zabezpieczyć przed pętlami
         {
             int l=0;
             if (m.GetType() == typeof(sekwencer))
                 return 1;
             foreach(var x in this.Values)
             {
                 foreach(var y in x.wyjście)
                 {
                     if (y.DrógiModół == m)
                         l += liczWejścia(x);
                 }
             }
             return l;
         }*/
        public XmlNode xml;
        public sound(string nazwa, XmlNode n)
        {
            this.nazwa = nazwa;
            xml = n;
        }
    }
}
