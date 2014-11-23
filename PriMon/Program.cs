using System;
using System.Windows.Forms;

namespace PriMon
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (PriMon priMon = new PriMon())
            {
                priMon.Display();

                Application.Run();
            }
        }
    }
}