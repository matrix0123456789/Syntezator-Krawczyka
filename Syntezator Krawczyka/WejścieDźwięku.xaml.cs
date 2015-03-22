using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Syntezator_Krawczyka
{
    /// <summary>
    /// Interaction logic for WejścieDźwięku.xaml
    /// </summary>
    public partial class WejścieDźwięku : UserControl
    {
        private NAudio.Wave.WaveInCapabilities info;

        public WaveIn wej;
        bool nagrywanie = false;

        List<float[,]> bufory;
        void wej_DataAvailable(object sender, WaveInEventArgs e)
        {
            var str = new MemoryStream(e.Buffer);
            var reader = new BinaryReader(str);
            if (nagrywanie)
            {
                var fala = new float[info.Channels, (e.BytesRecorded) / 2 / info.Channels];
                for (int i = 0; i < (e.BytesRecorded) / 2 / info.Channels; i++)
                {
                    for (byte k = 0; k < info.Channels; k++)
                        fala[k, i] = reader.ReadInt16() / 32768f;
                }
                bufory.Add(fala);

            }
            str.Position = 0;
            short pier = reader.ReadInt16();
            if (pier > 0)
                postęp.Value = pier;
            else
                postęp.Value = -pier;
        }
        public WejścieDźwięku(NAudio.Wave.WaveInCapabilities info, int nr)
        {
            InitializeComponent();

            this.info = info;
            Nazwa.Content = info.ProductName;
            wej = new WaveIn();
            wej.DeviceNumber = nr;

            wej.WaveFormat = new WaveFormat(48000, info.Channels);
            wej.StartRecording();
            wej.DataAvailable += wej_DataAvailable;
        }

        private void nagrywaj(object sender, RoutedEventArgs e)
        {
            if (nagrywanie)
            {
                (sender as Button).Content = "Nagrywaj";

                nagrywanie = false;//TODO do innego wątku
                var samp = new jedenSample();
                samp.sample = new sample(bufory);
                Statyczne.otwartyplik.sameSample.Add(samp);
            }
            else
            {
                (sender as Button).Content = "Zatrzymaj";
                bufory = new List<float[,]>();
                nagrywanie = true;
            }
        }

    }
}
