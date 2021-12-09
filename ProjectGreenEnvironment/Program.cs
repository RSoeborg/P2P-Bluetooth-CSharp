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
            var connectedDevices = new List<BluetoothDeviceInfo>();

            // Create bluetooth handler
            var bluetooth = new BluetoothHandler("8724");
            Console.WriteLine($"Welcome back, mr. {environment.PeerName}");
            
            // Discover and connect to all "approved" devices"
            bluetooth.DiscoverProgress += (s, e) => {
                var devices = (BluetoothDeviceInfo[])s;
                foreach (var device in devices)
                {
                    var btView = new BluetoothDeviceView(device);

                    if (!btView.Device.Remembered) continue;
                    if (connectedDevices.Any(connectedDevice => connectedDevice.DeviceName == btView.Device.DeviceName)) continue;

                    Console.WriteLine("Connecting to ... " + btView.Device.DeviceName);
                    bluetooth.BeginConnect(btView.Device);
                }
            };

            bluetooth.DiscoverComplete += (s, e) => {
                // Permanent discovering...
                bluetooth.BeginDiscoveringDevices();
            };

            bluetooth.ConnectedTo += (s, e) => {
                var Response = (BluetoothConnectedData)((IAsyncResult)s).AsyncState;
                var connectedToClient = new ConnectedBluetoothDeviceView(Response.Device)
                    {Client = Response.SocketClient};
                
                connectedDevices.Add(connectedToClient.Device);
                Console.WriteLine("Successfully connected to " + connectedToClient.Device.DeviceName);
            };
            

            // Accept incoming connections
            bluetooth.AcceptedConnection += (s, e) => {
                var device = (BluetoothClient)s;
                
                // Start a new thread to listen to the device forever.
                new Thread(() =>
                {
                    bluetooth.StartListeningTo(device);
                }).Start();
            };

            // 
            bluetooth.RecievedData += (s, e) =>
            {
                // On data receive
                // Save data to file
                var data = (BluetoothData)s;
                environment.SaveFileData(data.Content);
            };


            // Start the bluetooth listening.
            bluetooth.StartListener();
            bluetooth.BeginDiscoveringDevices();
            

            while (true) {  Thread.Sleep(15); }
        }


    }
}
