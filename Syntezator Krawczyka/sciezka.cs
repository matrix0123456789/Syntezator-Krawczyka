using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syntezator_Krawczyka.Synteza;
using System.Windows;
using System.Xml;

namespace Syntezator_Krawczyka
{
    /// <summary>
    /// prezentuje ścieszkę dźwiękową (linię melodyczną) w celu odtworzenia automatycznie (bez grania na żywo z klawiatury)
    /// </summary>
    public class sciezka : wejście, IComparable<sciezka>
    {
        /// <summary>
        /// Lista nut
        /// </summary>
        public List<nuta> nuty = new List<nuta>();
        /// <summary>
        /// sekwencer, do którego zostaną wysłane nuty
        /// </summary>
        public soundStart sekw{get;set;}
        public UIElement UI{get;set;}
        /// <summary>
        /// nazwa
        /// </summary>
        public string nazwa = "nazwa";

        /// <summary>
        /// XML przedstawiający ścierzkę
        /// </summary>
        public XmlNode xml;
        /// <summary>
        /// Czy ścieżka jest kopią innej ścierzki
        /// </summary>
        public bool kopia = false;
        public sciezka()
        {
            UI = new sciezkaUI(this);
        }
        public sciezka(string Nazwa, XmlNode xml)
        {
            nazwa = Nazwa;
            this.xml = xml;
            UI = new sciezkaUI(this);
        }
        public sciezka(string Nazwa, XmlNode xml, bool kopia)
        {

            this.xml = xml;
            if (kopia)
                nazwa = Nazwa + " (kopia)";
            else
                nazwa = Nazwa;
            this.kopia = kopia;
            UI = new sciezkaUI(this);
        }
        /// <summary>
        /// przesyła nuty do sekwencera
        /// </summary>
        public void działaj()
        {
           lock(granie.grają)
           {
               granie.liczbaGenerowanychMax+= nuty.Count;
               granie.liczbaGenerowanych += nuty.Count;
               foreach(var prz in nuty)
            {var tabl = (nuta)prz.Clone();
            tabl.grajDo = long.MaxValue;
            System.Threading.ThreadPool.QueueUserWorkItem((o) =>
            {
                if(sekw!=null)
                
sekw.działaj(tabl);
                lock (granie.liczbaGenerowanychBlokada)
                {
                    granie.liczbaGenerowanych--;
                    if(!granie.można&&granie.liczbaGenerowanych==0)

                        granie.grajcale(false);
                }
            }, tabl);
            }
           }
        }

        public int delay = 0;


        public int CompareTo(sciezka other)
        {
            return delay - other.delay;
        }
    }
}
