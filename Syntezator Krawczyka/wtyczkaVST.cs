
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



namespace Syntezator_Krawczyka
{
    class wtyczkaVST:soundStart
    {
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
         proces= Process.Start("VTSx86.exe", path);
         Thread.Sleep(1000);
         var zm =new byte[]{8,21,62,12};

         MessageHelper msg = new MessageHelper();
         int result = 0;
         result = msg.sendWindowsStringMessage((int)proces.MainWindowHandle, 3, "Test");
         result = msg.sendWindowsByteMessage((int)proces.MainWindowHandle, 80, zm);
         result = msg.sendWindowsByteMessage((int)proces.MainWindowHandle, 3, zm);


         SendMessage(proces.MainWindowHandle, 8753, (uint)polecenia.pokarzOkno.GetHashCode(), new byte[0]);

        }
        public string Nazwa
        {
            get
            {
                return "vst";
                //return cont.PluginCommandStub.GetEffectName();
            }
        }
        public string Nazwa23
        {
            get
            {
                return "vst";
                //return cont.PluginCommandStub.GetParameterName(0);
            }
        }
        /*static public void test1(string p) { }
        static public void test2(string p)
        {
            var CommandStub = new HostCommandStub();
        }
        static public void test3(string p)
        {
            var plugin = VstPluginContext.Create(p, null);
        }*/

        public void działaj(nuta input)
        {
            throw new NotImplementedException();
        }

        public bool czyWłączone
        {
            get { throw new NotImplementedException(); }
        }

        public long symuluj(long p)
        {
            throw new NotImplementedException();
        }

        public System.Xml.XmlElement xml;
        private Process proces;

        internal void actionZapis()
        {
            while(xml.ChildNodes.Count>0)
            {
                xml.RemoveChild(xml.LastChild);
            }
        }
    }


    enum polecenia { pokarzOkno, ukryjOkno }
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
        }//Used for WM_COPYDATA for string messages
        public struct COPYBYTESTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1)]
            public byte[] lpData;
        }

        public bool bringAppToFront(int hWnd)
        {
            return SetForegroundWindow(hWnd);
        }

        public int sendWindowsStringMessage(int hWnd, int wParam, string msg)
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
                result = SendMessage(hWnd, 0x4A, wParam, ref cds);
            }
            return result;
        }
        public int sendWindowsByteMessage(int hWnd, int wParam, byte[] msg)
        {
            int result = 0;

            if (hWnd != 0)
            {
                int len = msg.Length;
                COPYBYTESTRUCT cds;
                cds.dwData = (IntPtr)100;
                cds.lpData = msg;
                cds.cbData = len + 1;
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