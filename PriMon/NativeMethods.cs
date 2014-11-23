using System;
using System.Runtime.InteropServices;
using PriMon.WinAPI;

namespace PriMon
{
    static class NativeMethods
    {
        public const int EnumCurrentSettings = -1;

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern bool EnumDisplayDevices(IntPtr lpDevice, int iDevNum, ref DisplayDevice lpDisplayDevice, int dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern bool EnumDisplayDevices(string lpszDeviceName, int iDevNum, ref DisplayDevice lpDisplayDevice, int dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern int EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DevMode lpDevMode);

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern bool EnumDisplaySettingsEx(string lpszDeviceName, int iModeNum, ref DevMode lpDevMode, uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern ReturnCode ChangeDisplaySettings(ref DevMode lpDevMode, int dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern ReturnCode ChangeDisplaySettingsEx(string lpszDeviceName, ref DevMode lpDevMode, IntPtr hwnd, ChangeDisplaySettingsFlags dwflags, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern ReturnCode ChangeDisplaySettingsEx(string lpszDeviceName, IntPtr lpDevMode, IntPtr hwnd, ChangeDisplaySettingsFlags dwflags, IntPtr lParam);

    }
}