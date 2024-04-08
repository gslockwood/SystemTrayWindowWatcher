using System.Collections.Generic;

namespace WindowWatcher
{
    public class WindowWatcherItem : IWindowWatcherItem
    {
        public string Name { get; }
        public string ProcessName { get; }
        public string WindowClassName { get; }
        public IList<string> WindowTitles { get; }

        public WindowWatcherItem( string name, string windowClassName, IList<string> windowTitles, string processName )//
        {
            Name = name;
            WindowClassName = windowClassName;
            WindowTitles = windowTitles;
            ProcessName = processName;
        }

    }

}
