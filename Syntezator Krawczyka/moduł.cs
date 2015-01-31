using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;

namespace Syntezator_Krawczyka
{
    /// <summary>
    /// interfejs dla każdego modułu (oscylator, filtry itd.)
    /// </summary>
    public interface moduł
    {

        UserControl UI
        {
            get;
        }
        List<Typ> wejście { get; set; }
        /// <summary>
        /// Elementy, do których dalej będą przekazywane dane
        /// </summary>
        Typ[] wyjście
        {
            get;
            set;
        }

        Dictionary<string,string> ustawienia
        {
            get;
        }
        XmlNode XML { get; set; }
        /*void aktXML()
        {
            
        }*/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        void działaj(nuta input);


        long symuluj(long p);

        void akt();
    }
    public static class modułFunkcje
    {
        static public void czytajXML(Dictionary<string, string> ustawienia, XmlNode XML)
        {
            for (int i = 0; i < XML.Attributes.Count; i++)
            {
                if (XML.Attributes.Item(i).Name != "id" && XML.Attributes.Item(i).Name != "name" && XML.Attributes.Item(i).Name != "output")
                {
                    if(ustawienia.ContainsKey(XML.Attributes.Item(i).Name))
                    ustawienia[XML.Attributes.Item(i).Name]=XML.Attributes.Item(i).Value;
                    else
                    ustawienia.Add(XML.Attributes.Item(i).Name, XML.Attributes.Item(i).Value);
                }
            }
        }
        static public void zapiszXML(Dictionary<string, string> ustawienia, XmlNode XML)
        {
            foreach (var x in ustawienia)
            {
                try
                {
                    XML.Attributes.GetNamedItem(x.Key).Value = x.Value;
                }
                catch
                {
                    var atr = XML.OwnerDocument.CreateAttribute(x.Key);
                    atr.Value = x.Value;
                    XML.Attributes.Append(atr);

                }
            }
        }
    }
}
