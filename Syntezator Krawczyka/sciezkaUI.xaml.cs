﻿using System;
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
                foreach (var mod in MainWindow.otwartyplik.moduły)
                {
                    comboBox1.Items.Add(new ComboBoxItem());
                    (comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1) as FrameworkElement).Tag = (sekwencer)mod.Value["<sekwencer"];
                    (comboBox1.Items.GetItemAt(comboBox1.Items.Count - 1) as ComboBoxItem).Content = mod.Key;
                    if (parent.sekw == (sekwencer)mod.Value["<sekwencer"])
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
                parent.sekw = (comboBox1.SelectedItem as FrameworkElement).Tag as sekwencer;
            }
            catch (NullReferenceException exep) { }
        }
    }
}
