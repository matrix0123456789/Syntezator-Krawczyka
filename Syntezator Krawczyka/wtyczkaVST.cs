using Jacobi.Vst.Core;
using Jacobi.Vst.Core.Host;
using Jacobi.Vst.Interop.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Syntezator_Krawczyka
{
    class wtyczkaVST
    {
        VstPluginContext plugin;
        public wtyczkaVST(string path)
        {
            var CommandStub = new HostCommandStub();
            plugin = VstPluginContext.Create(path, CommandStub);
        }
        static public void test1(string p) { }
        static public void test2(string p)
        {
            var CommandStub = new HostCommandStub();
        }
        static public void test3(string p)
        {
            var plugin = VstPluginContext.Create(p, null);
        }
    }
    class HostCommandStub : IVstHostCommandStub {

        public IVstPluginContext PluginContext { get; set; }
        public bool BeginEdit(int index) { return false; }
        public VstCanDoResult CanDo(string cando) { return VstCanDoResult.Unknown; }
        public bool CloseFileSelector(VstFileSelect fileSelect) { return false; }
        public bool EndEdit(int index) { return false; }
        public VstAutomationStates GetAutomationState() { return 0; }
        public int GetBlockSize() { return 0; }
        public string GetDirectory() { return null; }
        public int GetInputLatency() { return 0; }
        public VstHostLanguage GetLanguage() { return 0; }
        public int GetOutputLatency() { return 0; }
        public VstProcessLevels GetProcessLevel() { return 0; }
        public string GetProductString() { return null; }
        public float GetSampleRate() { return 0; }
        public VstTimeInfo GetTimeInfo(VstTimeInfoFlags filterFlags) { return null; }
        public string GetVendorString() { return null; }
        public int GetVendorVersion() { return 0; }
        public bool IoChanged() { return false; }
        public bool OpenFileSelector(VstFileSelect fileSelect) { return false; }
        public bool ProcessEvents(VstEvent[] events) { return false; }
        public bool SizeWindow(int width, int height) { return false; }
        public bool UpdateDisplay() { return false; }
        public int GetCurrentPluginID() { return 0; }
        public int GetVersion() { return 0; }
        public void ProcessIdle() { }
        public void SetParameterAutomated(int index, float value) { }
    }
}
