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
        static public plik otwartyplik = new plik();
        static Random rand = new Random();
        //byte[] sinus;
        /// <summary>
        /// Referencja na ten obiekt, by dostać się łatwo do elementów okna.
        /// </summary>
        static public MainWindow thi;
        public sekwencer głównySekwencer;
        static public Dispatcher dispat;
        
        Timer aktualizacjaOkna;
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
            for (var x = 1; x < parametry.Length; x++)
            {
                if (parametry[x] == "/d" || parametry[x] == "-d")
                {
                    debugowanie = true;
                }
                else if (!otwarto)
                {
                    otwartyplik = new plik(parametry[1]);
                    otwarto = true;
                }
            }
            klawiatkompa = new klawiaturaKomputera();
            pokaz.Children.Add(klawiatkompa.UI);
            aktualizacjaOkna = new Timer(akt, null, 10, 100);
            //System.IO.StreamReader sa = new System.IO.StreamReader("C:\\Users\\Public\\Documents\\sinusoida.wav");


            //System.IO.BinaryReader read = new System.IO.BinaryReader(sa.BaseStream);
            //sinus = read.ReadBytes((int)read.BaseStream.Length);

            if (Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".synkra", false) == null)
            {
                if (MessageBoxResult.Yes == MessageBox.Show("Czy chcesz skojarzyć pliki .synkra z tym programem?", "Skojarzenie plików", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No))
                {
                    if ((new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator))
                        skojarzPliki();
                }
            }

            for (var i = 0; i < NAudio.Midi.MidiIn.NumberOfDevices; i++)
            {
                //var info = NAudio.Midi.MidiIn.DeviceInfo(i);
                var midiwejście = new MidiIn(i);
            }
            
        }
        void akt(object o)
        {
            MainWindow.dispat.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, (ThreadStart)delegate()
            {
                if (debugowanie)
                    Title = granie.liczbaGenerowanych.ToString() + " > " + granie.grają.Count.ToString() + " o=" + granie.o.ToString();
                postęp.Value = granie.liczbaGenerowanychMax - granie.liczbaGenerowanych;
                postęp.Maximum = granie.liczbaGenerowanychMax;

                foreach (var x in otwartyplik.sciezki)
                {
                    try
                    {
                        if (!pokaz.Children.Contains(x.UI))
                            pokaz.Children.Add(x.UI);
                    }
                    catch (ArgumentException) { }
                }
            });
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Statyczne.bufor.ClearBuffer();

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            /*sekwencer mod1=new sekwencer();
            oscylator mod2 = new oscylator();
            granie mod3 = new granie();
            mod1.wyjście[0].DrógiModół = mod2;
            mod2.wyjście[0].DrógiModół = mod3;
            pokaz.Children.Add(mod1.UI);
            pokaz.Children.Add(mod2.UI);
            pokaz.Children.Add(mod3.UI);*/
            otwartyplik = new plik(Syntezator_Krawczyka.Properties.Resources.przyklad, true);
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
                otwartyplik = new plikmidi(dialog.FileName);
            else
                otwartyplik = new plik(dialog.FileName);
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            otwartyplik.zapisz();
        }
        private void button5_Click(object sender, RoutedEventArgs e)
        {

            Thread.Sleep(10000);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }
        private void button6_Click(object sender, RoutedEventArgs e)
        {
            foreach (var x in otwartyplik.sciezki)
                x.działaj();
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            granie.można = false;
            foreach (var x in otwartyplik.sciezki)
                x.działaj();
            granie.grajcale(false);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(((System.Windows.Input.KeyboardEventArgs)(e)).KeyboardDevice.GetHashCode().ToString() + "\n" + e.Device.GetHashCode().ToString() + "\n" + e.InputSource.GetHashCode().ToString());
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            granie.grają.Clear();
        }
        /// <summary>
        /// Tworzy informacje o plikach *.synkra w rejestrze systemowym.
        /// </summary>
        /// <remarks>Wymaga uprawnień administratora.</remarks>
        void skojarzPliki()
        {

            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(".synkra");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\.synkra", "", "PlikSyntezatoraKrawczyka");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\.synkra", "Content Type", "audio/x-syntezator-krawczyka");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\.synkra", "application", "syntezator krawczyka.exe");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(".synkra\\OpenWithList");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\.synkra\\OpenWithList", "a", "syntezator krawczyka.exe");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(".synkra\\OpenWithList\\syntezator krawczyka.exe");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(".synkra");

            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\synkra", "", "URL:PlikSyntezatoraKrawczyka");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\synkra", "URL Protocol", "");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\synkra", "application", "syntezator krawczyka.exe");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("synkra\\OpenWithList");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\synkra\\OpenWithList", "a", "syntezator krawczyka.exe");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("synkra\\OpenWithList\\syntezator krawczyka.exe");


            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.synkra\\OpenWithList", "a", "syntezator krawczyka.exe");
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.synkra", "application", "syntezator krawczyka.exe");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("applications\\syntezator krawczyka.exe");
            //Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("applications\\syntezator krawczyka.exe\\DefaultIcon");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("applications\\syntezator krawczyka.exe\\shell\\open\\command");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\applications\\syntezator krawczyka.exe\\shell\\open\\command", "", "\"" + System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName + "\" \"%1\"");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("applications\\syntezator krawczyka.exe\\shell\\Graj\\command");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\applications\\syntezator krawczyka.exe\\shell\\Graj\\command", "", "\"" + System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName + "\" -p \"%1\"");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("applications\\syntezator krawczyka.exe\\SupportedTypes");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\applications\\syntezator krawczyka.exe\\SupportedTypes", ".synkra", "");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("MIME\\Database\\Content Type\\audio/x-syntezator-krawczyka");//{E436EB83-524F-11CE-9F53-0020AF0BA770}, {A82E50BA-8E92-41eb-9DF2-433F50EC2993}
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\MIME\\Database\\Content Type\\audio/x-syntezator-krawczyka", "Extension", ".synkra");//HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Wow6432Node\CLSID\{083863F1-70DE-11D0-BD40-00A0C911CE86}\Instance\{11A947C3-BABC-466E-A678-1FFEC95EB2F8} sprawdzić
            //HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations dodać
            //{cd3afa76-b84f-48f0-9393-7edc34128127} sprawdzić

            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("PlikSyntezatoraKrawczyka");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\PlikSyntezatoraKrawczyka", "", "Plik programu SYntezator Krawczyka");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\PlikSyntezatoraKrawczyka", "FriendlyTypeName", "Plik programu SYntezator Krawczyka");

            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("PlikSyntezatoraKrawczyka\\open");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("PlikSyntezatoraKrawczyka\\open\\command");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\PlikSyntezatoraKrawczyka\\open\\command", "", "\"" + System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName + "\" \"%1\"");

            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("PlikSyntezatoraKrawczyka\\Graj");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("PlikSyntezatoraKrawczyka\\Graj\\command");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\PlikSyntezatoraKrawczyka\\Graj\\command", "", "\"" + System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName + "\" \"%1\" -p");

            Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\App Paths\\syntezator krawczyka.exe");
            Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\App Paths\\syntezator krawczyka.exe", "", System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            granie.t.Change(0, (long)(sender as Slider).Value);
        }
    }
}