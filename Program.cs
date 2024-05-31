using ASRS.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetroFramework.Controls;
using System.Windows.Forms;
using ASRS.Properties;

namespace ASRS
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Manager());

        }
    }
}
