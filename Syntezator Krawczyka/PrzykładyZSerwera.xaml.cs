using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    /// Interaction logic for PrzykładyZSerwera.xaml
    /// </summary>
    public partial class PrzykładyZSerwera : UserControl
    {
        private Timer tim;
        [Bindable(true)]
        public event Action Wybrano;
        public PrzykładyZSerwera()
        {
            InitializeComponent();
            tim = new Timer(tiCall, null, 0, 60000);
        }

        private void tiCall(object state)
        {
            while (Statyczne.serwer == null)
                Thread.Sleep(10);
            wyswietlUtwory(Statyczne.serwer.pobierzUtwory("publiczne"));
        }

        internal void wyswietlUtwory(UtworySerwer utworySerwer)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, (ThreadStart)delegate()
            {
                if (Lista != null)
                {
                    Lista.Children.Clear();
                    if (utworySerwer != null)
                        foreach (var x in utworySerwer)
                        {
                            var ui = x.UI;
                            Lista.Children.Add(ui);
                            (ui as UtwórSerwerUI).Wybrano += () => { if (Wybrano != null) Wybrano(); };
                        }
                    else
                    {
                        var labBl = new Label();
                        labBl.Content = "Wystąpił błąd!";
                        Lista.Children.Add(labBl);
                    }
                }
            });
        }
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://jaebestudio.tk/musicstudio");
        }
    }
}
