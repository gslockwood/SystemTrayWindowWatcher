namespace WindowWatcher
{
    public interface IWindowCreateWatcher
    {
        void Execute();
        void ShutdownUIA();
    }
}