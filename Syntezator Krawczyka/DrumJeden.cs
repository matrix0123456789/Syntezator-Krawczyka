using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Syntezator_Krawczyka.Synteza;

namespace Syntezator_Krawczyka
{
    public class DrumJeden
    {
        public XmlNode xml;
        public nuta nuta = null;
        public soundStart sekw;
        public float wysokość;
        public short oktawy;
        public float czestotliwosc;
        public DrumJeden() { }
        public DrumJeden(XmlNode n)
        {
            this.xml = n;
            wysokość = float.Parse(n.Attributes.GetNamedItem("note").Value);
            oktawy = short.Parse(n.Attributes.GetNamedItem("oktawy").Value);
            czestotliwosc = float.Parse(n.Attributes.GetNamedItem("frequency").Value);
        }

        internal void nowyXML()
        {
            xml = Statyczne.otwartyplik.xml.CreateElement("drum");
            xml.Attributes.Append(Statyczne.otwartyplik.xml.CreateAttribute("note"));
            xml.Attributes.Append(Statyczne.otwartyplik.xml.CreateAttribute("sound"));
        }

    }
}
