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
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var lastReceived = DateTime.Now;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Check if setup has been completed
            var environment = new GreenEnvironment();
            if (!Directory.Exists(GreenEnvironment.GetFolderPath()))
            {
                var setup = new FrmSetup();
                var PeerName = default(string);
                setup.NameRecieved += (s, e) => {
                    PeerName = (string)s;
                };

                setup.ShowDialog();
                environment.Setup(PeerName);
            }
            environment.Load();
            
            // Connected devices
            var connectedDevices = new List<ConnectedBluetoothDeviceView>();
            var attemptingConnectionsTo = new List<BluetoothDeviceInfo>();

            // Create bluetooth handler
            var bluetooth = new BluetoothHandler("8724");
            Console.WriteLine($"Welcome back, mr. {environment.PeerName}");

            // 
            Console.WriteLine("Who do you want to connect to?");
            var listenTo = Console.ReadLine();

            
            // Discover and connect to all "approved" devices"
            bluetooth.DiscoverProgress += (s, e) => {
                var devices = (BluetoothDeviceInfo[])s;
                foreach (var device in devices)
                {
                    var btView = new BluetoothDeviceView(device);

                    if (!btView.Device.Remembered) continue;
                    if (connectedDevices.Any(connectedDevice => connectedDevice.Device.DeviceName == btView.Device.DeviceName)) continue;
                    if (attemptingConnectionsTo.Any(connectedDevice => connectedDevice.DeviceName == btView.Device.DeviceName)) continue;

                    attemptingConnectionsTo.Add(btView.Device);

                    if (listenTo != btView.Device.DeviceName) continue;
                    Console.WriteLine("Connecting to ... " + btView.Device.DeviceName);
                    bluetooth.BeginConnect(btView.Device);
                }
            };

            bluetooth.DiscoverComplete += (s, e) => {
                // Permanent discovering...
                bluetooth.BeginDiscoveringDevices();
            };


            bluetooth.ConnectedTo += (s, e) => {
                try
                {
                    var Response = (BluetoothConnectedData) ((IAsyncResult) s).AsyncState;
                    var connectedToClient = new ConnectedBluetoothDeviceView(Response.Device)
                        {Client = Response.SocketClient};


                    connectedDevices.Add(connectedToClient);
                    attemptingConnectionsTo.Remove(connectedToClient.Device);

                    Console.WriteLine("Successfully connected to " + connectedToClient.Device.DeviceName);
                } catch (Exception) { }
            };
            

            // Accept incoming connections
            bluetooth.AcceptedConnection += (s, e) => {
                var device = (BluetoothClient)s;
                

                // Start a new thread to listen to the device forever.
                new Thread(() =>
                {
                    bluetooth.StartListeningTo(device);
                }).Start();

                Console.WriteLine($"Now listening to {device.RemoteMachineName}");
            };

            // 
            bluetooth.RecievedData += (s, e) =>
            {
                // On data receive
                // Save data to file
                var data = (BluetoothData)s;
                lastReceived = DateTime.Now;

                Console.WriteLine("Recieved file from " + data.Sender.RemoteMachineName);
                environment.SaveFileData(data.Content);
            };


            // Start the bluetooth listening.
            bluetooth.StartListener();
            bluetooth.BeginDiscoveringDevices();


            var count = 0;
            while (true)
            {
                Thread.Sleep(50);
                count++;

                if (lastReceived.AddSeconds(65) < DateTime.Now)
                {
                    Console.WriteLine("Attempting reconnect");
                    bluetooth.RetryListening();
                    lastReceived = DateTime.Now;
                }

                if (count <= 500) continue; // 1200
                Console.WriteLine("Broadcasting data!");

                // Broadcast!
                var toRemoveFromConnected = new List<ConnectedBluetoothDeviceView>();
                foreach (var connectedDevice in connectedDevices)
                {
                    foreach (var file in environment.AllFiles())
                    {
                        if (connectedDevice.Client.Connected)
                        {
                            bluetooth.SendData(file, connectedDevice.Client);
                            Thread.Sleep(8500);
                        }
                        else
                        {
                            toRemoveFromConnected.Add(connectedDevice);
                        }
                    }
                }

                foreach (var connectedBluetoothDeviceView in toRemoveFromConnected)
                    connectedDevices.Remove(connectedBluetoothDeviceView);
                
                count = 0;
            }
        }
        
    }
}
