using NAudio.Midi;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Syntezator_Krawczyka
{
    /// <summary>
    /// Interaction logic for InstrumentMidiUI.xaml
    /// </summary>
    public partial class InstrumentMidiUI : UserControl
    {
        InstrumentMidi ParentNode;
        public InstrumentMidiUI(InstrumentMidi p)
        {
            InitializeComponent();
            ParentNode = p;
            for(var i=0;i<MidiOut.NumberOfDevices;i++)
            {
                var info=MidiOut.DeviceInfo(i);
                urządzenie.Items.Add(info.ProductName);
            }
            urządzenie.SelectedIndex = ParentNode.nrUrządzenia;
        }

        private void instrument_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ParentNode.instrument = byte.Parse(instrument.Text);
            }
            catch { }
        }

        private void kanał_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ParentNode.kanał = byte.Parse(kanał.Text);
            }
            catch { }

        }

        private void urządzenie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ParentNode.nrUrządzenia = urządzenie.SelectedIndex;
        }

    }
}
