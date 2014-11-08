using Jacobi.Vst.Core.Host;
using Jacobi.Vst.Interop.Host;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VTSx86
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            try
            {
                HostCommandStub cmdstub = new HostCommandStub(); //Code for this class is in the VSTHost Sample code
                cont = VstPluginContext.Create(Environment.GetCommandLineArgs()[1], cmdstub);
            }
            catch { }
        }

        [MarshalAs(UnmanagedType.LPStr)]
        string zzzz;
        protected override void WndProc(ref Message message)
        {
            //filter the RF_TESTMESSAGE
            if (message.Msg == 0x4A)
            {
                

                     //   MessageBox.Show(message.LParam.ToString(), message.WParam.ToString());
                        if ((int)message.WParam == 3)
                        {
                            var o = (COPYDATASTRUCT)message.GetLParam(typeof(COPYDATASTRUCT));
                            MessageBox.Show(o.lpData, "Wiadomość");
                        } if ((int)message.WParam == 80)
                        {
                            var o = (COPYBYTESTRUCT)message.GetLParam(typeof(COPYBYTESTRUCT));
                            MessageBox.Show(o.lpData[0].ToString(), "dane");
                        }
                   
            }
            if (message.Msg == 8753)
            {
                if ((int)message.WParam == polecenia.pokarzOkno.GetHashCode())
                    pokarzOkno();
            }
            //be sure to pass along all messages to the base also
            base.WndProc(ref message);
        }
        VstPluginContext cont;
        void pokarzOkno()
        {

            cont.PluginCommandStub.Open();
            var okno = new System.Windows.Forms.Form();
            okno.Text = cont.PluginCommandStub.GetEffectName();
            okno.Show();
            cont.PluginCommandStub.EditorOpen(okno.Handle);
        }
    }
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        //[MarshalAs(UnmanagedType.LPStr)]
        public string lpData;
    }
    public struct COPYBYTESTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType =VarEnum. VT_UI1)]
        public byte[] lpData;
    }
    enum polecenia { pokarzOkno, ukryjOkno }
    /// <summary>
    /// The HostCommandStub class represents the part of the host that a plugin can call.
    /// </summary>
    class HostCommandStub : IVstHostCommandStub
    {
        /// <summary>
        /// Raised when one of the methods is called.
        /// </summary>
        public event EventHandler<PluginCalledEventArgs> PluginCalled;

        private void RaisePluginCalled(string message)
        {
            EventHandler<PluginCalledEventArgs> handler = PluginCalled;

            if (handler != null)
            {
                handler(this, new PluginCalledEventArgs(message));
            }
        }

        #region IVstHostCommandsStub Members

        /// <inheritdoc />
        public IVstPluginContext PluginContext { get; set; }

        #endregion

        #region IVstHostCommands20 Members

        /// <inheritdoc />
        public bool BeginEdit(int index)
        {
            RaisePluginCalled("BeginEdit(" + index + ")");

            return false;
        }

        /// <inheritdoc />
        public Jacobi.Vst.Core.VstCanDoResult CanDo(string cando)
        {
            RaisePluginCalled("CanDo(" + cando + ")");
            return Jacobi.Vst.Core.VstCanDoResult.Unknown;
        }

        /// <inheritdoc />
        public bool CloseFileSelector(Jacobi.Vst.Core.VstFileSelect fileSelect)
        {
            RaisePluginCalled("CloseFileSelector(" + fileSelect.Command + ")");
            return false;
        }

        /// <inheritdoc />
        public bool EndEdit(int index)
        {
            RaisePluginCalled("EndEdit(" + index + ")");
            return false;
        }

        /// <inheritdoc />
        public Jacobi.Vst.Core.VstAutomationStates GetAutomationState()
        {
            RaisePluginCalled("GetAutomationState()");
            return Jacobi.Vst.Core.VstAutomationStates.Off;
        }

        /// <inheritdoc />
        public int GetBlockSize()
        {
            RaisePluginCalled("GetBlockSize()");
            return 1024;
        }

        /// <inheritdoc />
        public string GetDirectory()
        {
            RaisePluginCalled("GetDirectory()");
            return null;
        }

        /// <inheritdoc />
        public int GetInputLatency()
        {
            RaisePluginCalled("GetInputLatency()");
            return 0;
        }

        /// <inheritdoc />
        public Jacobi.Vst.Core.VstHostLanguage GetLanguage()
        {
            RaisePluginCalled("GetLanguage()");
            return Jacobi.Vst.Core.VstHostLanguage.NotSupported;
        }

        /// <inheritdoc />
        public int GetOutputLatency()
        {
            RaisePluginCalled("GetOutputLatency()");
            return 0;
        }

        /// <inheritdoc />
        public Jacobi.Vst.Core.VstProcessLevels GetProcessLevel()
        {
            RaisePluginCalled("GetProcessLevel()");
            return Jacobi.Vst.Core.VstProcessLevels.Unknown;
        }

        /// <inheritdoc />
        public string GetProductString()
        {
            RaisePluginCalled("GetProductString()");
            return "VST.NET";
        }

        /// <inheritdoc />
        public float GetSampleRate()
        {
            RaisePluginCalled("GetSampleRate()");
            return 44.8f;
        }

        /// <inheritdoc />
        public Jacobi.Vst.Core.VstTimeInfo GetTimeInfo(Jacobi.Vst.Core.VstTimeInfoFlags filterFlags)
        {
            RaisePluginCalled("GetTimeInfo(" + filterFlags + ")");
            return null;
        }

        /// <inheritdoc />
        public string GetVendorString()
        {
            RaisePluginCalled("GetVendorString()");
            return "Jacobi Software";
        }

        /// <inheritdoc />
        public int GetVendorVersion()
        {
            RaisePluginCalled("GetVendorVersion()");
            return 1000;
        }

        /// <inheritdoc />
        public bool IoChanged()
        {
            RaisePluginCalled("IoChanged()");
            return false;
        }

        /// <inheritdoc />
        public bool OpenFileSelector(Jacobi.Vst.Core.VstFileSelect fileSelect)
        {
            RaisePluginCalled("OpenFileSelector(" + fileSelect.Command + ")");
            return false;
        }

        /// <inheritdoc />
        public bool ProcessEvents(Jacobi.Vst.Core.VstEvent[] events)
        {
            RaisePluginCalled("ProcessEvents(" + events.Length + ")");
            return false;
        }

        /// <inheritdoc />
        public bool SizeWindow(int width, int height)
        {
            RaisePluginCalled("SizeWindow(" + width + ", " + height + ")");
            return false;
        }

        /// <inheritdoc />
        public bool UpdateDisplay()
        {
            RaisePluginCalled("UpdateDisplay()");
            return false;
        }

        #endregion

        #region IVstHostCommands10 Members

        /// <inheritdoc />
        public int GetCurrentPluginID()
        {
            RaisePluginCalled("GetCurrentPluginID()");
            return PluginContext.PluginInfo.PluginID;
        }

        /// <inheritdoc />
        public int GetVersion()
        {
            RaisePluginCalled("GetVersion()");
            return 1000;
        }

        /// <inheritdoc />
        public void ProcessIdle()
        {
            RaisePluginCalled("ProcessIdle()");
        }

        /// <inheritdoc />
        public void SetParameterAutomated(int index, float value)
        {
            RaisePluginCalled("SetParameterAutomated(" + index + ", " + value + ")");
        }

        #endregion
    }

    /// <summary>
    /// Event arguments used when one of the mehtods is called.
    /// </summary>
    class PluginCalledEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs a new instance with a <paramref name="message"/>.
        /// </summary>
        /// <param name="message"></param>
        public PluginCalledEventArgs(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; private set; }
    }
}