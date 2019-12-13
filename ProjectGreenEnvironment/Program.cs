using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Runtime.InteropServices;
using InTheHand.Windows.Forms;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using System.Threading;
using System.Net.NetworkInformation;

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
                environment.Setup(PeerName);
            }

            // Create handler and subscribe to events 
            var pairedDevices = new List<BluetoothDeviceInfo>();

            BluetoothHandler bluetooth = new BluetoothHandler("8724");
            bluetooth.DiscoverComplete += (s, e) => {
                var bluetoothDevices = (List<BluetoothDeviceInfo>)s;
                bluetooth.PairDevices(bluetoothDevices.ToArray());
                MessageBox.Show("Devices has been paired.");
            };

            bluetooth.ConnectedTo += (s, e) => {
                MessageBox.Show("Du har forbundet til en.");
            };

            bluetooth.AcceptedConnection += (s, e) => {
                MessageBox.Show("Der er en der har forbundet til dig.");
            };

            // Listen to events.
            bluetooth.StartListener();
            
            // Wait for callbacks
            bool justPressedAKey = false;
            while (true)
            {
                if (GetAsyncKeyState(0x78)) // F9 for at parre sig.
                {
                    if (!justPressedAKey)
                    {
                        bluetooth.BeginDiscoveringDevices();
                    }

                    justPressedAKey = true;
                }

                else if (GetAsyncKeyState(0x79)) // F10 for at forbinde sig til at devices.
                {
                    if (!justPressedAKey)
                    {
                        if (pairedDevices.Count == 0)
                        {
                            MessageBox.Show("Ingen parrede devices.");
                            continue;
                        }

                        foreach (var pairedDevice in pairedDevices)
                        {
                            bluetooth.BeginConnect(pairedDevice);
                        }
                    }
                    justPressedAKey = true;
                }

                else justPressedAKey = false;
            }   
        }

        
    }
}
