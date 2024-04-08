using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using Utilities;

namespace WindowWatcher
{
    public class WindowWatcher : IWindowWatcher
    {
        protected ManualResetEvent manualResetEvent = new ManualResetEvent( false );

        protected AutomationEventHandler UIAeventHandler;
        private AutomationEvent automationEvent = WindowPattern.WindowOpenedEvent;

        public string Name { get; }
        public IList<IWindowWatcherItem> WatcherItems { get { return watcherItems; } }
        protected readonly IList<IWindowWatcherItem> watcherItems = new List<IWindowWatcherItem>();

        public WindowWatcher( string name, AutomationEvent automationEvent ) { Name = name; this.automationEvent = automationEvent; }
        public void AddWatcher( IWindowWatcherItem windowWatcherItem )
        {
            watcherItems.Add( windowWatcherItem );
        }

        public virtual void Start()
        {
            ConsoleEx.Log( GetType().Name + "/" + MethodBase.GetCurrentMethod().Name );

            new Thread( () =>
            {
                Automation.AddAutomationEventHandler(
                    automationEvent,
                    AutomationElement.RootElement,
                    TreeScope.Subtree,
                    UIAeventHandler = new AutomationEventHandler( OnUIAutomationEvent ) );

                // keep it alive
                manualResetEvent.WaitOne();

            } ).Start();

            // seems necessary that it needs a bit of time to startup (before trying to start another)
            System.Threading.Thread.Sleep( 1000 );

        }
        public virtual void ShutdownUIA()
        {
            manualResetEvent.Set();

            ConsoleEx.Log( GetType().Name + "/" + MethodBase.GetCurrentMethod().Name );

            if( UIAeventHandler != null )
            {
                Automation.RemoveAutomationEventHandler( WindowPattern.WindowOpenedEvent, AutomationElement.RootElement, UIAeventHandler );
                ConsoleEx.Log( "ShutdownUIA: RemoveAutomationEventHandler" );

                UIAeventHandler = null;
            }
        }

        protected virtual IntPtr GetTopMostWindow( IntPtr dialogHandle )
        {
            IntPtr topMostWindoow = IntPtr.Zero;
            List<IntPtr> children = GetAllVisbleTitledChildrenHandles( NativeMethods.GetDesktopWindow() );
            if( children != null && children.Count > 0 )
            {
                int index = children.FindIndex( x => x == dialogHandle );
                topMostWindoow = children[index + 1];
                //StringBuilder title = new StringBuilder( 256 );
                //NativeMethods.GetWindowText( topMostWindoow, title, 256 );
                //Utilities.ConsoleEx.Log( title );
            }
            return topMostWindoow;
            //
        }

        protected List<IntPtr> GetAllVisbleTitledChildrenHandles( IntPtr hParent )
        {
            StringBuilder title = new StringBuilder( 256 );

            List<IntPtr> result = new List<IntPtr>();
            IntPtr prevChild = IntPtr.Zero;
            IntPtr currChild = IntPtr.Zero;
            do
            {
                currChild = NativeMethods.FindWindowEx( hParent, prevChild, null, null );
                if( currChild == IntPtr.Zero ) break;

                NativeMethods.GetWindowText( currChild, title, 256 );
                if( NativeMethods.IsWindowVisible( currChild ) && title.Length > 0 )
                    result.Add( currChild );

                prevChild = currChild;
            } while( true );
            return result;
        }

        protected bool FindChild( Process process, IntPtr hwndChild )
        {
            // IsChild doesn't work
            return IsChild( process, hwndChild );
        }
        protected bool IsChild( Process process, IntPtr hwndChild )
        {
            // doesn't work

            bool result = false;

            if( string.IsNullOrEmpty( process.MainWindowTitle ) ) return false;
            if( process.StartInfo.WindowStyle == ProcessWindowStyle.Hidden || process.StartInfo.WindowStyle == ProcessWindowStyle.Minimized ) return false;

            foreach( ProcessThread processThread in process.Threads )
            {
                result = NativeMethods.EnumThreadWindows( processThread.Id,
                    ( hWnd, lParam ) =>
                    {
                        //if( !NativeMethods.IsWindowVisible( hWnd ) )
                        //    return false;

                        //Get the Window's Title.
                        StringBuilder title = new StringBuilder( 256 );
                        NativeMethods.GetWindowText( hWnd, title, 256 );

                        //Check if Window has Title.
                        if( title.Length == 0 )
                            return false;

                        Utilities.ConsoleEx.Log( title );

                        if( !NativeMethods.IsWindowVisible( hWnd ) )
                            return false;

                        return ( hWnd != hwndChild ) ? false : true;
                        /*
                        //Utilities.ConsoleEx.Log( "( hWnd == hwndChild ) " + ( hWnd == hwndChild ) );
                        if( hWnd == hwndChild )
                            return true;

                        return false;*/

                    }, IntPtr.Zero );

                if( !result )
                    continue;

                if( result )
                    return true;

                continue;

            }

            return false;

        }
        protected Process[] GetProcessesByName( string processName )
        {
            Process[] processes = null;
            if( string.IsNullOrEmpty( processName ) )
                processes = Process.GetProcesses();
            else
            {
                if( processName.Equals( "*" ) )
                    processes = Process.GetProcesses();
                else
                    processes = Process.GetProcessesByName( processName );
            }

            return processes;
            //
        }
        private IWindowWatcherItem GetWatcher( IntPtr topMostWindoow )
        {
            string name = GetProcessNameByHandle( topMostWindoow );
            if( name == null ) return null;

            foreach( IWindowWatcherItem watcher in watcherItems )
            {
                //Utilities.ConsoleEx.Log( "" + watcher.ProcessName );
                if( !watcher.ProcessName.Equals( name ) ) continue;
                //Utilities.ConsoleEx.Log( "\tFound: " + name );

                return watcher;

            }
            return null;
        }

        protected virtual void OnUIAutomationEvent( object sender, AutomationEventArgs e )
        {
            //ConsoleEx.Log( GetType().Name + "/" + MethodBase.GetCurrentMethod().Name );

            //if( sender == null ) return;

            AutomationElement sourceElement;
            try
            {
                sourceElement = sender as AutomationElement;
            }
            catch( ElementNotAvailableException )
            {
                return;
            }

            if( sourceElement == null ) return;

            Execute( sourceElement );

        }

        protected virtual bool Execute( AutomationElement src )
        {
            IntPtr handle = (IntPtr)src.Current.NativeWindowHandle;
            if( handle == IntPtr.Zero ) return false;

            IntPtr parentWindowHandle = GetTopMostWindow( handle );
            if( parentWindowHandle == IntPtr.Zero ) return false;

            IWindowWatcherItem watcher = GetWatcher( parentWindowHandle );
            if( watcher == null ) return false;

            foreach( string windowTitle in watcher.WindowTitles )
            {
                try
                {
                    if( src.Current.Name.Equals( windowTitle ) )
                    {
                        //Utilities.ConsoleEx.Log( "\twindowTitle: " + windowTitle );
                        if( src.Current.ClassName == watcher.WindowClassName )
                        {
                            //Utilities.ConsoleEx.Log( "\tFound: windowTitle: " + windowTitle );
                            //bool result = ExecuteItem( watcher, (IntPtr)src.Current.NativeWindowHandle );
                            bool result = ExecuteItem( watcher, handle, parentWindowHandle );
                            if( result )
                                return true;
                            else
                            {
                            }
                        }
                    }
                }
                catch( Exception ex )
                {
                    //Utilities.ConsoleEx.Log( ex.Message );
                    return false;
                    //throw ex;
                }
            }

            return false;
            //
        }

        protected virtual bool ExecuteItem( IWindowWatcherItem watcher, IntPtr dialogHandle, IntPtr parentWindowHandle )
        {
            return false;
        }

        internal string GetProcessNameByHandle( IntPtr hWnd )
        {
            string name = null;
            Process process = null;
            if( hWnd != IntPtr.Zero )
            {
                UInt32 processid = 0;
                NativeMethods.GetWindowThreadProcessId( hWnd, out processid );
                if( processid != 0 )
                    process = Process.GetProcessById( (int)processid );


                name = process.ProcessName;

                //if( process.MainWindowHandle != IntPtr.Zero )
                //{
                //    process.WaitForInputIdle();
                //    handle = process.MainWindowHandle;

                //    if( NativeMethods.GetForegroundWindow() != handle )
                //        NativeMethods.SetForegroundWindow( handle );
                //    //
                //}

                //bool ret = WinFunctions.CloseWindow( hWnd );// works!!
            }

            return name;
            //
        }
    }

}