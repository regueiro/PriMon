
namespace PriMon.WinAPI
{
    public enum ReturnCode : int
    {
        DispChangeSuccessful = 0,
        DispChangeBaddualview = -6,
        DispChangeBadflags = -4,
        DispChangeBadmode = -2,
        DispChangeBadparam = -5,
        DispChangeFailed = -1,
        DispChangeNotupdated = -3,
        DispChangeRestart = 1
    }

}
