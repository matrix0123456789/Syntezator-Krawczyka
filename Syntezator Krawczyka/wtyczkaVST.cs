
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Diagnostics;
using SIURegistry_Installer;
using System.Threading;
using System.Windows.Interop;
using System.Windows.Forms;
using System.IO;
using Syntezator_Krawczyka;



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
            proces = Process.Start("VTSx86.exe", "\"" + path + "\" " + (Process.GetCurrentProcess().Id));
            otwarte.Add(proces.Id, this);
            czyWłączone = true;
        }
        public string _Nazwa = null;
        public string Nazwa
        {
            get
            {
                if (_Nazwa == null)
                {

                    SendMessage(proces.MainWindowHandle, 8753, (IntPtr)polecenia.Nazwa.GetHashCode(), (IntPtr)0);
                }
                int cz = 0;
                while (_Nazwa == null)
                {
                    Thread.Sleep(1); cz++;
                    if (cz % 10 == 0)

                        SendMessage(proces.MainWindowHandle, 8753, (IntPtr)polecenia.Nazwa.GetHashCode(), (IntPtr)0);
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
            dane.ilepróbekNaStarcie = input.ilepróbekNaStarcie;
            MessageHelper.sendWindowsByteMessage((int)proces.MainWindowHandle, (int)polecenia.działaj.GetHashCode(), dane);
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

        public System.Xml.XmlElement xml;
        private Process proces;

        internal void actionZapis()
        {
            while (xml.ChildNodes.Count > 0)
            {
                xml.RemoveChild(xml.LastChild);
            }
        }


        static wtyczkaVST()
        {
            HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(MainWindow.thi).Handle);
            source.AddHook(new HwndSourceHook(WndProc));
        }
        static public void wndprocStart() { }


        private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {

            if (msg == 8753)
            {
                otwarte[(long)lParam].czyWłączone = true;
                /* ThreadPool.QueueUserWorkItem((a) =>
                 {
                     SendMessage(otwarte[(int)lParam].proces.MainWindowHandle, 8753, (uint)polecenia.pokarzOkno.GetHashCode(), new byte[0]);
                 });*/
            }
            if (msg == 0x4A)
            {
                Message a = new Message();
                a.Msg = msg;
                a.HWnd = hwnd;
                a.LParam = lParam;
                a.WParam = wParam;
                var o = (SIURegistry_Installer.MessageHelper.COPYDATASTRUCT)a.GetLParam(typeof(SIURegistry_Installer.MessageHelper.COPYDATASTRUCT));
                otwarte[(long)o.dwData].Nazwa = o.lpData;
            }
            return IntPtr.Zero;
        }

        internal void Pokarz()
        {
            SendMessage(proces.MainWindowHandle, 8753, (IntPtr)polecenia.pokarzOkno.GetHashCode(), (IntPtr)0);
        }
    }

    enum polecenia { pokarzOkno, ukryjOkno, załadowano, Nazwa, działaj, puśćKlawisz }
    public struct NutaStruct
    {
        public double ilepróbekNaStarcie;
    }
}


namespace SIURegistry_Installer
{
    public class MessageHelper
    {
        [DllImport("User32.dll")]
        private static extern int RegisterWindowMessage(string lpString);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern Int32 FindWindow(String lpClassName, String lpWindowName);

        //For use with WM_COPYDATA and COPYDATASTRUCT
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);
        //For use with WM_COPYDATA and COPYDATASTRUCT
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, ref COPYBYTESTRUCT lParam);

        //For use with WM_COPYDATA and COPYDATASTRUCT
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(int hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, int lParam);

        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(int hWnd, int Msg, int wParam, int lParam);

        [DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(int hWnd);

        public const int WM_USER = 0x400;
        public const int WM_COPYDATA = 0x4A;

        //Used for WM_COPYDATA for string messages
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            // [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
            public IntPtr process;
        }//Used for WM_COPYDATA for string messages
        public struct COPYBYTESTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public NutaStruct lpData;
        }

        public bool bringAppToFront(int hWnd)
        {
            return SetForegroundWindow(hWnd);
        }

        public static int sendWindowsStringMessage(int hWnd, int wParam, string msg)
        {
            int result = 0;

            if (hWnd != 0)
            {
                byte[] sarr = System.Text.Encoding.Default.GetBytes(msg);
                int len = sarr.Length;
                COPYDATASTRUCT cds;
                cds.dwData = (IntPtr)100;
                cds.lpData = msg;
                cds.cbData = len + 1;
                cds.process = (IntPtr)Process.GetCurrentProcess().Id;
                result = SendMessage(hWnd, 0x4A, wParam, ref cds);
            }
            return result;
        }
        public static int sendWindowsByteMessage(int hWnd, int wParam, NutaStruct msg)
        {
            int result = 0;

            if (hWnd != 0)
            {
                //int len = msg.Length;
                COPYBYTESTRUCT cds;
                cds.dwData = (IntPtr)100;
                cds.lpData = msg;
                cds.cbData = Marshal.SizeOf(msg);
                //cds.process = (IntPtr)Process.GetCurrentProcess().Id;
                result = SendMessage(hWnd, 0x4A, wParam, ref cds);
            }
            return result;
        }

        public int sendWindowsMessage(int hWnd, int Msg, int wParam, int lParam)
        {
            int result = 0;

            if (hWnd > 0)
            {
                result = SendMessage(hWnd, Msg, wParam, lParam);
            }
            return result;
        }

        public int getWindowId(string className, string windowName)
        {

            return FindWindow(className, windowName);

        }
    }
}