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
        public sekwencer sekw{get;set;}
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
               granie.liczbaGenerowanychMax = nuty.Count;
               granie.liczbaGenerowanych += nuty.Count;
               foreach(var prz in nuty)
            {var tabl = (nuta)prz.Clone();
            System.Threading.ThreadPool.QueueUserWorkItem((o) =>
            {
                if(sekw!=null)
                sekw.wyjście[0].DrógiModół.działaj(tabl);
                lock (granie.liczbaGenerowanychBlokada)
                {
                    granie.liczbaGenerowanych--;
                }
            }, tabl);
            }
           }
        }
    }
}
