using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Syntezator_Krawczyka
{
    /// <summary>
    /// Interaction logic for Nagrywanie.xaml
    /// </summary>
    public partial class Nagrywanie : Window
    {
        public Nagrywanie()
        {
            InitializeComponent();
            rysujWejścia();
        }
        void rysujWejścia()
        {
            for(int i=0;i<WaveIn.DeviceCount;i++)
            {
                var info = WaveIn.GetCapabilities(i);
                var el = new WejścieDźwięku(info, i);
                lista.Children.Add(el);
            }
        }
    }
}
