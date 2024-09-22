using System;
namespace WTFIsTheBatteryAt
{
    internal static class Logging
    {
        public static void Log(string text = "This string will never ever be shown. In fact, it's entire existence is just a placeholder to know that the user hasn't entered anything in. I could really write whatever I wanted in here, couldn't I? Probably. Does it matter? Does anything matter? Oh god.")
        {
            if (!Program.debug) return;
            if (text == null) return;

            if (text == "This string will never ever be shown. In fact, it's entire existence is just a placeholder to know that the user hasn't entered anything in. I could really write whatever I wanted in here, couldn't I? Probably. Does it matter? Does anything matter? Oh god.") return;

            DateTime current = DateTime.Now;
            Console.WriteLine($"{current.ToShortDateString()} {current.ToLongTimeString()}: {text}");
        }
    }
}
