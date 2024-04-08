using BasicSystemTrayApplication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Utilities;
using WindowWatcher;
using WindowWatcher.WindowCreateWatcher;

namespace SystemTrayWindowWatcher
{
    public partial class WatcherSystemTrayApplicationContext : BasicSystemTrayApplicationContext
    {
        private readonly MenuItem StartMenuItem;
        private readonly MenuItem StopMenuItem;

        IList<IWindowWatcher> windowWatchers = new List<WindowWatcher.IWindowWatcher>();

        public WatcherSystemTrayApplicationContext() : base()
        {
            StartMenuItem = new MenuItem( "Start", new EventHandler( Start ) );
            StopMenuItem = new MenuItem( "Stop", new EventHandler( Stop ) );
            notifyIcon.ContextMenu = new ContextMenu( new MenuItem[] { StartMenuItem, StopMenuItem, exitMenuItem } );
            notifyIcon.Text = "Dialog watcher";
            notifyIcon.Icon = SystemTrayWindowWatcher.Properties.Resources.dialog_box_balloon;
            //

            //SetupWatchers();
            SetupWatchersBySettingsFile();

            //auto start
            Start( this, null );
            //
        }

        // this could be from a settings file
        private void SetupWatchersBySettingsFile()
        {
            string filePath = string.Empty;
            string? greenFlashSoftware = Environment.GetEnvironmentVariable( "GreenFlashSoftware" );
            if( greenFlashSoftware == null )
            {
                MessageBox.Show( string.Format( "GreenFlashSoftware\\PositionFileDialog doesn't exist. Using current directory" ) );
                filePath = System.IO.Directory.GetCurrentDirectory();
            }
            else
                filePath = string.Format( @"{0}\PositionFileDialog", greenFlashSoftware );


            try
            {
                filePath = string.Format( @"{0}\settings.json", filePath );

                string json = System.IO.File.ReadAllText( filePath );
                //var settings0 = JsonConvert.DeserializeObject<List<WindowWatcher.WindowWatcher>>( json );
                var settings = JsonConvert.DeserializeObject<List<WindowWatcher.WindowWatcher>>( json, new WindowWatcherItemConverter() );
                IWindowWatcherItem theone = settings[0].WatcherItems[1];

                IWindowWatcher windowCreateWatcher = new WindowCreateWatcher();
                IWindowWatcher windowClosedWatcher = new WindowClosedWatcher();

                foreach( var setting in settings )
                {
                    if( setting.Name.Equals( "WindowCreateWatcher" ) )
                    {
                        foreach( WindowCreateWatcherItem watcherItem in setting.WatcherItems )
                        {
                            var item = new WindowCreateWatcherItem( watcherItem.Name, watcherItem.WindowClassName, watcherItem.WindowTitles, watcherItem.MsgToSend, watcherItem.ProcessName );
                            windowCreateWatcher.AddWatcher( item );
                        }
                    }
                    else if( setting.Name.Equals( "WindowClosedWatcher" ) )
                    {
                        foreach( var watcherItem in setting.WatcherItems )
                        {
                            windowClosedWatcher.AddWatcher( new WindowWatcherItem( watcherItem.Name, watcherItem.WindowClassName, watcherItem.WindowTitles, watcherItem.ProcessName ) );
                        }
                    }
                }

                windowWatchers.Add( windowCreateWatcher );
                windowWatchers.Add( windowClosedWatcher );

            }
            catch( Exception ex )
            {
                MessageBox.Show( string.Format( "Problem with loading settings file." ) );

                System.Environment.Exit( 1 );
            }

            /*

if( false )
{
    string json = JsonConvert.SerializeObject( windowWatchers, new WindowWatcherItemConverter() );

    var settings = JsonConvert.DeserializeObject<List<WindowWatcher.WindowWatcher>>( json, new WindowWatcherItemConverter() );
    //if( !System.IO.Directory.Exists( filePath ) )
    //{
    //    System.IO.Directory.CreateDirectory( filePath );
    //    filePath = System.IO.Directory.GetCurrentDirectory();
    //}

    System.IO.File.WriteAllText( filePath, json );
}
//goto again;
*/

        }

        private void SetupWatchers()
        {
            IWindowWatcher windowCreateWatcher = new WindowCreateWatcher();

            IList<string> windowTitles = new List<string>();

            /**/
            //IWindowWatcher windowCreateWatcher = new WindowCreateWatcher();

            windowTitles.Add( "Find" );
            windowCreateWatcher.AddWatcher( new WindowCreateWatcherItem( "Notepad", "#32770", windowTitles, null, "notepad++" ) );//, "notepad++"

            windowTitles = new List<string>();
            windowTitles.Add( "Open" );
            windowCreateWatcher.AddWatcher( new WindowCreateWatcherItem( "JobSearchTable", "#32770", windowTitles, "*.pdf", "JobSearchTable" ) );//, "JobSearchTable"

            windowTitles = new List<string>();
            windowTitles.Add( "Open" );
            windowTitles.Add( "Save" );
            windowCreateWatcher.AddWatcher( new WindowCreateWatcherItem( "Chrome", "#32770", windowTitles, "*.pdf", "chrome" ) );//, "chrome"

            windowWatchers.Add( windowCreateWatcher );

            IWindowWatcher windowClosedWatcher = new WindowClosedWatcher();
            windowClosedWatcher.AddWatcher( new WindowWatcherItem( "Notepad", "Notepad++", windowTitles, "notepad++" ) );//

            windowWatchers.Add( windowClosedWatcher );
        }

        protected override void DoubleClickMethod( object sender, EventArgs e )
        {
            MessageBox.Show( "WatcherSystemTrayApplication for positioning certain windows to a more productive screen location" );
        }

        void Start( object sender, EventArgs e )
        {
            ConsoleEx.Log( GetType().Name + "/" + MethodBase.GetCurrentMethod().Name );

            StartMenuItem.Enabled = false;
            StopMenuItem.Enabled = true;

            foreach( IWindowWatcher windowWatchers in windowWatchers )
                windowWatchers.Start();

        }
        void Stop( object sender, EventArgs e )
        {
            ConsoleEx.Log( GetType().Name + "/" + MethodBase.GetCurrentMethod().Name );

            StartMenuItem.Enabled = true;
            StopMenuItem.Enabled = false;

            foreach( IWindowWatcher windowWatchers in windowWatchers )
                windowWatchers.ShutdownUIA();

        }

        protected override void Exit( object sender, EventArgs e )
        {
            ConsoleEx.Log( GetType().Name + "/" + MethodBase.GetCurrentMethod().Name );

            foreach( IWindowWatcher windowWatchers in windowWatchers )
                windowWatchers.ShutdownUIA();

            base.Exit( sender, e );
        }

    }
}
