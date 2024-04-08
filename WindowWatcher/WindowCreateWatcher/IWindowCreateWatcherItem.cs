using System.Collections.Generic;

namespace WindowWatcher.WindowCreateWatcher
{
    public interface IWindowCreateWatcherItem
    {
        string MsgToSend { get; }
        IList<string> WindowTitles { get; }
    }

}