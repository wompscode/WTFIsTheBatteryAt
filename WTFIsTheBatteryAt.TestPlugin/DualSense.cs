using HidSharp;
using static WTFIsTheBatteryAt.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WTFIsTheBatteryAt.DualSensePlugin
{
    public static class DualSense
    {
        public static int controllerPlayer = 0;
        public static bool dualsenseStarted = false;

        public static Color dualsenseColor = Properties.Settings.Default.LightbarColour;
        public static IconGenerator icon = new IconGenerator();
        public static int lastBatteryPercent = 0;

        public static int devLength = 0;
        public static string devID = "";
        public static bool devROE = false;
        public static bool devBT = false;
        public static int devNum = 0;
        public static bool devBTInit = false;
        public static DeviceStream? dev;

        public static int devBatteryPercent = 0;
        public static BatteryState devBatteryState = 0;
        public static void DS_GetDev(int controller = 0)
        {
            Log($"DS_GetDev(): Reached.");
            if (dev != null)
            {
                Log($"DS_GetDev(): Previous dev present - disposing.");
                dev.Dispose();
                devBTInit = false;
            }
            List<HidDevice> devices = new List<HidDevice>();
            foreach (var dev in DeviceList.Local.GetHidDevices())
            {
                if (dev.VendorID == 1356 && dev.ProductID == 3302)
                {
                    devices.Add(dev);
                    Log($"DS_GetDev(): DualSense detected (1356&3302)");

                    // DualSense
                }
                else if (dev.VendorID == 1356 && dev.ProductID == 3570)
                {
                    devices.Add(dev);
                    Log($"DS_GetDev(): DualSense Edge detected (1356&3570)");

                    // DualSense Edge
                }
            }

            try
            {
                dev = devices[controller].Open();
                devID = devices[controller].DevicePath;
                devROE = devices[controller].VendorID == 1536 && devices[controller].ProductID == 3570;
                devLength = devices[controller].GetMaxOutputReportLength();
                devBTInit = devBTInit ? devBTInit : false;
                devNum = controller;

                if (!devROE)
                    devBT = devLength >= 78;
                else
                    devBT = devLength >= 94;
                Log($"DS_GetDev(): New controller info: " +
                    $"{(devROE ? "DualSense Edge" : "DualSense")}: {devNum} {(devBT ? "(BT)" : "(USB)")}");
            }
            catch (Exception ex)
            {
                Log($"DS_GetDev(): Run into problem connecting controller: \n{ex.StackTrace}");
                throw new Exception("Couldn't connect controller.\n" + ex.StackTrace);
            }
        }
        public enum BatteryState
        {
            // https://github.com/WujekFoliarz/Wujek-Dualsense-API/blob/master/Wujek%20Dualsense%20API/BatteryState.cs

            POWER_SUPPLY_STATUS_DISCHARGING = 0x0,
            POWER_SUPPLY_STATUS_CHARGING = 0x2,
            POWER_SUPPLY_STATUS_FULL = 0x1,
            POWER_SUPPLY_STATUS_NOT_CHARGING = 0xb,
            POWER_SUPPLY_STATUS_ERROR = 0xf,
            POWER_SUPPLY_TEMP_OR_VOLTAGE_OUT_OF_RANGE = 0xa,
            POWER_SUPPLY_STATUS_UNKNOWN = 0x0
        }
        public static void DS_ReadData()
        {
            Log($"DS_ReadData(): Reached.");

            try
            {
                if (dev == null) return;
                Log($"DS_ReadData(): Device is not null.");

                byte[] deviceStates = new byte[devLength];
                dev.Read(deviceStates);
                int tempOffset = devBT ? 1 : 0;

                byte faceButtons = deviceStates[8 + tempOffset];
                Log($"DS_ReadData(): X: {(faceButtons & (1 << 5)) != 0}"); // For debug purposes.


                if (devBT)
                {
                    Log($"DS_ReadData(): STATE: {(BatteryState)((byte)(deviceStates[53 + tempOffset] & 0xF0) >> 4)}");
                    Log($"DS_ReadData(): PERCENT: {Math.Min((int)((deviceStates[53 + tempOffset] & 0x0F) * 10 + 5), 100)}%");

                    devBatteryState = (BatteryState)((byte)(deviceStates[53 + tempOffset] & 0xF0) >> 4);
                    devBatteryPercent = Math.Min((int)((deviceStates[53 + tempOffset] & 0x0F) * 10 + 5), 100);
                }
                else
                {
                    devBatteryState = BatteryState.POWER_SUPPLY_STATUS_UNKNOWN;
                    devBatteryPercent = 100;
                }

            }
            catch (Exception exception)
            {

                Log(exception.Message);

                OnDisconnect();
                DS_Dispose(devNum);
            }
        }

        public static void DS_WriteData(Color col)
        {
            Log("DS_WriteData(): Reached.");

            byte[] outputDevStates = new byte[devLength];

            if (devBT)
            {
                Log($"DS_WriteData(): Device is connected via BT");

                int[] rtf = new int[7];
                int[] ltf = new int[7];
                outputDevStates[0] = 0x31;
                outputDevStates[1] = 2;
                outputDevStates[2] = (byte)0xFC;
                if (devBTInit == false)
                {
                    outputDevStates[3] = 0x1 | 0x2 | 0x4 | 0x8 | 0x10 | 0x40;
                    devBTInit = true;
                }
                else
                    outputDevStates[3] = (byte)0x57;
                outputDevStates[4] = (byte)0; // right low freq motor 0-255
                outputDevStates[5] = (byte)0; // left low freq motor 0-255
                outputDevStates[10] = (byte)0; //microphone led
                outputDevStates[11] = (byte)0x10;
                outputDevStates[12] = (byte)0;
                outputDevStates[13] = (byte)rtf[0];
                outputDevStates[14] = (byte)rtf[1];
                outputDevStates[15] = (byte)rtf[2];
                outputDevStates[16] = (byte)rtf[3];
                outputDevStates[17] = (byte)rtf[4];
                outputDevStates[18] = (byte)rtf[5];
                outputDevStates[21] = (byte)rtf[6];
                outputDevStates[23] = (byte)0;
                outputDevStates[24] = (byte)ltf[0];
                outputDevStates[25] = (byte)ltf[1];
                outputDevStates[26] = (byte)ltf[2];
                outputDevStates[27] = (byte)ltf[3];
                outputDevStates[28] = (byte)ltf[4];
                outputDevStates[29] = (byte)ltf[5];
                outputDevStates[32] = (byte)ltf[6];
                outputDevStates[40] = (byte)0;
                outputDevStates[43] = (byte)0;
                outputDevStates[44] = (byte)0;
                outputDevStates[45] = (byte)0;
                outputDevStates[45] = (byte)0;
                outputDevStates[46] = (byte)col.R;
                outputDevStates[47] = (byte)col.G;
                outputDevStates[48] = (byte)col.B;

                uint crcChecksum = CRC32.ComputeCRC32(outputDevStates, 74);
                byte[] checksum = BitConverter.GetBytes(crcChecksum);
                Array.Copy(checksum, 0, outputDevStates, 74, 4);
            }
            else
            {

                Log($"DS_WriteData(): Device is not connected via BT");

                int[] rtf = new int[7];
                int[] ltf = new int[7];
                outputDevStates[0] = 2;
                outputDevStates[1] = (byte)0xFC;
                outputDevStates[2] = (byte)0x57;
                outputDevStates[3] = (byte)0; // right low freq motor 0-255
                outputDevStates[4] = (byte)0; // left low freq motor 0-255
                outputDevStates[5] = 0x7C; // <-- headset volume
                outputDevStates[6] = (byte)100; // <-- speaker volume
                outputDevStates[7] = (byte)35; // <-- mic volume
                outputDevStates[8] = (byte)0x31; // <-- audio output
                outputDevStates[9] = (byte)0; //microphone led
                outputDevStates[10] = (byte)0x10;
                outputDevStates[11] = (byte)0;
                outputDevStates[12] = (byte)rtf[0];
                outputDevStates[13] = (byte)rtf[1];
                outputDevStates[14] = (byte)rtf[2];
                outputDevStates[15] = (byte)rtf[3];
                outputDevStates[16] = (byte)rtf[4];
                outputDevStates[17] = (byte)rtf[5];
                outputDevStates[20] = (byte)rtf[6];
                outputDevStates[22] = (byte)0;
                outputDevStates[23] = (byte)ltf[0];
                outputDevStates[24] = (byte)ltf[1];
                outputDevStates[25] = (byte)ltf[2];
                outputDevStates[26] = (byte)ltf[3];
                outputDevStates[27] = (byte)ltf[4];
                outputDevStates[28] = (byte)ltf[5];
                outputDevStates[31] = (byte)ltf[6];
                outputDevStates[39] = (byte)0;
                outputDevStates[41] = (byte)0;
                outputDevStates[42] = (byte)0;
                outputDevStates[43] = (byte)0;
                outputDevStates[44] = (byte)0;
                outputDevStates[45] = (byte)col.R;
                outputDevStates[46] = (byte)col.G;
                outputDevStates[47] = (byte)col.B;
            }

            try
            {
                Log($"DS_WriteData(): Trying to write to device");

                if (dev != null) dev.WriteAsync(outputDevStates, 0, devLength);
                else
                {
                    Log("DS_WriteData(): Device is null?");
                }
            }
            catch (Exception exception)
            {
                Log($"DS_WriteData(): Failed to write to device");
                if (exception.StackTrace != null) Log(exception.StackTrace);
            }
        }

        public static void DS_Dispose(int num)
        {
            if (dev != null)
            {
                Log("DS_Dispose(): Colour reset.");

                DS_WriteData(Color.Blue);

                dev.Dispose();
                Log("DS_Dispose(): Device disposed.");
            };
        }

        public static void OnDisconnect()
        {
            Log("OnDisconnect(): Disconnected.");
            if (!dualsenseStarted) return;
            dualsenseStarted = false;
            devBTInit = false;
            lastBatteryPercent = 0;
        }
    }
}
