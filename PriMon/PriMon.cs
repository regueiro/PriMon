using System;
using System.Drawing;
using System.Windows.Forms;
using PriMon.Properties;

namespace PriMon
{
    class PriMon : IDisposable
    {
        private readonly NotifyIcon icon;

        internal PriMon()
        {
            icon = new NotifyIcon();
        }

        public void Dispose()
        {
            icon.Dispose();
        }

        public void Display()
        {
            var monitorIcon = Resources.monitor;
            icon.Icon = Icon.FromHandle(monitorIcon.GetHicon());
            icon.Text = Resources.PriMon_Display_Change_Primary_Monitor;
            icon.Visible = true;

            icon.ContextMenu = new Menu();
        }

    }
}