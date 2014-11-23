using System;

namespace PriMon.WinAPI
{
    [Flags]
    public enum ChangeDisplaySettingsFlags : int
    {
        CdsNone = 0,
        CdsUpdateRegistry = 0x00000001,
        CdsTest = 0x00000002,
        CdsFullscreen = 0x00000004,
        CdsGlobal = 0x00000008,
        CdsSetPrimary = 0x00000010,
        CdsVideoParameters = 0x00000020,
        CdsEnableUnsafeModes = 0x00000100,
        CdsDisableUnsafeModes = 0x00000200,
        CdsReset = 0x40000000,
        CdsResetEx = 0x20000000,
        CdsNoReset = 0x10000000
    }

}
