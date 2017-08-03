using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace payloader
{
    static class Program {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            if (args.Length == 0) {                                                                        // If no arguments we run the GUI.
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new payloader());
            }
            else if (args.Length < 3 || args.Length > 3) magic.PrintUsage();                               // If arguments less or higher 3 we print a usage.
            else if (!magic.CheckArgs(args)) magic.PrintUsage();                                           // If argument input is wrong, we print a usage.
            else {
                magic.PrintWelcome();                                                                      // Show welcome.
                if (magic.ConnectAndSend(magic._ip, magic._port, magic._path)) magic.PrintByBy();          // Connect and send.
                else magic.PrintMeh();                                                                     // Report a error.
                return;                                                                                    // Close application.
            }
        }
    }
}
