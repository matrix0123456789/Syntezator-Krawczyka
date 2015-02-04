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
using System.Threading;
using Syntezator_Krawczyka.Synteza;
namespace Syntezator_Krawczyka
{
    /// <summary>
    /// Interfejs graiczny dla
    /// <see cref="KlawiaturaMidi"/>
    /// </summary>
    public partial class KlawiaturaMidiUI : UserControl, IDisposable
    {
        KlawiaturaMidi parent;
        Timer akttimer;
        string nazwa;
        public KlawiaturaMidiUI(KlawiaturaMidi thi, string nazwa)
        {
            parent = thi;
            InitializeComponent();
            this.nazwa = nazwa;
            label1.Content = "Klawiatura MIDI - "+nazwa;
            akttimer = new Timer((object o) => { MainWindow.dispat.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate() { aktModuły(); ; }); }, null, 0, 1000);

        }
        void aktModuły()
        {
            if (Statyczne.otwartyplik == null)
                return;
            lock (comboBox1)
            {
                comboBox1.Items.Clear();
                foreach (var mod in Statyczne.otwartyplik.moduły)
                {
                    var chil = new ComboBoxItem();
                    (chil as FrameworkElement).Tag = mod.Value.sekw;
                    (chil as ComboBoxItem).Content = mod.Key;
                    comboBox1.Items.Add(chil);
                    if (parent.sekw == mod.Value.sekw)
                    {
                        comboBox1.SelectedItem = chil;
                    }
                }
            }
            foreach (WrapPanel x in przedzialy.Children)
            {
                lock (x)
                {
                    var el = (x.Children[4] as ComboBox);
                    el.Items.Clear();
                    foreach (var mod in Statyczne.otwartyplik.moduły)
                    {
                        var chil = new ComboBoxItem();

                        (chil as FrameworkElement).Tag = mod.Value.sekw;
                        (chil as ComboBoxItem).Content = mod.Key;
                        el.Items.Add(chil);
                        if (parent.sekw == mod.Value.sekw)
                        {
                            comboBox1.SelectedItem = chil;
                        }
                    }
                }
            }
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(comboBox1.SelectedItem!=null)
            {
                parent.sekw = (comboBox1.SelectedItem as FrameworkElement).Tag as soundStart;
            }
        }
        //list
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var przedzial = new Syntezator_Krawczyka.KlawiaturaMidi.przedział();
            parent.przedziały.Add(przedzial);
            var graficzny=new WrapPanel();
            //graficzny.Height = 36;
            var comboOktMin = new ComboBox();
            comboOktMin.ToolTip = "Oktawa, w której jest nuta, od której zaczyna się śledzenie";
            comboOktMin.Height = 25;
            comboOktMin.Width = 50;
            //comboOktMin.Margin = new Thickness(0, 0, 0, 0);
            for (sbyte i = -4; i < 5; i++)
            {
                var item = new ComboBoxItem();
                item.Tag = i;
                item.Content=  i.ToString();
                comboOktMin.Items.Add(item);
            }
            comboOktMin.SelectedIndex = 4;
            var comboOktMax = new ComboBox();
            comboOktMax.ToolTip = "Oktawa, w której jest nuta, na której kończy się śledzenie";
            comboOktMax.Height = 25;
            comboOktMax.Width = 50;
            //comboOktMin.Margin = new Thickness(75, 0, 0, 0);
            for (sbyte i = -4; i < 5; i++)
            {
                var item = new ComboBoxItem();
                item.Tag = i;
                item.Content  = i.ToString();
                comboOktMax.Items.Add(item);
            }
            comboOktMax.SelectedIndex = 4;
            var comboTonMin = new ComboBox();
            comboTonMin.Height = 25;
            comboTonMin.Width = 60;
            //comboOktMin.Margin = new Thickness(30, 0, 0, 0);
            for(byte i=0;i<Statyczne.nazwyDźwięków.Length;i++)
            {
                var item = new ComboBoxItem();
                item.Tag=  i;
               // item.Name = i.ToString();
                item.Content = Statyczne.nazwyDźwięków[i];
                comboTonMin.Items.Add(item);
            }
            comboTonMin.SelectedIndex = 0;
            var comboTonMax = new ComboBox();
            comboTonMax.Height = 25;
            comboTonMax.Width = 60;
            //comboOktMin.Margin = new Thickness(105, 0, 0, 0);
            for (byte i = 0; i < Statyczne.nazwyDźwięków.Length; i++)
            {
                var item = new ComboBoxItem();
                item.Tag = i;
                //item.Name = i.ToString();
                item.Content = Statyczne.nazwyDźwięków[i];
                comboTonMax.Items.Add(item);
            }
            comboTonMax.SelectedIndex = 11;
            graficzny.Children.Add(comboOktMin);
            graficzny.Children.Add(comboTonMin);
            graficzny.Children.Add(comboOktMax);
            graficzny.Children.Add(comboTonMax);
            var comboSekw = new ComboBox();
            comboSekw.Height = 25;
            comboSekw.Width = 100;
            //comboOktMin.Margin = new Thickness(150, 0, 0, 0);
            graficzny.Children.Add(comboSekw);
            przedzialy.Children.Add(graficzny);
        }
        public void Dispose()
        {
            if (akttimer != null)
                akttimer.Dispose();
        }
    }
}
