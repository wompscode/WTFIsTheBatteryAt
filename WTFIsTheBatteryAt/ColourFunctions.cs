namespace WTFIsTheBatteryAt
{
    internal static class ColourFunctions
    {
        public static Color IdealTextColor(Color bg)
        {
            // Borrowed from https://www.codeproject.com/Articles/16565/Determining-Ideal-Text-Color-Based-on-Specified-Ba (thanks guys!)
            int nThreshold = 105;
            int bgDelta = Convert.ToInt32((bg.R * 0.299) + (bg.G * 0.587) +
                                          (bg.B * 0.114));

            Color foreColor = (255 - bgDelta < nThreshold) ? Color.Black : Color.White;
            return foreColor;
        }
    }
}
