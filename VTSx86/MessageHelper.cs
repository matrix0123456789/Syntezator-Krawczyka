
using Syntezator_Krawczyka;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Syntezator_Krawczyka
{

    enum polecenia { pokarzOkno, ukryjOkno, załadowano, Nazwa, wcisnijKlawisz, puśćKlawisz, Zapisz, Ładuj, Dźwięk, stanZaladowano }
    public struct NutaStruct
    {
        public int a;
        public int nuta;
    }
}

namespace SIURegistry_Installer
{//Used for WM_COPYDATA for string messages
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        // [MarshalAs(UnmanagedType.LPStr)]
        public string lpData;
    }//Used for WM_COPYDATA for string messages
    public unsafe struct COPYBYTESTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        // [MarshalAs(UnmanagedType.LPStr)]
        public NutaStruct* lpData;
    }
    public unsafe struct COPYBYTESTRUCT2
    {
        public IntPtr dwData;
        public int cbData;
        // [MarshalAs(UnmanagedType.LPStr)]
        public byte* lpData;
    }
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
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, ref COPYBYTESTRUCT2 lParam);

        //For use with WM_COPYDATA and COPYDATASTRUCT
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(int hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, int lParam);
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, IntPtr Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(int hWnd, int Msg, int wParam, int lParam);

        [DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(int hWnd);

        public const int WM_USER = 0x400;
        public const int WM_COPYDATA = 0x4A;



        public bool bringAppToFront(int hWnd)
        {
            return SetForegroundWindow(hWnd);
        }

        public static int sendWindowsMessage(int hWnd, int wParam, string msg)
        {
            int result = 0;

            if (hWnd != 0)
            {
                byte[] sarr = System.Text.Encoding.Default.GetBytes(msg);
                int len = sarr.Length;
                COPYDATASTRUCT cds;
                cds.dwData = (IntPtr)Process.GetCurrentProcess().Id;
                cds.lpData = msg;
                cds.cbData = len + 1;
                // cds.process = (IntPtr)Process.GetCurrentProcess().Id;
                result = SendMessage(hWnd, 0x4A, wParam, ref cds);
            }
            return result;
        }
        public unsafe static int sendWindowsMessage(int hWnd, int wParam, NutaStruct msg)
        {
            int result = 0;

            if (hWnd != 0)
            {
                COPYBYTESTRUCT cds;
                cds.dwData = (IntPtr)Process.GetCurrentProcess().Id;
                cds.cbData = Marshal.SizeOf(msg) + 1;
                var blok = Marshal.AllocHGlobal(100);
                //Marshal.StructureToPtr(msg, blok, false);
                cds.lpData = (NutaStruct*)blok;
                //SendMessage(hWnd, 0x4A, wParam, ref cds);
                cds.lpData[0] = msg;
                SendMessage(hWnd, 0x4A, wParam, ref cds);

            }
            return result;
        }
        public unsafe static int sendWindowsMessage(int hWnd, int wParam, byte[] msg)
        {
            int result = 0;

            if (hWnd != 0)
            {
                COPYBYTESTRUCT2 cds;
                cds.dwData = (IntPtr)Process.GetCurrentProcess().Id;
                cds.cbData = msg.Length + 10;
                var blok = Marshal.AllocHGlobal(msg.Length + 10);
                Marshal.StructureToPtr(msg, blok, false);
                cds.lpData = (byte*)blok;
                //SendMessage(hWnd, 0x4A, wParam, ref cds);
                //cds.lpData[0] = msg;
                SendMessage(hWnd, 0x4A, wParam, ref cds);

            }
            return result;
        }
        public unsafe static int sendWindowsMessage(int hWnd, int wParam, float* msg, int length)
        {
            int result = 0;

            if (hWnd != 0)
            {
                COPYBYTESTRUCT2 cds;
                cds.dwData = (IntPtr)Process.GetCurrentProcess().Id;
                cds.cbData = length + 10;
                //var blok = Marshal.AllocHGlobal(msg.Length + 10);
                //Marshal.StructureToPtr(msg, blok, false);
                cds.lpData = (byte*)msg;
                //SendMessage(hWnd, 0x4A, wParam, ref cds);
                //cds.lpData[0] = msg;
                result=SendMessage(hWnd, 0x4A, wParam, ref cds);

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