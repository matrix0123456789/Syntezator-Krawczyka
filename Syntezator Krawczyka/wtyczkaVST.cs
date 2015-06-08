
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Runtime.Serialization.Formatters.Binary;
using SIURegistry_Installer;
using System.Threading;
using System.Windows.Interop;
using System.Windows.Forms;
using System.IO;
using Syntezator_Krawczyka;
using Syntezator_Krawczyka.Synteza;



namespace Syntezator_Krawczyka
{
    class wtyczkaVST : soundStart
    {
        static Dictionary<long, wtyczkaVST> otwarte = new System.Collections.Generic.Dictionary<long, wtyczkaVST>();
        VstUI _UI = null;
        public VstUI UI
        {
            get
            {
                if (_UI == null)
                    _UI = new VstUI();
                return _UI;
            }
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SendMessage(IntPtr hwnd, [MarshalAs(UnmanagedType.U4)] int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern uint SendMessage(IntPtr hWnd, uint MSG, uint zero, byte[] text);
        //For use with WM_COPYDATA and COPYDATASTRUCT
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, ref string lParam);
        [MarshalAs(UnmanagedType.LPStr)]
        string zzzz = "esdx";
        public wtyczkaVST(string path)
        {
            var exe = System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName;
            var info = new ProcessStartInfo(exe.Substring(0, exe.LastIndexOfAny(new char[] { '/', '\\' })) + "\\VTSx86.exe", "\"" + path + "\" " + (Process.GetCurrentProcess().Id));
            info.WorkingDirectory = path.Substring(0, path.LastIndexOfAny(new char[] { '/', '\\' }));
            info.UseShellExecute = false;
            proces = Process.Start(info);
            otwarte.Add(proces.Id, this);
            czyWłączone = true;
            Statyczne.otwartyplik.zapis += zapiszUstawienia;
            timerek = new System.Threading.Thread(timerekCall);
            timerek.Start();
        }
        private void zapiszUstawienia()
        {
            SendMessage(MainWindowHandle, 8753, (IntPtr)polecenia.Zapisz.GetHashCode(), (IntPtr)0);
        }
        public string _Nazwa = null;
        public string Nazwa
        {
            get
            {
                if (_Nazwa == null)
                {

                    var res1 = SendMessage(MainWindowHandle, 8753, (IntPtr)polecenia.Nazwa.GetHashCode(), (IntPtr)0);
                }
                int cz = 0;
                while (_Nazwa == null)
                {
                    Thread.Sleep(1); cz++;
                    if (cz % 10 == 0)
                    {
                        var res2 = SendMessage(MainWindowHandle, 8753, (IntPtr)polecenia.Nazwa.GetHashCode(), (IntPtr)0);
                    }
                    if (cz > 1000)
                        _Nazwa = "nieznany";
                }
                return _Nazwa;
            }
            set
            {
                _Nazwa = value;
            }
        }
        public void działaj(nuta input)
        {
            var dane = new NutaStruct();
            dane.nuta = (int)(funkcje.ton(input.ilepróbekNaStarcie) * 2 + 60);
            dane.a = 0x11223344;
            MessageHelper.sendWindowsMessage((int)MainWindowHandle, (int)polecenia.wcisnijKlawisz.GetHashCode(), dane);
        }
        public void pusc(nuta input)
        {
            var dane = new NutaStruct();
            dane.nuta = (int)(funkcje.ton(input.ilepróbekNaStarcie) * 2 + 60);
            dane.a = 0x11223344;
            MessageHelper.sendWindowsMessage((int)MainWindowHandle, (int)polecenia.puśćKlawisz.GetHashCode(), dane);
        }

        public bool czyWłączone
        {
            get;
            set;
        }

        public long symuluj(long p)
        {
            throw new NotImplementedException();
        }

        public System.Xml.XmlNode xml;
        private Process proces;
        IntPtr _MainWindowHandle = (IntPtr)0;
       public IntPtr MainWindowHandle
        {
            get
            {
                if ((long)_MainWindowHandle != 0)
                    return _MainWindowHandle;
                else
                    return proces.MainWindowHandle;
            }
            set { _MainWindowHandle = value; }
        }
        internal void actionZapis()
        {
            /* while (xml.ChildNodes.Count > 0)
             {
                 xml.RemoveChild(xml.LastChild);
             }*/
        }


        static wtyczkaVST()
        {
            IntPtr han = (IntPtr)0;
        sprawdz:
            if (MainWindow.thi != null)
                han = new WindowInteropHelper(MainWindow.thi).Handle;
            if (han == (IntPtr)0 && Start.thi != null)
            {
                han = new WindowInteropHelper(Start.thi).Handle;
            }
            if (han != (IntPtr)0)
            /*{
                Thread.Sleep(1);
                goto sprawdz;
            }*/
            {
                HwndSource source = HwndSource.FromHwnd(han);
                source.AddHook(new HwndSourceHook(WndProc));
            }
        }

        private void timerekCall(object state)
        {
            while (true)
            {
                var arr = granie.dodanoDo.ToArray();
                foreach (var x in arr)
                {
                    
                        SendMessage(otwarte[(long)x.Key].MainWindowHandle, 8753, (IntPtr)polecenia.stanZaladowano.GetHashCode(), (IntPtr)x.Value);
                    
                }
                Thread.Sleep(10);
            }
            
        }
        static public void wndprocStart() { }
        System.Threading.Thread timerek;

        private unsafe static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {

            if (msg == 8754)
            {
                var i = 0;
                while (!otwarte.ContainsKey((long)lParam)&&i<3000)
                {
                    Thread.Sleep(1);
                   // i++;
                }
                if (otwarte.ContainsKey((long)lParam))
                {
                    otwarte[(long)lParam].czyWłączone = true;
                    otwarte[(long)lParam].MainWindowHandle = wParam;
                }
                /* ThreadPool.QueueUserWorkItem((a) =>
                 {
                     SendMessage(otwarte[(int)lParam].MainWindowHandle, 8753, (uint)polecenia.pokarzOkno.GetHashCode(), new byte[0]);
                 });*/
            }
            if (msg == 0x4A)
            {
                Message a = new Message();
                a.Msg = msg;
                a.HWnd = hwnd;
                a.LParam = lParam;
                a.WParam = wParam;
                if ((polecenia)wParam == polecenia.Nazwa)
                {

                    var o = (COPYDATASTRUCT)a.GetLParam(typeof(COPYDATASTRUCT));
                    otwarte[(long)o.dwData].Nazwa = o.lpData;
                }
                else if ((polecenia)wParam == polecenia.Zapisz)
                {

                    var o = (COPYDATASTRUCT)a.GetLParam(typeof(COPYDATASTRUCT));
                    //Marshal.
                    otwarte[(long)o.dwData].xml.InnerText = o.lpData;
                }
                else if ((polecenia)wParam == polecenia.Dźwięk)
                {

                    var o = (COPYBYTESTRUCT3)a.GetLParam(typeof(COPYBYTESTRUCT3));
                    long opuz = 0;
                    lock (granie.dodanoDo)
                    {
                        if (granie.dodanoDo.ContainsKey(o.dwData))
                            opuz = granie.dodanoDo[o.dwData];
                        granie.Działaj(o.lpData, (o.cbData - 10) / 8, opuz, 0);
                        granie.dodanoDo[o.dwData] = opuz + (o.cbData - 10) / 8;
                        return (IntPtr)granie.dodanoDo[o.dwData];
                    }
                }
            }
            return IntPtr.Zero;
        }
        public void ładuj(string base64chunk)
        {
            var res = MessageHelper.sendWindowsMessage((int)MainWindowHandle, (int)polecenia.Ładuj, base64chunk);
        }
        internal void Pokarz()
        {
            SendMessage(MainWindowHandle, 8753, (IntPtr)polecenia.pokarzOkno.GetHashCode(), (IntPtr)0);
        }


    }

}

