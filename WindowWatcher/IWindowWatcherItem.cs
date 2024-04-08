using System.Collections.Generic;

namespace WindowWatcher
{
    public interface IWindowWatcherItem
    {
        string Name { get; }
        string ProcessName { get; }
        string WindowClassName { get; }
        public IList<string> WindowTitles { get; }
    }
}