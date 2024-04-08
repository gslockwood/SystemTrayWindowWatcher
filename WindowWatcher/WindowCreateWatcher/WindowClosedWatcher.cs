using System.Windows.Automation;

namespace WindowWatcher.WindowCreateWatcher
{
    public class WindowClosedWatcher : WindowWatcher
    {
        public WindowClosedWatcher() : base( "WindowClosedWatcher", WindowPattern.WindowClosedEvent ) { }

        protected override void OnUIAutomationEvent( object sender, AutomationEventArgs e )
        {
            /*
            ConsoleEx.Log( GetType().Name + "/" + MethodBase.GetCurrentMethod().Name );
            //StringBuilder title = new StringBuilder( 256 );
            //NativeMethods.GetWindowText( dialogHandle, title, 256 );
            //MessageBox.Show( string.Format( "Window was closed" ) );
            ConsoleEx.Log( "Window was closed" );
            */
            return;

            //base.OnUIAutomationEvent( sender, e );
        }

    }

}
