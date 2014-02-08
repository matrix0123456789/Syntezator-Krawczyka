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
using System.Globalization;

namespace Syntezator_Krawczyka
{
    /// <summary>
    /// Interaction logic for KlawiaturaKomputeraUI.xaml
    /// </summary>
    public partial class sciezkaUI : UserControl
    {
        sciezka parent;
        Timer akttimer;
        public sciezkaUI(sciezka thi)
        {
            parent = thi;
            InitializeComponent();
            if(thi.kopia)
            {
                przyciskEdytuj.Visibility = Visibility.Collapsed;
            }
            if(thi.xml.Attributes.GetNamedItem("delay")!=null)
            {
                delay.Text=double.Parse(thi.xml.Attributes.GetNamedItem("delay").Value,CultureInfo.InvariantCulture).ToString();
            }
            akttimer = new Timer((object o) => { MainWindow.dispat.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate() { 
                aktModuły();
                nazwa.Content = "Ścieżka – "+parent.nazwa;
            }); }, null, 0, 1000);

        }
        void aktModuły()
        {
            lock (comboBox1)
            {
                comboBox1.Items.Clear();

                comboBox1.Items.Add(new ComboBoxItem());
                (comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1) as FrameworkElement).Tag = null;
                (comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1) as ComboBoxItem).Content = "(puste)";
                if (parent.sekw == null)
                {
                    comboBox1.SelectedItem = comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1);
                }
                foreach (var mod in Statyczne.otwartyplik.moduły)
                {
                    comboBox1.Items.Add(new ComboBoxItem());
                    (comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1) as FrameworkElement).Tag = mod.Value.sekw;
                    (comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1) as ComboBoxItem).Content = mod.Key;
                    if (parent.sekw == mod.Value.sekw)
                    {
                        comboBox1.SelectedItem = comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1);
                    }
                }
            }
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(comboBox1.SelectedItem!=null)
                parent.sekw = (comboBox1.SelectedItem as FrameworkElement).Tag as soundStart;
            }
            catch (NullReferenceException) { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var okno = new EdytorNut(parent);
            okno.Show();
        }
    }
}
