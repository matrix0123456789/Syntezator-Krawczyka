using Jacobi.Vst.Core;
using Jacobi.Vst.Core.Host;
using Jacobi.Vst.Interop.Host;
using SIURegistry_Installer;
using Syntezator_Krawczyka;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VTSx86;

namespace VTSx86
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SendMessage(IntPtr hwnd, [MarshalAs(UnmanagedType.U4)] int Msg, IntPtr wParam, IntPtr lParam);
        Process host;
        public Form1()
        {
            InitializeComponent();
            Hide();
            try
            {
                HostCommandStub cmdstub = new HostCommandStub(); //Code for this class is in the VSTHost Sample code
                cont = VstPluginContext.Create(Environment.GetCommandLineArgs()[1], cmdstub);
                cont.PluginCommandStub.StartProcess();

            }
            catch { }
            host = Process.GetProcessById(int.Parse(Environment.GetCommandLineArgs()[2]));
            cont.PluginCommandStub.Open();
            cont.PluginCommandStub.EditorOpen(this.Handle);
            // pokarzOkno();
            ThreadPool.QueueUserWorkItem((a) =>
                {
                    SendMessage(host.MainWindowHandle, 8753, (IntPtr)polecenia.załadowano.GetHashCode(), (IntPtr)Process.GetCurrentProcess().Id);
                });
            //System.Threading.Timer dzwiekti = new System.Threading.Timer(dzwiekodb, null, 100, 100);
            ThreadPool.QueueUserWorkItem((a) =>
            {
                while (true)
                {
                    dzwiekodb(null);
                    Thread.Sleep(10);
                }
            });
        }

        private void dzwiekodb(object state)
        {
            VstAudioBufferManager inputMgr = new VstAudioBufferManager(2, 4800);
            VstAudioBufferManager outputMgr = new VstAudioBufferManager(2, 4800);

            var vstInputBuffers = inputMgr.ToArray();
            var vstOutputBuffers = outputMgr.ToArray();
            cont.PluginCommandStub.ProcessReplacing(vstInputBuffers, vstOutputBuffers);

            unsafe
            {
                float* tablica = (float*)Marshal.AllocHGlobal(vstOutputBuffers[0].SampleCount * 8);
                for (var i = 0; i < vstOutputBuffers[0].SampleCount; i++)
                {
                    tablica[2 * i] = vstOutputBuffers[0][i];
                    tablica[2 * i + 1] = vstOutputBuffers[1][i];
                }
                MessageHelper.sendWindowsMessage((int)host.MainWindowHandle, polecenia.Dźwięk.GetHashCode(), (tablica), vstOutputBuffers[0].SampleCount * 8);
            }
        }

        private VstEvent[] CreateMidiEvent(byte statusByte, byte midiNote, byte midiVelocity)
        {
            /* 
             * Just a small note on the code for setting up a midi event:
             * You can use the VstEventCollection class (Framework) to setup one or more events
             * and then call the ToArray() method on the collection when passing it to
             * ProcessEvents. This will save you the hassle of dealing with arrays explicitly.
             * http://computermusicresource.com/MIDI.Commands.html
             * 
             * Freq to Midi notes etc:
             * http://www.sengpielaudio.com/calculator-notenames.htm
             * 
             * Example to use NAudio Midi support
             * http://stackoverflow.com/questions/6474388/naudio-and-midi-file-reading
             */
            byte[] midiData = new byte[4];

            midiData[0] = statusByte;
            midiData[1] = midiNote;   	// Midi note
            midiData[2] = midiVelocity; // Note strike velocity
            midiData[3] = 0;    		// Reserved, unused

            VstMidiEvent vse = new VstMidiEvent(/*DeltaFrames*/ 	0,
                /*NoteLength*/ 	0,
                /*NoteOffset*/ 	0,
                                                midiData,
                /*Detune*/    		0,
                /*NoteOffVelocity*/ 127); // previously 0

            VstEvent[] ve = new VstEvent[1];
            ve[0] = vse;
            return ve;
        }

        //[MarshalAs(UnmanagedType.LPStr)]
        //string zzzz;
        protected unsafe override void WndProc(ref Message message)
        {
            //filter the RF_TESTMESSAGE
            if (message.Msg == 0x4A)
            {

                var polecenie = (polecenia)message.WParam;
                // 
                switch (polecenie)
                {
                    case polecenia.działaj:
                        var lp = (COPYBYTESTRUCT)message.GetLParam(typeof(COPYBYTESTRUCT));

                        // var test = cont.PluginCommandStub.GetParameterProperties(0);
                        /*var a = (VstPluginCommandStub)cont.PluginCommandStub;
                       var b= a.GetDestinationBuffer();
                        
                        MessageBox.Show(b.ToString());*/
                        cont.PluginCommandStub.SetSampleRate(48000);

                        VstEvent[] vEvent = CreateMidiEvent(144, lp.lpData[0].nuta, 100);
                        cont.PluginCommandStub.ProcessEvents(vEvent);
                        //cont.PluginCommandStub.
                        break;
                    case polecenia.Ładuj:

                        var lpdane = (COPYDATASTRUCT)message.GetLParam(typeof(COPYDATASTRUCT));
                        var chunk = Convert.FromBase64String(lpdane.lpData);
                        cont.PluginCommandStub.SetChunk(chunk, true);
                        message.Result = (IntPtr)polecenia.Ładuj;
                        break;
                    default:
                        MessageBox.Show(polecenie.ToString() + "   zz");
                        break;
                }


            }
            if (message.Msg == 8753)
            {
                // MessageBox.Show(message.ToString());
                if ((int)message.WParam == polecenia.pokarzOkno.GetHashCode())
                    pokarzOkno();
                if ((int)message.WParam == polecenia.Nazwa.GetHashCode())
                {


                    int result = 0;
                    result = MessageHelper.sendWindowsMessage((int)host.MainWindowHandle, polecenia.Nazwa.GetHashCode(), cont.PluginCommandStub.GetEffectName());
                    message.Result = (IntPtr)result;
                } if ((int)message.WParam == polecenia.Zapisz.GetHashCode())
                {

                    var chunk = Convert.ToBase64String(cont.PluginCommandStub.GetChunk(true));
                    MessageHelper.sendWindowsMessage((int)host.MainWindowHandle, polecenia.Zapisz.GetHashCode(), chunk);

                    message.Result = (IntPtr)polecenia.Zapisz;
                }
            }
            //be sure to pass along all messages to the base also
            base.WndProc(ref message);
        }
        VstPluginContext cont;
        //Form okno = null;
        void pokarzOkno()
        {
            //if (okno == null)
            {
                //okno = new Form();
                //cont.PluginCommandStub.EditorOpen(okno.Handle);
            }
            // okno.Show();
            //  Hide();
            //Opacity = 0;
            Show();
        }
    }
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
    public struct NutaStruct
    {
        public int a;
        public double ilepróbekNaStarcie;
    }
}