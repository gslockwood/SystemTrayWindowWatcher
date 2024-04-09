using System;
using System.Drawing;
using System.Windows.Automation;
using System.Windows.Forms;

namespace WindowWatcher.WindowCreateWatcher
{
    public class WindowCreateWatcherMouse : WindowWatcher
    {
        public WindowCreateWatcherMouse() : base( "WindowCreateWatcherMouse", WindowPattern.WindowOpenedEvent ) { }

        protected override bool ExecuteItem( IWindowWatcherItem windowWatcherItem, IntPtr handle, IntPtr parentWindowHandle )
        {
            if( parentWindowHandle != IntPtr.Zero )
            {
                Rectangle lpRectDialog = new Rectangle();
                NativeMethods.GetWindowRect( handle, ref lpRectDialog );
                Rectangle lpRectChromeWindow = new Rectangle();
                NativeMethods.GetWindowRect( parentWindowHandle, ref lpRectChromeWindow );

                //Utilities.ConsoleEx.Log( System.Windows.Forms.Cursor.Position );
                Point postion = System.Windows.Forms.Cursor.Position;
                NativeMethods.SetWindowPos(
                    handle,
                    0,
                    postion.X - 500,
                     postion.Y - 150,
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

        private void MoveCursor()
        {
            // Set the Current cursor, move the cursor's Position,
            // and set its clipping rectangle to the form. 
            int i;
            Cursor cursor = new Cursor( Cursor.Current.Handle );
            Cursor.Position = new Point( Cursor.Position.X, Cursor.Position.Y );//- 50
            //Cursor.Clip = new Rectangle( this.Location, this.Size );

            Utilities.ConsoleEx.Log( Cursor.Position );
            Utilities.ConsoleEx.Log( System.Windows.Forms.Cursor.Position );

        }


    }
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
