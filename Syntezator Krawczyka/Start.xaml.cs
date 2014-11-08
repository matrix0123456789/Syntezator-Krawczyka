using System;
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
            polecenia();
            InitializeComponent();
            ThreadPool.QueueUserWorkItem((a) => { Backup.czyśćStare(new TimeSpan(14, 0, 0, 0)); });//czyści backup starszy niż 14 dni

        }
        void polecenia()
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
            
        }
    }
}
