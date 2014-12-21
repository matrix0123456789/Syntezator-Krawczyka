﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
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
    /// Interaction logic for Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        public Start()
        {
            thi = this;
          //
            try
            {
                new Statyczne();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Brakuje pliku NAudio.dll, bez którego program nie może odtwarzać dźwięku.", "Brak pliku NAudio.dll", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd ładowania biblioteki NAudio.dll, bez której program nie może odtwarzać dźwięku.", "Błąd pliku NAudio.dll", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show(ex.ToString());
            }
            if (polecenia()) { 
            InitializeComponent();
            if (Syntezator_Krawczyka.Properties.Settings.Default.OstatnioOtwarte != null)
                for (var i = Syntezator_Krawczyka.Properties.Settings.Default.OstatnioOtwarte.Count - 1; i >= 0 && i >= Syntezator_Krawczyka.Properties.Settings.Default.OstatnioOtwarte.Count-20; i--)
                {
                    var lab = new Label();
                    var str = Syntezator_Krawczyka.Properties.Settings.Default.OstatnioOtwarte[i];
                    lab.ToolTip=str;
                    lab.Content=str.Substring(str.LastIndexOfAny(new char[]{'/','\\'}));
                    lab.MouseLeftButtonDown += uruchomOstatnie;
                    OstOtw.Children.Add(lab);

                }
        }
            ThreadPool.QueueUserWorkItem((a) => { Backup.czyśćStare(new TimeSpan(14, 0, 0, 0)); });//czyści backup starszy niż 14 dni

        }

        void uruchomOstatnie(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var main = new MainWindow();

                string[] explode = ((string)(sender as Label).ToolTip).Split('.');
                    if (explode.Last() == "mid" || explode.Last() == "midi")
                        Statyczne.otwartyplik = new plikmidi((string)(sender as Label).ToolTip);
                    else
                        Statyczne.otwartyplik = new plik((string)(sender as Label).ToolTip);
                    main.Show();
                
            }
            catch (Exception e2) { MessageBox.Show(e2.ToString(), "Błąd", MessageBoxButton.OK, MessageBoxImage.Error); }
            Close();
        }
        bool polecenia()
        {

            string[] parametry = Environment.GetCommandLineArgs();
            bool otwarto = false;
            bool zamknij = false;
            for (var x = 1; x < parametry.Length; x++)
            {
                if (parametry[x] == "/d" || parametry[x] == "-d")
                {
                    Statyczne.debugowanie = true;
                    Statyczne.gpgpu = true;
                }
                else if (parametry[x] == "/z" || parametry[x] == "-z")
                {
                    zamknij = true;
                }
                else if (parametry[x] == "/f" || parametry[x] == "-f")
                {
                    x++;
                    plik.Hz = float.Parse(parametry[x]);
                    plik.kHz = plik.Hz / 1000;
                }
                else if (parametry[x] == "/gc" || parametry[x] == "-gc")
                {
                    MainWindow.czyGC = true;
                }
                else if (parametry[x] == "/p" || parametry[x] == "-p")
                {//graj od razu
                }
                else if (parametry[x] == "/s" || parametry[x] == "-s")
                {
                    if ((new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator))
                        Statyczne.skojarzPliki();
                }
                else if (!otwarto)
                {
                    var xKopia = x;

                    try
                    {
                        var main = new MainWindow();
                        main.Show();
                    }
                    catch (Exception e2) { MessageBox.Show(e2.ToString(), "Błąd", MessageBoxButton.OK, MessageBoxImage.Error); }
wtyczkaVST.wndprocStart(); 
                    System.Threading.ThreadPool.QueueUserWorkItem((o) =>
                    { Statyczne.otwartyplik = new plik(parametry[xKopia]); });
                    otwarto = true;
                    Close();
                }
            }

            if (!otwarto)
            {

            }
            if (zamknij)
                App.Current.Shutdown();
            else
            {

                //System.IO.StreamReader sa = new System.IO.StreamReader("C:\\Users\\Public\\Documents\\sinusoida.wav");


                //System.IO.BinaryReader read = new System.IO.BinaryReader(sa.BaseStream);
                //sinus = read.ReadBytes((int)read.BaseStream.Length);

                if (Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".jms", false) == null)
                {
                    if (MessageBoxResult.Yes == MessageBox.Show("Czy chcesz skojarzyć pliki .jms z tym programem?", "Skojarzenie plików", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No))
                    {
                        if ((new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator))
                            Statyczne.skojarzPliki();
                        else
                        {
                            ProcessStartInfo startInfo = new ProcessStartInfo();
                            startInfo.UseShellExecute = true;
                            startInfo.WorkingDirectory = Environment.CurrentDirectory;
                            startInfo.FileName = System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName;
                            startInfo.Verb = "runas";
                            startInfo.Arguments = "/s /z";
                            try
                            {
                                Process p = Process.Start(startInfo);
                            }
                            catch (System.ComponentModel.Win32Exception)
                            {
                                MessageBox.Show("Nie udało się skojażyć plików.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }

                for (var i = 0; i < NAudio.Midi.MidiIn.NumberOfDevices; i++)
                {
                    //var info = NAudio.Midi.MidiIn.DeviceInfo(i);
                    var midiwejście = new MidiIn(i);
                }
            }
            return !otwarto;
        }

        private void Pusty_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var main = new MainWindow();
                Statyczne.otwartyplik = new plik(Syntezator_Krawczyka.Properties.Resources.przyklad, true);
                main.Show();
            }
            catch (Exception e2) { MessageBox.Show(e2.ToString(), "Błąd", MessageBoxButton.OK, MessageBoxImage.Error); }
            Close();

        }

        private void OtwórzClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var main = new MainWindow();
                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.Filter = "Wszystkie pliki z nutami|*.mid;*.midi;*.xml;*.jms|Plik XML|*.xml|Plik Jaebe Music Studio|*.jms|Plik MIDI|*.mid;*.midi|Wszystkie Pliki|*.*";
                dialog.ShowDialog();
                if (dialog.FileName != null)
                {
                    string[] explode = dialog.FileName.Split('.');
                    if (explode.Last() == "mid" || explode.Last() == "midi")
                        Statyczne.otwartyplik = new plikmidi(dialog.FileName);
                    else
                        Statyczne.otwartyplik = new plik(dialog.FileName);
                    main.Show();
                }
            }
            catch (Exception e2) { MessageBox.Show(e2.ToString(), "Błąd", MessageBoxButton.OK, MessageBoxImage.Error); }
            Close();
        }

        public static Window thi;

        private void WyczyśćListę_Click(object sender, RoutedEventArgs e)
        {
            Syntezator_Krawczyka.Properties.Settings.Default.OstatnioOtwarte.Clear();
            Syntezator_Krawczyka.Properties.Settings.Default.Save();
            OstOtw.Children.Clear();
        }
    }
}