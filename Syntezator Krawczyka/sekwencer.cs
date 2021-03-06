﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Controls;
using System.Xml;

namespace Syntezator_Krawczyka.Synteza
{
    public class sekwencer : moduł, soundStart
    {
        public XmlNode XML { get; set; }
        public void akt()
        {
             oktawy = float.Parse(_ustawienia["oktawy"], CultureInfo.InvariantCulture);
        }
        public UserControl UI
        {
            get
            {
                if (_UI == null)
                    _UI = new sekwencerUI(this);
                return _UI; }
        }
        UserControl _UI;
        public List<Typ> wejście { get; set; }
        public Typ[] wyjście
        {
            get { return _wyjście; }set { _wyjście = value; }
        }
        Typ[] _wyjście;
        public Dictionary<string, string> ustawienia
        {
            get { return _ustawienia; }
        }
        Dictionary<string, string> _ustawienia;
        private float oktawy;
        public sekwencer()
        {
            wejście = new List<Typ>();
            _wyjście = new Typ[1];
            _wyjście[0] = new Typ();
            _ustawienia = new Dictionary<string, string>();
            _ustawienia.Add("oktawy", "0");
        }
        /*[DllImport("HelloWorldLib", CallingConvention = CallingConvention.StdCall)]
        extern unsafe static void generuj_nutę(float[][] wej, int pozycja, float[] fala);
        [DllImport("HelloWorldLib", CallingConvention = CallingConvention.StdCall)]
        extern unsafe static float suma(float* a, float* b);
        [DllImport("HelloWorldLib", CallingConvention = CallingConvention.StdCall)]
        extern unsafe static float sumab(float a, float b);*/
    
        public /*unsafe*/ void działaj(nuta o)
        {
            if (wyjście[0].DrógiModół != null)
            {
                if (Statyczne.gpgpu)
                {
                    //nutaC a = new nutaC();
                    float[] q={5,5,5};
                    float[] w={10,10,10};
                    float e;
                    /*System.Windows.MessageBox.Show(sumab(5, 20).ToString());
                    fixed (float* qq = &q[0])
                    {

                        fixed (float* ww = &w[0])
                        {
                            e = suma(qq, ww);
                        }
                    }*/
                    var daneWejściowe = new List<float[]>();
                    float[] daneNuty = { o.id, o.długość, (float)o.ilepróbek, o.los, o.opuznienie };
                    daneWejściowe.Add(daneNuty);
                    var tablicaDanychWejściowych = new float[daneWejściowe.Count, 32];
                    for (var i = 0; i < daneWejściowe.Count; i++)
                    {
                        for (var x = 0; x < daneWejściowe[i].Length; x++)
                            tablicaDanychWejściowych[i, x] = daneWejściowe[i][x];
                    }
                    //fixed (float* wskaźnikTablicyDanychTekstowych = &tablicaDanychWejściowych[0,0])
                    {

                    //generuj_nutę(daneWejściowe.ToArray(), 1, new float[0]);
                    }
                }
                //else
                {
                    o.ilepróbek = o.ilepróbekNaStarcie / Math.Pow(2, oktawy);
                    o.głośność =o.balans0=o.balans1= 1;
                    try
                    {
                        wyjście[0].DrógiModół.działaj(o);
                    }
                    catch { }
                }
            }
        }
        public void gpgpuGeneruj()
        {
            var dane = new List<int>();
            if(wyjście[0].DrógiModół != null)
            {

            }
        }
        public long symuluj(long p)
        {
            return wyjście[0].DrógiModół.symuluj(p);
        }
        public bool czyWłączone
        {
            get
            {
                if (wyjście[0].DrógiModół == null)
                    return false;
                else
                    return true;
            }
        }
    }
}
