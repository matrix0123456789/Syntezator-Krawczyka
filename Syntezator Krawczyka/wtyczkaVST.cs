
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;



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

        public wtyczkaVST(string path)
        {
         proces= Process.Start("VTSx86.exe", path);
         var zm = new dane();

         SendMessage(proces.MainWindowHandle, 8753, (IntPtr)74, System.Runtime.InteropServices.Marshal.StringToCoTaskMemUni("wsxz"));
            
           /* HostCommandStub cmdstub = new HostCommandStub(); //Code for this class is in the VSTHost Sample code
            cont = VstPluginContext.Create(path, cmdstub);

            cont.PluginCommandStub.Open();
            var okno=new  System.Windows.Forms.Form();
            okno.Text = cont.PluginCommandStub.GetEffectName();
            okno.Show();
            cont.PluginCommandStub.EditorOpen(okno.Handle);*/
            
           // cont.PluginCommandStub.
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


    struct dane { public int z; }
    }

