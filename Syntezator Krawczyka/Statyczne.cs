using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Syntezator_Krawczyka
{
    /// <summary>
    /// zawiera niektóre elementy, które są statyczne, a nie było dla nich lepszego miejsca
    /// </summary>
    class Statyczne
    {
        public static BufferedWaveProvider bufor=new BufferedWaveProvider(new WaveFormat((int)plik.Hz,1));
        public static WasapiOut WasapiWyjście = new WasapiOut(AudioClientShareMode.Shared, 100);
        public Statyczne()
        {

            WasapiWyjście.Init(bufor);
            WasapiWyjście.Play();
        }
    }
}
