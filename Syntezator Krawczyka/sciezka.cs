using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syntezator_Krawczyka.Synteza;
using System.Windows;

namespace Syntezator_Krawczyka
{
    /// <summary>
    /// prezentuje ścieszkę dźwiękową (linię melodyczną) w celu odtworzenia automatycznie (bez grania na żywo z klawiatury)
    /// </summary>
    public class sciezka:wejście
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
        public sciezka()
        {
            UI = new sciezkaUI(this);
        }
        public sciezka(string Nazwa)
        {
            nazwa = Nazwa;
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
    }
}
