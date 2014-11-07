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
using NAudio.CoreAudioApi;
using Syntezator_Krawczyka.Synteza;
using System.Net.Sockets;

namespace Syntezator_Krawczyka
{
    /// <summary>
    /// Interaction logic for WyjścieDzwieku.xaml
    /// </summary>
    public partial class WyjścieDzwieku : Window
    {
        public WyjścieDzwieku()
        {
            InitializeComponent();
        }
        static WyjścieDzwieku okno = null;
        internal static void pokarz()
        {
            if (okno == null)
                okno = new WyjścieDzwieku();
            okno.akt();
            okno.Show();
        }
        void akt()
        {
            Wyjścia.Children.Clear();
            /* var kolecja = Device
             foreach (var x in kolecja)
             {
                 var el = new CheckBox();
                 el.Content = x.ToString();
                 Wyjścia.Children.Add(el);
             }*/
        }

        private void zmienLAN(object sender, TextChangedEventArgs e)
        {
            zmienLAN();
        }

        private void zmienLAN(object sender, RoutedEventArgs e)
        {
            zmienLAN();
        }
        void zmienLAN()
        {
            if (granie.LANSocket != null)
            {
                granie.LANSocket.Stop();
            }
            if (UdostLAN.IsChecked.Value)
            {
                try
                {
                    granie.LANSocket = new TcpListener(int.Parse(port.Text));
                }
                catch { }
                granie.LANSocket.Start();
                granie.LANSocket.BeginAcceptTcpClient((IAsyncResult a) =>
                {
                    var klij = granie.LANSocket.AcceptTcpClient();
                    var strum=klij.GetStream();
                    //strum.
                }, null);
            }
        }
    }
}
