namespace PriMon
{
    public class Monitor
    {
        public string Name { get; set; }
        public string DeviceName { get; set; }
        public bool Primary { get; set; }

        public Monitor(string name, string deviceName, bool primary)
        {
            Name = name;
            DeviceName = deviceName;
            Primary = primary;
        }
    }
}