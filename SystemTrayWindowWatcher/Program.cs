using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace SystemTrayWindowWatcher
{
    internal static class Program
    {
        static EventWaitHandle s_event;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault( false );
            //Application.Run( new WatcherSystemTrayApplicationContext() );

            bool created;
            s_event = new EventWaitHandle( false,
                EventResetMode.ManualReset, Assembly.GetEntryAssembly().GetName().Name, out created );
            if( created )
                Launch();
            else
            {
                MessageBox.Show( string.Format( "Instance of {0} is already running!\n\n", Assembly.GetEntryAssembly().GetName().Name ), "Error!", MessageBoxButtons.OK );
                Environment.Exit( 1 );
            }

        }
        static void Launch()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new WatcherSystemTrayApplicationContext() );
        }
    }
}
