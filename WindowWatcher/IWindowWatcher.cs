namespace WindowWatcher
{
    public interface IWindowWatcher
    {
        string Name { get; }

        void Start();
        void ShutdownUIA();

        void AddWatcher( IWindowWatcherItem windowWatcherItem );
    }
}