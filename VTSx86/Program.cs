using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VTSx86
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.GetCommandLineArgs().Length == 0)
            {
                MessageBox.Show("Uruchomiłeś program słurzący do ładowania wtyczek VST do programu Jaebe Music Studio.\r\n\r\nProgram ten może być uruchomiony tylko z poziomu Music Studio.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }
    }
}
