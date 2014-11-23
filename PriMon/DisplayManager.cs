using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CCD.Enum;
using CCD.Struct;
using PriMon.WinAPI;

namespace PriMon
{
    static class DisplayManager
    {
        public static void MakePrimary(Monitor monitor)
        {
            var offsetX = 0;
            var offsetY = 0;

            var devices = GetDevices();

            var modes = new List<DevMode>();
            DevMode primaryDeviceMode = new DevMode();
            DisplayDevice primaryDevice = devices[0];
            foreach (var device in devices)
            {
                DevMode mode = new DevMode();
                mode.dmSize = (short)Marshal.SizeOf(mode);

                var resultOk = NativeMethods.EnumDisplaySettingsEx(device.DeviceName, NativeMethods.EnumCurrentSettings, ref mode, 0);

                if (resultOk)
                {
                    if (device.DeviceName.Equals(monitor.DeviceName))
                    {
                        primaryDevice = device;
                        primaryDeviceMode = mode;
                        offsetX = mode.dmPositionX;
                        offsetY = mode.dmPositionY;
                    }
                    modes.Add(mode);
                }

            }

            primaryDeviceMode.dmPositionX = 0;
            primaryDeviceMode.dmPositionY = 0;
            //primaryDeviceMode.dmFields = 0x20;

            var changedPrimary = NativeMethods.ChangeDisplaySettingsEx(monitor.DeviceName, ref primaryDeviceMode, IntPtr.Zero, (ChangeDisplaySettingsFlags.CdsSetPrimary | ChangeDisplaySettingsFlags.CdsUpdateRegistry | ChangeDisplaySettingsFlags.CdsNoReset), IntPtr.Zero);
            Console.WriteLine(changedPrimary);

            for (int i = 0; i < devices.Count; i++)
            {

                DevMode mode = modes[i];
                mode.dmSize = (short)Marshal.SizeOf(mode);
                mode.dmDriverExtra = 0;

                //Console.WriteLine(mode.dmDeviceName);
                //Console.WriteLine("Old offset X: " + mode.dmPositionX);
                //Console.WriteLine("Old offset Y: " + mode.dmPositionY);
                mode.dmPositionX -= offsetX;
                mode.dmPositionY = 0;
                mode.dmSize = (short)Marshal.SizeOf(mode);
                //mode.dmFields = 0x20;
                //Console.WriteLine("New offset X: " + mode.dmPositionX);
                //Console.WriteLine("New offset Y: " + mode.dmPositionY);

                if (devices[i].DeviceName.Equals(monitor.DeviceName))
                {
                    //                    primaryDeviceMode = mode;
                    //                    primaryDevice = devices[i];
                    //                    var changed = NativeMethods.ChangeDisplaySettingsEx(devices[i].DeviceName, ref mode, IntPtr.Zero, (ChangeDisplaySettingsFlags.CDS_SET_PRIMARY |
                    //ChangeDisplaySettingsFlags.CDS_UPDATEREGISTRY | ChangeDisplaySettingsFlags.CDS_NORESET), IntPtr.Zero);
                    //                    Console.WriteLine(changed);
                }
                else
                {
                    var changed = NativeMethods.ChangeDisplaySettingsEx(devices[i].DeviceName, ref mode, IntPtr.Zero, (ChangeDisplaySettingsFlags.CdsUpdateRegistry | ChangeDisplaySettingsFlags.CdsNoReset), IntPtr.Zero);
                    Console.WriteLine(changed);
                }

            }


            var changedPrimary1 = NativeMethods.ChangeDisplaySettingsEx(null, IntPtr.Zero, (IntPtr)null, ChangeDisplaySettingsFlags.CdsNone, (IntPtr)null);
            Console.WriteLine(changedPrimary1);

            return;
        }

        public static string GetDeviceName(int sourceId)
        {
            var monitors = new List<Monitor>();

            int numPathArrayElements;
            int numModeInfoArrayElements;

            var statusCode = CCD.Wrapper.GetDisplayConfigBufferSizes(QueryDisplayFlags.OnlyActivePaths, out numPathArrayElements,
                out numModeInfoArrayElements);


            var pathInfoArray = new DisplayConfigPathInfo[numPathArrayElements];
            var modeInfoArray = new DisplayConfigModeInfo[numModeInfoArrayElements];

            statusCode = CCD.Wrapper.QueryDisplayConfig(QueryDisplayFlags.OnlyActivePaths, ref numPathArrayElements, pathInfoArray, ref numModeInfoArrayElements, modeInfoArray);


            foreach (var displayConfigPathInfo in pathInfoArray)
            {
                if (displayConfigPathInfo.sourceInfo.id == sourceId)
                {
                    var displayConfigSourceDeviceName = new DisplayConfigTargetDeviceName
                    {
                        header = new DisplayConfigDeviceInfoHeader()
                        {
                            id = displayConfigPathInfo.targetInfo.id,
                            adapterId = displayConfigPathInfo.targetInfo.adapterId,
                            type = DisplayConfigDeviceInfoType.GetTargetName,
                            size = Marshal.SizeOf(typeof(DisplayConfigTargetDeviceName))
                        }
                    };
                    var returnCode = CCD.Wrapper.DisplayConfigGetDeviceInfo(ref displayConfigSourceDeviceName);

                    if (returnCode.Equals(StatusCode.Success))
                    {
                        return displayConfigSourceDeviceName.monitorFriendlyDeviceName;
                    }
                }

            }

            return null;
        }


        private static List<DisplayDevice> GetDevices()
        {
            var monitors = GetMonitors();


            var devices = new List<DisplayDevice>();

            int iDevNum = 0;
            bool resultOk = false;

            do
            {
                var device = new DisplayDevice();
                device.cb = Marshal.SizeOf(device);

                resultOk = NativeMethods.EnumDisplayDevices(IntPtr.Zero, iDevNum, ref device, 0);

                if (resultOk && device.StateFlags.HasFlag(DisplayDeviceStateFlags.AttachedToDesktop))
                {

                    var monitor = new DisplayDevice();
                    monitor.cb = Marshal.SizeOf(monitor);


                    NativeMethods.EnumDisplayDevices(device.DeviceName, 0, ref monitor, 0);
                    devices.Add(device);
                }

                iDevNum++;
            } while (resultOk);

            return devices;
        }



        public static IEnumerable<Monitor> GetMonitors()
        {
            var monitors = new List<Monitor>();

            int iDevNum = 0;
            bool resultOk = false;

            do
            {
                var device = new DisplayDevice();
                device.cb = Marshal.SizeOf(device);

                resultOk = NativeMethods.EnumDisplayDevices(IntPtr.Zero, iDevNum, ref device, 0);

                if (resultOk && device.StateFlags.HasFlag(DisplayDeviceStateFlags.AttachedToDesktop))
                {

                    var monitor = new DisplayDevice();
                    monitor.cb = Marshal.SizeOf(monitor);


                    NativeMethods.EnumDisplayDevices(device.DeviceName, 0, ref monitor, 0);

                    string name = GetDeviceName(iDevNum);

                    monitors.Add(new Monitor(name, device.DeviceName, device.StateFlags.HasFlag(DisplayDeviceStateFlags.PrimaryDevice)));
                }

                iDevNum++;
            } while (resultOk);

            return monitors;
        }
    }
}