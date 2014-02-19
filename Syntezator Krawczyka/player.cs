using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Threading;
using System.Xml;

namespace Syntezator_Krawczyka.Synteza
{
    public class player : moduł
    {
        public XmlNode XML { get; set; }
        public UserControl UI
        {
            get { return _UI; }
        }
        public void akt() { }
        UserControl _UI;
        public List<Typ> wejście { get; set; }
        public Typ[] wyjście
        {
            get { return _wyjście; }
        }
        Typ[] _wyjście;
        public Dictionary<string, string> ustawienia
        {
            get { return _ustawienia; }
        }
        Dictionary<string, string> _ustawienia;
        public sciezka sciezkaa;
        public player()
        {
            _UI = new playerUI(this);
            wejście = new List<Typ>();
            _wyjście = new Typ[1];
            _wyjście[0] = new Typ();
            _ustawienia = new Dictionary<string, string>();
            _ustawienia.Add("oktawy", "0");
        }
        public long symuluj(long p)
        {
            return wyjście[0].DrógiModół.symuluj(p);
        }
        public void działaj(nuta input)
        {
        }
        public void graj(bool graj)
        {

            //granie.o = plik.Hz0;
            //granie.t.Change((int)(granie.o / plik.kHz),(int)( granie.o / plik.kHz));

            List<moduł> mod = new List<moduł>();
            mod.Add(this);
            granie gran;
            short oktawy = short.Parse(ustawienia["oktawy"]);
            granie.grają = new Dictionary<long, gra>();
            granie.grateraz = true;
            if (wyjście[0].DrógiModół != null)
            {
                EventWaitHandle[] synchronizacja = new EventWaitHandle[(int)Math.Floor((sciezkaa.nuty.Count+99)/100f)];
                
                for (int i = 0; sciezkaa.nuty.Count > i; i+=100)
                {
                    synchronizacja[(int)Math.Floor(i/100f)]=new AutoResetEvent(false);
                    System.Threading.ThreadPool.QueueUserWorkItem((o) =>
                    {
                        for (int i2 = (int)o; sciezkaa.nuty.Count > i2; i2++)
                        {
                            wyjście[0].DrógiModół.działaj((nuta)sciezkaa.nuty[i2].Clone(oktawy));
                        }
                        synchronizacja[(int)Math.Floor((int)o / 100f)].Set();
                    }, i);
                }
                foreach (var a in synchronizacja)
                {
                    a.WaitOne();
                }
                for (int i = 0; i < mod.Count; i++)
                {
                    foreach (Typ typy in mod[i].wyjście)
                    {
                        try
                        {
                            if (typy.DrógiModół.GetType() == typeof(granie))
                            {
                                //((granie)typy.DrógiModół).grajcale(graj);
                                //((granie)typy.DrógiModół).grajcale(graj);
                                break;
                            }
                        }
                        catch(NullReferenceException e){}
                        if (typy.DrógiModół != null)
                            mod.Add(typy.DrógiModół);
                    }
                }
            }
        }
    }
}
