using System;
using System.Windows.Forms;
using PriMon.Properties;

namespace PriMon
{
    sealed class Menu : ContextMenu
    {

        public Menu()
        {
            Populate();
        }

        private void Populate()
        {
            MenuItems.Clear();

            foreach (var monitor in DisplayManager.GetMonitors())
            {
                var display = new MenuItem { Text = monitor.Name, Checked = monitor.Primary };
                display.Click += (sender, args) => DisplayOnCheckStateChanged(sender, new ScreenEventArgs(monitor));

                MenuItems.Add(display);
            }

            // Separator.
            MenuItems.Add(new MenuItem("-"));

            // Exit.
            var item = new MenuItem { Text = Resources.ContextMenu_Display_Exit };
            item.Click += ExitClick;
            MenuItems.Add(item);
        }

        public void DisplayOnCheckStateChanged(object sender, ScreenEventArgs eventArgs)
        {
            DisplayManager.MakePrimary(eventArgs.Monitor);

            var senderItem = sender as MenuItem;

            Populate();

            //foreach (var ltoolStripMenuItem in (from object
            //                            item in senderItem.Owner.Items
            //                                    let ltoolStripMenuItem = item as ToolStripMenuItem
            //                                    where ltoolStripMenuItem != null
            //                                    where !item.Equals(senderItem)
            //                                    select ltoolStripMenuItem))
            //    (ltoolStripMenuItem).Checked = ltoolStripMenuItem.Equals(sender);
        }

        public static void ExitClick(object sender, EventArgs eventArgs)
        {
            Application.Exit();
        }

    }

    public delegate void DisplayOnCheckStateChanged(object sender, ScreenEventArgs args);

    public class ScreenEventArgs : EventArgs
    {
        public Monitor Monitor { get; set; }

        public ScreenEventArgs(Monitor monitor)
        {
            this.Monitor = monitor;
        }
    }

}
