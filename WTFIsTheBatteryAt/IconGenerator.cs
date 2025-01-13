using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WTFIsTheBatteryAt
{
    public class IconGenerator
    {
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        public void Generate(string value, Font font, Color colour, Point offset, NotifyIcon tray)
        {
            Bitmap bmp = new Bitmap(16, 16);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.DrawString(value, font, new SolidBrush(colour), offset.X, offset.Y);

            IntPtr _handle = bmp.GetHicon();
            Icon output;

            Icon _temp = Icon.FromHandle(_handle);
            output = _temp.Clone() as Icon;
            DestroyIcon(_handle);

            tray.Icon = output;

            output.Dispose();
            graphics.Dispose();
            // DISPOSE PROPERLY!!! PLEASE!!! WINDOWS HATES IT IF YOU HAVE TOO MANY GDI+ OBJECTS!!! AHHH!!!
        }

    }
}
