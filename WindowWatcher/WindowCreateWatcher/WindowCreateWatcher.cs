using System;
using System.Drawing;
using System.Windows.Automation;
using System.Windows.Forms;

namespace WindowWatcher.WindowCreateWatcher
{
    public class WindowCreateWatcher : WindowWatcher
    {
        public WindowCreateWatcher() : base( "WindowCreateWatcher", WindowPattern.WindowOpenedEvent ) { }

        protected override bool ExecuteItem( IWindowWatcherItem windowWatcherItem, IntPtr handle, IntPtr parentWindowHandle )
        {
            //ConsoleEx.Log( GetType().Name + "/" + MethodBase.GetCurrentMethod().Name );


            if( parentWindowHandle != IntPtr.Zero )
            {
                Rectangle lpRectDialog = new Rectangle();
                NativeMethods.GetWindowRect( handle, ref lpRectDialog );
                Rectangle lpRectChromeWindow = new Rectangle();
                NativeMethods.GetWindowRect( parentWindowHandle, ref lpRectChromeWindow );

                NativeMethods.SetWindowPos(
                    handle,
                    0,
                    ( lpRectChromeWindow.Left + lpRectChromeWindow.Width / 2 ) - lpRectDialog.Width / 2,
                     lpRectChromeWindow.Top + lpRectChromeWindow.Height * 1 / 3,
                     0, 0, NativeMethods.SWP_NOZORDER | NativeMethods.SWP_NOSIZE | NativeMethods.SWP_SHOWWINDOW );

                // make the active window so you can SendKeys to it
                NativeMethods.SetForegroundWindow( handle );

                WindowCreateWatcherItem watcher = windowWatcherItem as WindowCreateWatcherItem;
                if( !string.IsNullOrEmpty( watcher.MsgToSend ) )
                {
                    Utilities.ConsoleEx.Log( "\tMsgToSend: " + watcher.MsgToSend );
                    foreach( char c in watcher.MsgToSend )//"*.pdf"
                        SendKeys.SendWait( c.ToString() );
                    SendKeys.SendWait( "{Enter}" );

                }

                return true;
                //
            }

            else
                return false;

        }

    }

}
