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

            environment.Load();

            // Important stops for fuckups
            bool parring = false;
            bool connecting = false;
            string lastSentData = "";
            bool isPaired = false;

            // Create handler and subscribe to events 
            var pairedDevices = new List<BluetoothDeviceInfo>();
            var connectionClients = new List<BluetoothClient>();

            BluetoothHandler bluetooth = new BluetoothHandler("8724");
            bluetooth.DiscoverComplete += (s, e) => {
                var bluetoothDevices = (List<BluetoothDeviceInfo>)s;
                var newDevices = bluetoothDevices.Where(c => !pairedDevices.Contains(c));

                foreach (var newDevice in newDevices)
                {
                    if (newDevice.Connected)
                    {
                        BluetoothEndPoint localEndpoint = new BluetoothEndPoint(bluetooth.GetBluetoothAddress(), BluetoothService.SerialPort);
                        var c = new BluetoothClient(localEndpoint);
                        connectionClients.Add(c);

                        new Thread(() =>
                        {
                            bluetooth.StartListeningTo(c);
                        }).Start();
                        MessageBox.Show($"Computeren lytter nu til {c.RemoteMachineName}");
                    }
                }

                pairedDevices.AddRange(newDevices);

                //var newDevices = bluetoothDevices.Where(c => !pairedDevices.Contains(c)).ToArray();
                //bluetooth.PairDevices( newDevices );
                //pairedDevices.AddRange(bluetoothDevices.Where(c => c.Authenticated).ToList());

                MessageBox.Show($"{pairedDevices.Count} device(s) is now paired.");

                if (pairedDevices.Count > 0)
                {
                    isPaired = true;
                }

                parring = false;
            };
            bluetooth.RecievedData += (s, e) => 
            {
                var data = (BluetoothData)s;
                environment.SaveFileData(data.Content);
                MessageBox.Show("Maple 2019: New update available", "Maple 2019", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            bluetooth.ConnectedTo += (s, e) => {
                var connData = (BluetoothConnectedData)((IAsyncResult)s).AsyncState;

                if (connData.SocketClient.Connected && connData.Device.Authenticated)
                {
                    connectionClients.Add(connData.SocketClient);
                    MessageBox.Show($"Forbundet til en {connData.Device.DeviceName}.");
                } else
                {
                    MessageBox.Show("Fejl ved at forbinde til en device.");
                }

                connecting = false; 
            };
            bluetooth.AcceptedConnection += (s, e) => {
                var device = (BluetoothClient)s;
                new Thread(() => 
                {
                    bluetooth.StartListeningTo(device);
                }).Start();
                MessageBox.Show($"Computeren lytter nu til {device.RemoteMachineName}");
            };

            // Listen to events.
            bluetooth.StartListener();
            
            // Wait for callbacks
            bool justPressedAKey = false;
            while (true)
            {
                if (GetAsyncKeyState(0x78)) // F9 for at parre sig.
                {
                    if (!justPressedAKey && !parring)
                    {
                        bluetooth.BeginDiscoveringDevices();
                        parring = true;
                    }

                    justPressedAKey = true;
                }

                else if (GetAsyncKeyState(0x79)) // F10 for at forbinde sig til at devices.
                {
                    if (!isPaired)
                    {
                        MessageBox.Show("Du skal parre først!");
                    }
                    else if (!justPressedAKey && !connecting)
                    {
                        connecting = true;

                        if (pairedDevices.Count == 0)
                        {
                            MessageBox.Show("Ingen parrede devices.");
                            continue;
                        }

                        foreach (var pairedDevice in pairedDevices)
                        {
                            if (!pairedDevice.Connected)
                            {
                                bluetooth.BeginConnect(pairedDevice);
                            }
                        }
                    }
                    justPressedAKey = true;
                }

                else if (GetAsyncKeyState(0x7A))
                {
                    if (!justPressedAKey && !lastSentData.Equals(environment.MyFile()))
                    {
                        lastSentData = environment.MyFile();

                        // Broadcast file.
                        foreach (var client in connectionClients)
                        {
                            bluetooth.SendData(environment.MyFile(), client);
                        }
                    }

                    justPressedAKey = true;
                }


                else justPressedAKey = false;
            }   
        }

        
    }
}
