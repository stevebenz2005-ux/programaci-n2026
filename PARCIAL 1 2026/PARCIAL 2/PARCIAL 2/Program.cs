using System;
using System.Windows.Forms;

namespace PARCIAL_2
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new frmusuario());
        }
    }
}