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
using System.Globalization;

namespace Syntezator_Krawczyka
{
    /// <summary>
    /// Interaction logic for KlawiaturaKomputeraUI.xaml
    /// </summary>
    public partial class sciezkaUI : UserControl, IDisposable
    {
        sciezka parent;
        Timer akttimer;
        Boolean gotowe = false;
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
            gotowe = true;
            akttimer = new Timer((object o) => { MainWindow.dispat.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate() { 
                aktModuły();
                nazwa.Content = "Ścieżka – "+parent.nazwa;
            }); }, null, 0, 1000);

        }
        int ilemod = 0;
        void aktModuły()
        {
            lock (comboBox1)
            {
                var cou = Statyczne.otwartyplik.moduły.Count;
                if (ilemod != cou)
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
                    ilemod = cou;
                }
            }
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem != null)
                {
                    parent.sekw = (comboBox1.SelectedItem as FrameworkElement).Tag as soundStart;
                    var atr = parent.xml.OwnerDocument.CreateAttribute("sound");
                    atr.Value = (string)(comboBox1.SelectedItem as ComboBoxItem).Content;
                    parent.xml.Attributes.Append(atr);
                    Statyczne.otwartyplik.zmiana();
                }
            }
            catch (NullReferenceException) { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var okno = new EdytorNut(parent);
            okno.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Statyczne.otwartyplik.duplikujScierzke(parent);
            Statyczne.otwartyplik.zmiana();
        }

        private void delay_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (gotowe)
            {
                try
                {
                    parent.xml.Attributes.GetNamedItem("delay").Value = (float.Parse(delay.Text).ToString(CultureInfo.InvariantCulture));
                    parent.delay = (int)(float.Parse(delay.Text) * 60 * plik.Hz / plik.tempo);
                }
                catch(System.FormatException)
                {
                    (sender as TextBox).Background = Brushes.Red;
                }
                    Statyczne.otwartyplik.zmiana();
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Clipboard.SetData("audio/x-syntezator-krawczyka-track",parent.xml.OuterXml);
        }


        private void MenuItem_Click_usuń(object sender, RoutedEventArgs e)
        {
            Statyczne.otwartyplik.sciezki.Remove(parent);
            if (Statyczne.otwartyplik.scieżkiZId.ContainsKey(parent.nazwa))
                Statyczne.otwartyplik.scieżkiZId.Remove(parent.nazwa);
            parent.xml.ParentNode.RemoveChild(parent.xml);
            (Parent as WrapPanel).Children.Remove(this);
            Statyczne.otwartyplik.zmiana();
        }

        public void Dispose()
        {
            if (akttimer != null)
                akttimer.Dispose();
        }
    }
}
