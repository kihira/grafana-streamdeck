using BarRaider.SdTools;

namespace GrafanaStreamdeck
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            // while (!System.Diagnostics.Debugger.IsAttached) { System.Threading.Thread.Sleep(100); }
            
            SDWrapper.Run(args);
        }
    }
}