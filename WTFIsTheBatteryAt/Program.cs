using System.Data;
using System.Runtime.InteropServices;
using static WTFIsTheBatteryAt.UpdateChecker;
using static WTFIsTheBatteryAt.Logging;
using System.Diagnostics;
namespace WTFIsTheBatteryAt
{

    internal static class Program
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public static string version = Application.ProductVersion;

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

            Log($"WTFIsTheBatteryAt {version}", "[init]");
            Log("Checking for update..", "[debug]");

            UpdateChecker.UpdateStatus updateStatus = null;
            Task<UpdateChecker.UpdateStatus> updateCheck = CheckForUpdateAsync(version);
            Task pauseCheck = updateCheck.ContinueWith(x => updateStatus = x.Result);
            pauseCheck.Wait();
            if(updateStatus != null)
            {
                if(updateStatus.state == VersionCheck.out_of_date)
                {
                    Log("Out of date! Prompting user to go to GitHub..", "[debug]");
                    DialogResult dialogResult = MessageBox.Show($"You are on version {updateStatus.current}, when the newest is {updateStatus.requested}. Would you like to go to GitHub?", "WTFIsTheBatteryAt", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Process page = new Process();
                        page.StartInfo.UseShellExecute = true;
                        page.StartInfo.FileName = "https://github.com/wompscode/WTFIsTheBatteryAt/releases";
                        page.Start();
                        Environment.Exit(0);
                    } else
                    {
                        Log("User skipped update.", "[debug]");
                    }
                } else
                {
                    Log("Up to date.", "[debug]");
                }
            } else
            {
                Log("UpdateCheck failed, updateStatus is still null - shouldn't happen, ignoring anyway..", "[debug]");
            }
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}