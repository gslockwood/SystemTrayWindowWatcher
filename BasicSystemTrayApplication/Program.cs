using System;
using System.Windows.Forms;

namespace BasicSystemTrayApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            // Instead of running a form, we run an ApplicationContext.
            Application.Run( new BasicSystemTrayApplicationContext() );
        }
    }
}