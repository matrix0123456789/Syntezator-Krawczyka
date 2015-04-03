using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace Syntezator_Krawczyka.JaebeHelp
{
    public class HelpDatabase
    {
        public static HelpDatabase baza { get {
            if (_baza == null)
            {
                var doc = new XmlDocument();
                doc.LoadXml(Syntezator_Krawczyka.Properties.Resources.wiki);
                _baza = new JaebeHelp.HelpDatabase(doc);
            }
            return _baza;
        } }
        static HelpDatabase(){
            ThreadPool.QueueUserWorkItem((a) =>
            {
                var doc = new XmlDocument();
                doc.Load("http://jaebestudio.tk/wiki.php");
                _baza = new JaebeHelp.HelpDatabase(doc);
            });
    }
        static HelpDatabase _baza;
        public Dictionary<string, Dictionary<string, HelpPage>> poNazwie = new Dictionary<string, Dictionary<string, HelpPage>>();
        public Dictionary<string, Dictionary<string, HelpPage>> poPolskiejNazwie = new Dictionary<string, Dictionary<string, HelpPage>>();
        public HelpDatabase(XmlDocument doc)
        {
            
            foreach(XmlElement d in doc.GetElementsByTagName("page"))
            {
                var str = new HelpPage();
                XmlNode atr;
                if ((atr = d.Attributes.GetNamedItem("lang")) != null)
                    str.lang = atr.InnerText;
                if ((atr = d.Attributes.GetNamedItem("nazwa")) != null)
                {
                    str.nazwa = atr.InnerText;
                    Dictionary<string, HelpPage> jezyk;
                    if (poNazwie.ContainsKey(str.lang))
                        jezyk = poNazwie[str.lang];
                    else
                    {
                        jezyk = new Dictionary<string, HelpPage>();
                        poNazwie[str.lang] = jezyk;
                    }
                    jezyk.Add(str.nazwa, str);
                }
                if ((atr = d.Attributes.GetNamedItem("polskanazwa")) != null)
                {
                    str.polskanazwa = atr.InnerText;
                    Dictionary<string, HelpPage> jezyk;
                    if (poPolskiejNazwie.ContainsKey(str.lang))
                        jezyk = poPolskiejNazwie[str.polskanazwa];
                    else
                    {
                        jezyk = new Dictionary<string, HelpPage>();
                        poPolskiejNazwie[str.polskanazwa] = jezyk;
                    }
                    jezyk.Add(str.lang, str);
                }
                    str.tre = d.InnerText;
            }
        }
    }
    public class HelpPage
    {
        public string tre;
        public string nazwa;
        public string polskanazwa;
        public string lang;
        public int id;
    }
}
