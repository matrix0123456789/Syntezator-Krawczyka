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
using Syntezator_Krawczyka.Synteza;
using System.Windows.Threading;
using System.Threading;
using System.Security.Principal;
using System.Diagnostics;
using System.Windows.Shell;
using System.Xml;
namespace Syntezator_Krawczyka
{

    /// <summary lang="pl">
    /// Główne okno, w danym procesie powinno być otwarte tylko jedno.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Przedstawia dane, które są aktualnie otwarte.
        /// </summary>
        //static public plik otwartyplik = new plik();
        static Random rand = new Random();
        //byte[] sinus;
        /// <summary>
        /// Referencja na ten obiekt, by dostać się łatwo do elementów okna.
        /// </summary>
        static public MainWindow thi;
        public sekwencer głównySekwencer;
        static public Dispatcher dispat;
        static public bool czyGC = false;

        Thread aktualizacjaOkna;
        /// <summary>
        /// Informuje, czy jest włączony trub debugowania (parametr /d przy uruchamianiu)
        /// </summary>
        static public bool debugowanie = false;
        klawiaturaKomputera klawiatkompa;
        public MainWindow()
        {
            try
            {
                new Statyczne();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Brakuje pliku NAudio.dll, bez którego program nie może odtwarzać dźwięku.", "Brak pliku NAudio.dll", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            thi = this;
            InitializeComponent();
            dispat = Dispatcher;
            string[] parametry = Environment.GetCommandLineArgs();
            bool otwarto = false;
            bool zamknij = false;
            for (var x = 1; x < parametry.Length; x++)
            {
                if (parametry[x] == "/d" || parametry[x] == "-d")
                {
                    debugowanie = true;
                    gpgpu = true;
                }
                else if (parametry[x] == "/z" || parametry[x] == "-z")
                {
                    zamknij = true;
                }
                else if (parametry[x] == "/gc" || parametry[x] == "-gc")
                {
                    czyGC = true;
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
                    Statyczne.otwartyplik = new plik(parametry[x]);
                    otwarto = true;
                }
            }

            if(!otwarto)
                Statyczne.otwartyplik = new plik(Syntezator_Krawczyka.Properties.Resources.przyklad, true);

            if (zamknij)
                App.Current.Shutdown();
            else
            {
                klawiatkompa = new klawiaturaKomputera();
                pokaz.Children.Add(klawiatkompa.UI);
                //aktualizacjaOkna = new Timer(akt, null, 10, 100);
                aktualizacjaOkna = new Thread(akt);
                aktualizacjaOkna.Start();
                //System.IO.StreamReader sa = new System.IO.StreamReader("C:\\Users\\Public\\Documents\\sinusoida.wav");


                //System.IO.BinaryReader read = new System.IO.BinaryReader(sa.BaseStream);
                //sinus = read.ReadBytes((int)read.BaseStream.Length);

                if (Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".synkra", false) == null)
                {
                    if (MessageBoxResult.Yes == MessageBox.Show("Czy chcesz skojarzyć pliki .synkra z tym programem?", "Skojarzenie plików", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No))
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
        void akt(object o)
        {
            ushort ileDoGC = 0,ileDoKopii=0;
            while (true)
            {
                ileDoGC++;
                if (ileDoGC > 50&&czyGC||ileDoGC>300)
                {

                    
                        GC.Collect(20, GCCollectionMode.Forced);
                    ileDoGC = 0;
                }
                ileDoKopii++;
                if(ileDoKopii>600)
                {
                    System.IO.Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\SyntezatorKrawczyka");
                    Statyczne.otwartyplik.zapisz(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\SyntezatorKrawczyka\\kopia"+DateTime.Now.ToFileTime()+".synkra");
                    ileDoKopii = 0;
                }
                MainWindow.dispat.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, (ThreadStart)delegate()
                    {
                        if (debugowanie)
                            Title = granie.liczbaGenerowanych.ToString() + " > " + granie.grają.Count.ToString() + " o=" + granie.o.ToString() + "; " + ileDoGC.ToString();
                        postęp.Value = granie.liczbaGenerowanychMax - granie.liczbaGenerowanych;
                        postęp.Maximum = granie.liczbaGenerowanychMax;
                        if (postęp.Value != 0)
                        {
                            if (postęp.Value / (double)granie.liczbaGenerowanychMax<1)
                            pasekZadań.ProgressState = TaskbarItemProgressState.Normal;
                            else
                                pasekZadań.ProgressState = TaskbarItemProgressState.None;

                            pasekZadań.ProgressValue = postęp.Value / (double)granie.liczbaGenerowanychMax;
                        }
                        foreach (var x in Statyczne.otwartyplik.sciezki)
                        {
                            try
                            {
                                if (!pokaz.Children.Contains(x.UI))
                                    pokaz.Children.Add(x.UI);
                            }
                            catch (ArgumentException) { }
                        }
                    });
                Thread.Sleep(100);
            }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            GC.Collect(2, GCCollectionMode.Forced);

        }

       
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try { granie.o = int.Parse(((TextBox)sender).Text); }
            catch { }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Wszystkie pliki z nutami|*.mid;*.midi;*.xml;*.synkra|Plik XML|*.xml|Plik Syntezatora Krawczyka|*.synkra|Plik MIDI|*.mid;*.midi|Wszystkie Pliki|*.*";
            dialog.ShowDialog();
            string[] explode = dialog.FileName.Split('.');
            if (explode.Last() == "mid" || explode.Last() == "midi")
                Statyczne.otwartyplik = new plikmidi(dialog.FileName);
            else
                Statyczne.otwartyplik = new plik(dialog.FileName);
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            Statyczne.otwartyplik.zapisz();
        }
        private void button5_Click(object sender, RoutedEventArgs e)
        {

            Thread.Sleep(10000);
        }

        private void Window_Closed(object sender, EventArgs e)
        {

            aktualizacjaOkna.Abort();
            App.Current.Shutdown();
        }
        private void button6_Click(object sender, RoutedEventArgs e)
        {
            foreach (var x in Statyczne.otwartyplik.sciezki)
            {
                x.działaj();
                //akt(null);
            }
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            granie.można = false;
            granie.grają.Clear();
            long długość = 0;
            foreach (var x in Statyczne.otwartyplik.sciezki)
            {
                if (x.sekw != null)
                {
                    long długośćStart = 0;
                    for (var i = 0; i < x.nuty.Count; i++)
                    {

                        if (długośćStart < x.nuty[i].opuznienie + x.nuty[i].długość)
                            długośćStart = x.nuty[i].opuznienie + x.nuty[i].długość;

                    }

                    long długośćTeraz = x.sekw.symuluj(długośćStart);
                    if (długośćTeraz > długość)
                        długość = długośćTeraz;
                }
            }
            granie.wynik = new float[długość];
            foreach (var x in Statyczne.otwartyplik.sciezki)
            {
                x.działaj();
                //akt(null);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(((System.Windows.Input.KeyboardEventArgs)(e)).KeyboardDevice.GetHashCode().ToString() + "\n" + e.Device.GetHashCode().ToString() + "\n" + e.InputSource.GetHashCode().ToString());
            if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
            {
                if (e.Key == Key.S)
                    button4_Click(null, null);
                if (e.Key == Key.O)
                    button3_Click(null, null);
            }
            else klawiatkompa.klawisz(e, true);
        }
        
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            
             klawiatkompa.klawisz(e,false);
        }
        private void button8_Click(object sender, RoutedEventArgs e)
        {
            granie.grają.Clear();
        }


        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            granie.t.Change(0, (long)(sender as Slider).Value);
        }
        public static bool gpgpu = false;

        private void buttonNowyInstrument_Click(object sender, RoutedEventArgs e)
        {
            if(Statyczne.otwartyplik!=null)
            {
                
                var xml = new XmlDocument();
                xml.LoadXml(Properties.Resources.przyklad);
                var soundList=xml.GetElementsByTagName("sound");
                var sound=soundList[0];
                var id = sound.Attributes.GetNamedItem("id").Value;
                for (var i = 1; true; i++)
                {
                    if (!Statyczne.otwartyplik.moduły.ContainsKey(id + i.ToString()))
                    {
                        id = id + i.ToString();
                        break;
                    }

                }
                sound.Attributes.GetNamedItem("id").Value = id;
                var klon=funkcje.klonujXML(Statyczne.otwartyplik.xml, sound);
                Statyczne.otwartyplik.xml.GetElementsByTagName("file")[0].AppendChild(klon);
                

                Statyczne.otwartyplik.dekoduj(soundList);//poprawić na nową referencję
            }
        }
    }
}