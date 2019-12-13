using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Runtime.InteropServices;

namespace ProjectGreenEnvironment
{
    static class Program
    {

        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(int vKey);


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Check if setup has been completed
            var environment = new GreenEnvironment();
            if (!Directory.Exists(environment.GetFolderPath()))
            {
                FrmSetup setup = new FrmSetup();
                
                string PeerName = default(string);
                setup.NameRecieved += (s, e) => {
                    PeerName = (string)s;
                };

                setup.ShowDialog(); 
                environment.Setup( PeerName );
            }

            environment.Load();

            // Init bluetooth listener
            //environment.StartListening();


            var paired = environment.FindPairedDevices();
            environment.ConnectTo(paired.First().DeviceAddress);

            new System.Threading.Thread(() => {
                System.Threading.Thread.Sleep(5000);
                System.Diagnostics.Debugger.Break();

                environment.Broadcast("Hej Hans");
            }).Start();

           
            //environment.FindPairedDevices();


            // Wait for callbacks
            while (true)
            {
                if (GetAsyncKeyState(0x78))
                {
                    //TODO: Send file
                }
            }

            

        }
    }
}
