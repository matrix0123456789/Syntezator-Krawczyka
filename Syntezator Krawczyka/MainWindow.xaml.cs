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
    public partial class MainWindow : Window, IDisposable
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
        public klawiaturaKomputera klawiatkompa1, klawiatkompa2;
        List<KlawiaturaMidi> klawiatMidi = new List<KlawiaturaMidi>();
        public MainWindow()
        {
            // var test = new Test();
            // test.Show();
            InitializeComponent();
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            PolaczenieHTTP.zmianaLogowania += zmianaLogowania;
            thi = this;
            dispat = Dispatcher; klawiatkompa1 = new klawiaturaKomputera(typKlawiaturyKomputera.dolna);
            klawiatkompa2 = new klawiaturaKomputera(typKlawiaturyKomputera.górna);
            for (int i = 0; i < NAudio.Midi.MidiIn.NumberOfDevices; i++)
            {
                var k = new KlawiaturaMidi(i);
                klawiatMidi.Add(k);
                pokazŚcie.Children.Add(k.UI);
            }
            if (Statyczne.debugowanie)
            {
                var k = new KlawiaturaMidi();
                klawiatMidi.Add(k);
                pokazŚcie.Children.Add(k.UI);
            }
            pokazŚcie.Children.Add(klawiatkompa1.UI);
            pokazŚcie.Children.Add(klawiatkompa2.UI);
            //aktualizacjaOkna = new Timer(akt, null, 10, 100);
            aktualizacjaOkna = new Thread(akt);
            aktualizacjaOkna.Start();
            zmianaLogowania(Statyczne.serwer);
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
                        Statyczne.otwartyplik.zapisz(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\SyntezatorKrawczyka\\kopia" + DateTime.Now.ToFileTime() + ".jms", false);
                        ileDoKopii = 0;
                    }
                    catch { ileDoKopii = 300; }
                }
                MainWindow.dispat.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, (ThreadStart)delegate()
                    {
                        if(!BoxTempo.IsFocused)
                        BoxTempo.Text = plik.tempo.ToString();
                        if (Statyczne.debugowanie)
                            Title = Statyczne.bufor.BufferedBytes.ToString();
                        czas.Content = funkcje.sekundy(granie.graniePrzy - Statyczne.bufor.BufferedBytes / 4) + '/' + funkcje.sekundy(granie.granieMax);
                        var pozycjaSuwaka = granie.graniePrzy - Statyczne.bufor.BufferedBytes / 4;

                        suwak.Tag = pozycjaSuwaka;
                        suwak.Value = pozycjaSuwaka;
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
                                            if (!pokazŚcie.Children.Contains(x.UI))
                                                pokazŚcie.Children.Add(x.UI);
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
            dialog.Filter = "Wszystkie pliki z nutami|*.mid;*.midi;*.xml;*.jms|Plik XML|*.xml|Plik Jaebe Music Studio|*.jms|Plik MIDI|*.mid;*.midi|Wszystkie Pliki|*.*";
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
        private void button4b_Click(object sender, RoutedEventArgs e)
        {
            if (Statyczne.otwartyplik.URL != null)
                Statyczne.otwartyplik.zapisz(Statyczne.otwartyplik.URL);
            else
                Statyczne.otwartyplik.zapisz();
        }
        private void button5_Click(object sender, RoutedEventArgs e)
        {

            Thread.Sleep(10000);
        }

        private void Window_Closed(object sender, EventArgs e)
        {

            aktualizacjaOkna.Abort();
            Environment.Exit(0);
            App.Current.Shutdown();
        }
        private void buttonGraj_Click(object sender, RoutedEventArgs e)
        { Statyczne.otwartyplik.grajStart(); }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            granie.bity = 16;
            Statyczne.otwartyplik.generuj();
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

           // granie.generować[0] = false;
            //granie.generować = new bool[1];
           // granie.generować[0] = true;
           // granie.graniePrzy = 0;
            //granie.wynik = null;
            //granie.granieMax = 0;
           // granie.granieNuty = null;
            //granie.liczbaGenerowanych = 0;
            //granie.liczbaGenerowanychMax = 0;
            Statyczne.bufor.ClearBuffer();
        }


        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //granie.t.Change(0, (long)(sender as Slider).Value);
        }

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
            // if (suwakdziala&&e.NewValue!=e.OldValue)
            //if(((System.Windows.RoutedEventArgs)(e)).UserInitiated)
            //if(((System.Windows.RoutedEventArgs)(e)).Handled)
            if (e.NewValue != (int)suwak.Tag)
            {
                if (granie.graniePrzy != (int)suwak.Value)
                    Statyczne.bufor.ClearBuffer();
                granie.graniePrzy = (int)suwak.Value;
            }
        }

        internal void zmianaLogowania(PolaczenieHTTP polaczenieHTTP)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, (ThreadStart)delegate()
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
                                     });
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            Grid_Dropp(sender, e);
        }
        public static void Grid_Dropp(object sender, DragEventArgs e)
        {

            //if (e.Data.GetData("audio/x-syntezator-krawczyka-instrument")!=null)
            //            nowyInstrument.laduj((string)e.Data.GetData("audio/x-syntezator-krawczyka-instrument"));
            if (e.Data.GetData("audio/x-syntezator-krawczyka-instrument") != null)
            {
                // e.Effects = DragDropEffects.None;
               /* if (e.Data.GetData("audio/x-syntezator-krawczyka-instrument").GetHashCode() == hashCodeDragAndDrop)
                {
                    e.Effects = DragDropEffects.None;
                    // e.Data.SetData(null);
                }
                */
            }
            else if (e.Data.GetData("FileDrop") != null)
            {
                string[] explode = (e.Data.GetData("FileDrop") as String[])[0].Split('.');
                if (explode.Last() == "mid" || explode.Last() == "midi")
                    Statyczne.otwartyplik = new plikmidi((e.Data.GetData("FileDrop") as String[])[0]);
                else
                    Statyczne.otwartyplik = new plik((e.Data.GetData("FileDrop") as String[])[0]);
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        public void Grid_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetData("audio/x-syntezator-krawczyka-instrument") != null)
            {
                // e.Effects = DragDropEffects.None;
                if (e.Data.GetData("audio/x-syntezator-krawczyka-instrument").GetHashCode() == hashCodeDragAndDrop)
                {
                    e.Effects = DragDropEffects.None;
                   // e.Data.SetData(null);
                }

            }
            else if (e.Data.GetData("FileDrop") != null)
            {
                /*string[] explode = e.Data.GetData("FileDrop").Split('.');
                if (explode.Last() == "mid" || explode.Last() == "midi")
                    Statyczne.otwartyplik = new plikmidi(dialog.FileName);
                else
                    Statyczne.otwartyplik = new plik(dialog.FileName);*/
            }
            else
            {
                
                e.Effects = DragDropEffects.None;
            }
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

                dialog.Multiselect = true;
                dialog.ShowDialog();
                wtyczkaVST.wndprocStart();
                foreach (var x in dialog.FileNames)
                {
                    //wtyczkaVST.test1(dialog.FileName);
                    //wtyczkaVST.test2(dialog.FileName);
                    //wtyczkaVST.test3(dialog.FileName);
                    // var b = new HostCommandStub();
                    ThreadPool.QueueUserWorkItem((xx) =>
                    {

                        var a = new wtyczkaVST(xx as string);
                        var sou = new sound();
                        sou.nazwa = a.Nazwa;
                        if (Statyczne.otwartyplik.moduły.ContainsKey(a.Nazwa))
                            for (var i = 1; true; i++)
                            {
                                if (!Statyczne.otwartyplik.moduły.ContainsKey(a.Nazwa + i.ToString()))
                                {
                                    sou.nazwa = a.Nazwa + i.ToString();
                                    break;
                                }

                            }
                        Statyczne.otwartyplik.moduły.Add(sou.nazwa, sou);
                        sou.sekw = a;
                        sou.xml = a.xml = Statyczne.otwartyplik.xml.CreateElement("sound");
                        var type = Statyczne.otwartyplik.xml.CreateAttribute("type");
                        type.Value = "VST";
                        sou.xml.Attributes.Append(type);
                        var url = Statyczne.otwartyplik.xml.CreateAttribute("url");
                        url.Value = xx as string;
                        sou.xml.Attributes.Append(url);
                        Statyczne.otwartyplik.xml.DocumentElement.AppendChild(sou.xml);
                        Statyczne.otwartyplik.zapis += a.actionZapis;
                        MainWindow.dispat.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, (ThreadStart)delegate()
                        {
                            sou.UI = new Instrument(sou.nazwa, sou, "VST");
                            sou.UI.wewnętrzny.Children.Add((a).UI);
                            if (!MainWindow.thi.pokazInstr.Children.Contains(sou.UI))
                            {

                                MainWindow.thi.pokazInstr.Children.Add(sou.UI);
                            }
                        });
                    }, x);
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


        private void OśCzasu_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OśCzasu();
            okno.Show();
        }

        private void Oscyloskop_Click(object sender, RoutedEventArgs e)
        {
            Oscyloskop.pokarz();
        }

        private void J8_click(object sender, RoutedEventArgs e)
        {
            granie.bity = 8;
            Statyczne.otwartyplik.generuj();
        }

        private void J16_click(object sender, RoutedEventArgs e)
        {

            granie.bity = 16;
            Statyczne.otwartyplik.generuj();
        }
        private void J32_click(object sender, RoutedEventArgs e)
        {

            granie.bity = 32;
            Statyczne.otwartyplik.generuj();
        }

        private void WyjDzwieku_Click(object sender, RoutedEventArgs e)
        {
            WyjścieDzwieku.pokarz();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var okno = new About();
            okno.Show();
        }

        private void WWW_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://jaebe.za.pl/musicstudio");
        }





        public void Dispose()
        {
            if (klawiatkompa1 != null)
                klawiatkompa1.Dispose();
            if (klawiatkompa2 != null)
                klawiatkompa2.Dispose();
            foreach (var x in klawiatMidi)
                x.Dispose();

        }

        private void BoxTempo_TextChanged(object sender, TextChangedEventArgs e)
        {
            try{
            plik.tempo = float.Parse(BoxTempo.Text);
            var at=Statyczne.otwartyplik.xml.CreateAttribute("tempo");
            at.Value=BoxTempo.Text;
            Statyczne.otwartyplik.xml.DocumentElement.Attributes.Append(at);
             foreach(var scie in   Statyczne.otwartyplik.sciezki)
             {
                 foreach (var nut in scie.nuty)
                     nut.przeliczOpóźnenie();
             }
             Statyczne.otwartyplik.zmiana();
            }catch{}
        }
    }
}