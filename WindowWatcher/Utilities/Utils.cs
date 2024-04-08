//using NLog;
namespace Utilities
{
    public class ConsoleEx
    {
        public static void Log( string message )
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine( message );
#else
            System.Console.WriteLine( message );
#endif
        }
        public static void Log( object message )
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine( message );
#else
            System.Console.WriteLine( message );
#endif
        }

        public static void Log( string format, params object?[] args )
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine( format, args );
#else
            System.Console.WriteLine( format, args );
#endif
        }
    }

}