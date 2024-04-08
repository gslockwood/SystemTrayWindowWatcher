using System;
using System.Windows.Forms;

namespace BasicSystemTrayApplication
{
    public class BasicSystemTrayApplicationContext : ApplicationContext
    {
        protected NotifyIcon notifyIcon = new NotifyIcon();
        protected MenuItem exitMenuItem = null;

        public BasicSystemTrayApplicationContext()
        {
            exitMenuItem = new MenuItem( "Exit", new EventHandler( Exit ) );

            notifyIcon.Text = "BasicSystemTrayApplicationContext";
            notifyIcon.Icon = BasicSystemTrayApplication.Properties.Resources.AppIcon;
            notifyIcon.DoubleClick += new EventHandler( DoubleClickMethod );
            notifyIcon.ContextMenu = new ContextMenu( new MenuItem[] { exitMenuItem } );
            notifyIcon.Visible = true;
        }

        protected virtual void DoubleClickMethod( object sender, EventArgs e )
        {
        }

        protected virtual void Exit( object sender, EventArgs e )
        {
            notifyIcon.Visible = false;
            Application.Exit();
        }
    }
}
