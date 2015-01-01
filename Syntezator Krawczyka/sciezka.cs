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
    public class sciezka : wejście, IComparable<sciezka>, IodDo, IDisposable
    {
        /// <summary>
        /// Lista nut
        /// </summary>
        public List<nuta> nuty = new List<nuta>();
        /// <summary>
        /// sekwencer, do którego zostaną wysłane nuty
        /// </summary>
        public soundStart sekw { get; set; }
        UIElement _UI = null;
        public UIElement UI
        {
            get
            {
                if (_UI == null)
                    _UI = new sciezkaUI(this);
                return _UI;
            }
        }
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
        }
        public sciezka(string Nazwa, XmlNode xml)
        {
            nazwa = Nazwa;
            this.xml = xml;
        }
        public sciezka(string Nazwa, XmlNode xml, bool kopia)
        {

            this.xml = xml;
            if (kopia && (Nazwa.Length < 8 || Nazwa.Substring(Nazwa.Length - 8) != " (kopia)"))
                nazwa = Nazwa + " (kopia)";
            else
                nazwa = Nazwa;
            this.kopia = kopia;
        }
        /// <summary>
        /// przesyła nuty do sekwencera
        /// </summary>
        public void działaj()
        {
            lock (granie.grają)
            {
                granie.liczbaGenerowanychMax += nuty.Count;
                granie.liczbaGenerowanych += nuty.Count;
                foreach (var prz in nuty)
                {
                    var tabl = (nuta)prz.Clone();
                    tabl.grajDo = long.MaxValue;
                    System.Threading.ThreadPool.QueueUserWorkItem((o) =>
                    {
                        if (sekw != null)

                            sekw.działaj(tabl);
                        lock (granie.liczbaGenerowanychBlokada)
                        {
                            granie.liczbaGenerowanych--;
                            if (!granie.można && granie.liczbaGenerowanych == 0)

                                granie.grajcale(false);
                        }
                    }, tabl);
                }
            }
        }

        public double delayUstawione;
        private long? _delay = null;
        public long delay
        {
            get
            {
                if (_delay == null)
                    _delay = 0;
                return (long)_delay;
            }
            set
            {
                if (_delay == null)
                    _delay = value;
                else if (value != _delay)
                {
                    var roznica = (value - (long)_delay);
                    foreach (var x in nuty)
                    {
                        x.opuznienie += roznica;
                    }
                    _delay = value;
                }
            }
        }

        public int CompareTo(sciezka other)
        {
            if (delay - other.delay > 0)
                return 1;
            else
                return -1;
        }

        public sciezka oryginał;
        public long dlugosc
        {
            get
            {
                long akt = 0;
                foreach (var x in nuty)
                {
                    if (x.opuznienie + x.ilepróbekNaStarcie > akt)
                        akt = x.opuznienie + x.długość;
                }
                return akt-delay;
            }
        }
        public override string ToString()
        {
            return nazwa;
        }

        public void Dispose()
        {
            if (_UI != null)
                (_UI as IDisposable).Dispose();
        }
    }
}
