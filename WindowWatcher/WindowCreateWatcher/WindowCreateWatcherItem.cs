using System.Collections.Generic;

namespace WindowWatcher.WindowCreateWatcher
{
    public class WindowCreateWatcherItem : WindowWatcherItem, IWindowCreateWatcherItem
    {
        //public IList<string> WindowTitles { get; }
        public string MsgToSend { get; }

        //public WindowCreateWatcherItem( string windowClassName, IList<string> windowTitles, string msgToSend = null ) : base( windowClassName, windowTitles )
        public WindowCreateWatcherItem( string name, string windowClassName, IList<string> windowTitles, string msgToSend = null, string processName = null ) :
            base( name, windowClassName, windowTitles, processName )
        {
            //WindowTitles = windowTitles;
            MsgToSend = msgToSend;
        }

    }

}
