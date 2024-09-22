using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WTFIsTheBatteryAt
{
    internal static class Notifications
    {
        public static void ShowNotification(NotifyIcon notify, string title, string text, int iconType = 0)
        {
            switch (iconType)
            {
                case 0:
                    notify.BalloonTipIcon = ToolTipIcon.None;
                    break;
                case 1:
                    notify.BalloonTipIcon = ToolTipIcon.Info;
                    break;
                case 2:
                    notify.BalloonTipIcon = ToolTipIcon.Warning;
                    break;
                case 3:
                    notify.BalloonTipIcon = ToolTipIcon.Error;
                    break;
            }

            notify.BalloonTipText = text;
            notify.BalloonTipTitle = title;
            notify.ShowBalloonTip(0);
        }
    }
}
