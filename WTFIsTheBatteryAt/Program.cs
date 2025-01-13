using System.Runtime.InteropServices;

namespace WTFIsTheBatteryAt
{

    internal static class Program
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public static string version = "1.0.6";

        public static bool debug = false;

        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

#if DEBUG
            debug = true;
#endif

            string[] argv = Environment.GetCommandLineArgs();

            if(argv.Length > 1) if (argv[1] == "--debug")
                debug = true;

            if (debug) AllocConsole();

            Console.WriteLine($"WTFIsTheBatteryAt {version} - debug");

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}