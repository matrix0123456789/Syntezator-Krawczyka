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
using Microsoft.Win32;
using System.IO;
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
        public static Logowanie oknoLogowanie = null;
        Thread aktualizacjaOkna;
        /// <summary>
        /// Informuje, czy jest włączony trub debugowania (parametr /d przy uruchamianiu)
        /// </summary>
        static public bool debugowanie = false;
        public klawiaturaKomputera klawiatkompa1, klawiatkompa2;
        List<KlawiaturaMidi> klawiatMidi = new List<KlawiaturaMidi>();
        public MainWindow()
        {
            InitializeComponent();
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
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
            thi = this;
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
                else if (parametry[x] == "/f" || parametry[x] == "-f")
                {
                    x++;
                    plik.Hz = float.Parse(parametry[x]);
                    plik.kHz = plik.Hz / 1000;
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
                    var xKopia = x;
                    System.Threading.ThreadPool.QueueUserWorkItem((o) =>
                            { Statyczne.otwartyplik = new plik(parametry[xKopia]); });
                    otwarto = true;
                }
            }

            if (!otwarto)
            {
                System.Threading.ThreadPool.QueueUserWorkItem((o) =>
                           {
                               Statyczne.otwartyplik = new plik(Syntezator_Krawczyka.Properties.Resources.przyklad, true);
                           });
            }
            if (zamknij)
                App.Current.Shutdown();
            else
            {
                klawiatkompa1 = new klawiaturaKomputera(typKlawiaturyKomputera.dolna);
                klawiatkompa2 = new klawiaturaKomputera(typKlawiaturyKomputera.górna);
                for (int i = 0; i < NAudio.Midi.MidiIn.NumberOfDevices; i++)
                {
                    var k = new KlawiaturaMidi(i);
                    klawiatMidi.Add(k);
                    pokaz.Children.Add(k.UI);
                }
                if (debugowanie)
                {
                    var k = new KlawiaturaMidi();
                    klawiatMidi.Add(k);
                    pokaz.Children.Add(k.UI);
                }
                pokaz.Children.Add(klawiatkompa1.UI);
                pokaz.Children.Add(klawiatkompa2.UI);
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
            ushort ileDoGC = 0, ileDoKopii = 0;
            while (true)
            {
                ileDoGC++;
                if (ileDoGC > 50 && czyGC)
                {


                    GC.Collect(20, GCCollectionMode.Forced);
                    ileDoGC = 0;
                }
                else if (ileDoGC > 300)
                {


                    GC.Collect(10, GCCollectionMode.Optimized);
                    ileDoGC = 0;
                }
                ileDoKopii++;
                if (ileDoKopii > 600)
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\SyntezatorKrawczyka");
                        Statyczne.otwartyplik.zapisz(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\SyntezatorKrawczyka\\kopia" + DateTime.Now.ToFileTime() + ".synkra");
                        ileDoKopii = 0;
                    }
                    catch { ileDoKopii = 300; }
                }
                MainWindow.dispat.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, (ThreadStart)delegate()
                    {
                        if (debugowanie)
                            Title = Statyczne.bufor.BufferedBytes.ToString();
                        czas.Content = funkcje.sekundy(granie.graniePrzy - Statyczne.bufor.BufferedBytes / 4) + '/' + funkcje.sekundy(granie.granieMax);
                        suwak.Value = granie.graniePrzy - Statyczne.bufor.BufferedBytes / 4;
                        suwak.Maximum = granie.granieMax;
                        postęp.Value = granie.liczbaGenerowanychMax - granie.liczbaGenerowanych;
                        postęp.Maximum = granie.liczbaGenerowanychMax;
                        if (postęp.Value != 0)
                        {
                            if (postęp.Value / (double)granie.liczbaGenerowanychMax < 1)
                            {
                                pasekZadań.ProgressState = TaskbarItemProgressState.Normal;
                                postęp.Visibility = System.Windows.Visibility.Visible;
                            }
                            else
                            {
                                pasekZadań.ProgressState = TaskbarItemProgressState.None;
                                postęp.Visibility = System.Windows.Visibility.Collapsed;
                            }
                            pasekZadań.ProgressValue = postęp.Value / (double)granie.liczbaGenerowanychMax;
                        }
                        else
                        {
                            pasekZadań.ProgressState = TaskbarItemProgressState.None;
                            postęp.Visibility = System.Windows.Visibility.Collapsed;
                        }
                    });

                if (Statyczne.otwartyplik != null)
                {
                    var cou = Statyczne.otwartyplik.sciezki.Count;
                    lock (Statyczne.otwartyplik)
                    if ((ileDoKopii % 200 == 0 || ileScierzekWyswietla != cou))
                    {
                        MainWindow.dispat.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, (ThreadStart)delegate()
                            {


                                Statyczne.otwartyplik.sciezki.Sort();
                                foreach (var x in Statyczne.otwartyplik.sciezki)
                                {
                                    try
                                    {
                                        if (!pokaz.Children.Contains(x.UI))
                                            pokaz.Children.Add(x.UI);
                                    }
                                    catch (ArgumentException) { }
                                }
                                ileScierzekWyswietla = cou;

                            });
                    }
                }
                Thread.Sleep(100);
            }
        }
        public static int ileScierzekWyswietla = 0;
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
        private void buttonGraj_Click(object sender, RoutedEventArgs e)
        {
            granie.graniePrzy = 0;



            granie.generować[0] = false;
            granie.generować = new bool[1];
            granie.generować[0] = true;
            granie.liczbaGenerowanychMax = granie.liczbaGenerowanych = 0;
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
            granie.PlikDoZapisu = null;
            granie.granieMax = (int)długość;
            granie.wynik = new float[2, długość];
            List<nuta> lista = new List<nuta>();
            foreach (var x in Statyczne.otwartyplik.sciezki)
            {
                foreach (var nuta in x.nuty)
                {
                    nuta.sekw = x.sekw;
                    lista.Add(nuta);
                }
            }
            lista.Sort(Syntezator_Krawczyka.nuta.sortuj);
            granie.granieNuty = lista.ToArray();
            granie.graniePlay = true;
            granie.liczbaGenerowanych += granie.granieNuty.Length;
            granie.liczbaGenerowanychMax += granie.granieNuty.Length;
            foreach (var x in granie.granieNuty)
            {
                lock (granie.grają)
                {

                    var tabl = (nuta)x.Clone();
                    tabl.grajDo = long.MaxValue;
                    System.Threading.ThreadPool.QueueUserWorkItem((o) =>
                    {

                        if (((bool[])o)[0] && x.sekw != null)
                        {
                            x.sekw.działaj(tabl);
                            x.czyGotowe = true;
                            // granie.liveGraj();
                        }

                        lock (granie.liczbaGenerowanychBlokada)
                        {
                            granie.liczbaGenerowanych--;
                            //if (!granie.można && granie.liczbaGenerowanych == 0)

                            //granie.grajcale(false);
                        }
                    }, granie.generować);
                    var watek = new Thread(() => { var gen = granie.generować; while (granie.liveGraj() && gen[0]) { Thread.Sleep(1000); } });
                    watek.Start();

                }
            }
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                granie.liczbaGenerowanychMax = granie.liczbaGenerowanych = 0;
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
                granie.PlikDoZapisu = null;
                granie.wynik = new float[2, długość];
                foreach (var x in Statyczne.otwartyplik.sciezki)
                {
                    x.działaj();
                    //akt(null);
                }

                var dialog = new SaveFileDialog();
                dialog.Filter = "Plik muzyczny|*.wav;*.wave";
                dialog.ShowDialog();
                granie.PlikDoZapisu = dialog.FileName;
            }
            catch (Exception e1) { MessageBox.Show("Błąd przy zapisie dźwięku", e1.ToString()); }
        }

        public void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(((System.Windows.Input.KeyboardEventArgs)(e)).KeyboardDevice.GetHashCode().ToString() + "\n" + e.Device.GetHashCode().ToString() + "\n" + e.InputSource.GetHashCode().ToString());
            if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
            {
                if (e.Key == Key.S)
                    button4_Click(null, null);
                if (e.Key == Key.V)
                {
                    if (Clipboard.ContainsData("audio/x-syntezator-krawczyka-instrument"))
                        nowyInstrument.laduj((string)Clipboard.GetData("audio/x-syntezator-krawczyka-instrument"));

                }
                if (e.Key == Key.O)
                    button3_Click(null, null);
            }
            else
            {
                klawiatkompa1.klawisz(e, true);
                klawiatkompa2.klawisz(e, true);
            }
        }

        public void Window_KeyUp(object sender, KeyEventArgs e)
        {

            klawiatkompa1.klawisz(e, false);
            klawiatkompa2.klawisz(e, false);
        }
        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            granie.grają.Clear();
            granie.można = true;
            granie.graniePlay = false;

            granie.generować[0] = false;
            granie.generować = new bool[1];
            granie.generować[0] = true;
            granie.graniePrzy = 0;
            granie.wynik = null;
            granie.granieMax = 0;
            granie.granieNuty = null;
            granie.liczbaGenerowanych = 0;
            granie.liczbaGenerowanychMax = 0;
            Statyczne.bufor.ClearBuffer();
        }


        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //granie.t.Change(0, (long)(sender as Slider).Value);
        }
        public static bool gpgpu = false;

        private void buttonNowyInstrument_Click(object sender, RoutedEventArgs e)
        {
            var a = new nowyInstrument();
            a.Show();
        }
        private void buttonNowaScierzka_Click(object sender, RoutedEventArgs e)
        {
            Statyczne.otwartyplik.nowaScierzka();
        }
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (oknoLogowanie == null)
                oknoLogowanie = new Logowanie();
            oknoLogowanie.Show();
            oknoLogowanie.Activate();
        }

        private void suwak_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (suwakdziala)
            {
                if (granie.graniePrzy != (int)suwak.Value)
                    Statyczne.bufor.ClearBuffer();
                granie.graniePrzy = (int)suwak.Value;
            }
        }

        internal void zmianaLogowania(PolaczenieHTTP polaczenieHTTP)
        {
            if (polaczenieHTTP.zalogowano)
            {
                LogowanieTxt.Content = polaczenieHTTP.login;
                LogowanieTxt.ToolTip = "zalogowano jako " + polaczenieHTTP.login;
                LogowanieTxt.FontSize = 8;
            }
            else
                LogowanieTxt.Content = "Zaloguj";
            LogowanieTxt.FontSize = 12;
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {

            //if (e.Data.GetData("audio/x-syntezator-krawczyka-instrument")!=null)
            //            nowyInstrument.laduj((string)e.Data.GetData("audio/x-syntezator-krawczyka-instrument"));

        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            /*if(e.Data.GetData("audio/x-syntezator-krawczyka-instrument")==null)
            {
                e.Effects = DragDropEffects.None;
                

            }
            else if (e.Data.GetData("audio/x-syntezator-krawczyka-instrument").GetHashCode() == hashCodeDragAndDrop)
            {
                e.Effects = DragDropEffects.None;
                e.Data.SetData(null);
            }*/
        }
        static public int hashCodeDragAndDrop = 0;
        Boolean suwakdziala = false;
        private void suwakEnter(object sender, MouseEventArgs e)
        {
            suwakdziala = true;
        }

        private void suwakLeave(object sender, MouseEventArgs e)
        {
            suwakdziala = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var oknoEN = new EdytorNut();
            oknoEN.Show();
        }

        private void VST_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var dialog = new OpenFileDialog();
                dialog.Filter = "Wtyczka VST|*.dll";
                Jacobi.Vst.Core.VstCanDoHelper.ParseHostCanDo("S");
                new Jacobi.Vst.Framework.VstMidiProgram();
                dialog.ShowDialog();
                if (dialog.FileName != null)
                {
                    wtyczkaVST.test1(dialog.FileName);
                    wtyczkaVST.test2(dialog.FileName);
                    wtyczkaVST.test3(dialog.FileName);
                    new wtyczkaVST(dialog.FileName);
                }
            }
            catch (Exception e2) { MessageBox.Show(e2.ToString(), "Błąd ładowania wtyczki", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void DrumPad_Click(object sender, RoutedEventArgs e)
        {
            var okno = new DrumPad();
            okno.Show();
        }
        private void Kopia_Click(object sender, RoutedEventArgs e)
        {
            var okno = new Backup();
            okno.Show();
        }


        private void Metadane_click(object sender, RoutedEventArgs e)
        {

            var okno = new Metadane();
            okno.Show();
        }




    }
}