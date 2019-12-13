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

        private static List<BluetoothDeviceInfo> deviceList = new List<BluetoothDeviceInfo>();
        private static BluetoothClient localClient;

        private static void component_DiscoverDevicesProgress(object sender, DiscoverDevicesEventArgs e)
        {
            // log and save all found devices
            for (int i = 0; i < e.Devices.Length; i++)
            {
                deviceList.Add(e.Devices[i]);
            }
        }
        private static void component_DiscoverDevicesComplete(object sender, DiscoverDevicesEventArgs e)
        {
            // get a list of all paired devices
            BluetoothDeviceInfo[] paired = localClient.DiscoverDevices(255, false, true, false, false);
            // check every discovered device if it is already paired 
            foreach (BluetoothDeviceInfo device in deviceList)
            {
                bool isPaired = false;
                for (int i = 0; i < paired.Length; i++)
                {
                    if (device.Equals(paired[i]))
                    {
                        isPaired = true;
                        break;
                    }
                }

                // if the device is not paired, pair it!
                if (!isPaired)
                {
                    // replace DEVICE_PIN here, synchronous method, but fast
                    isPaired = BluetoothSecurity.PairRequest(device.DeviceAddress, "1234");
                    if (isPaired)
                    {
                        // now it is paired
                        if (device.Authenticated)
                        {
                            
                        }
                    }
                    else
                    {
                        // pairing failed
                    }
                }
            }

            MessageBox.Show("All devices has been paired!");
        }

        private static void Connect(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                // client is connected now :)
                MessageBox.Show("Forbundet");
            }
        }

        private static void AcceptConnection(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                BluetoothClient remoteDevice = ((BluetoothListener)result.AsyncState).EndAcceptBluetoothClient(result);
                MessageBox.Show("En lort har forbundet til dig");
            }
        }

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


            // Create listener
            var localBtAddr = BluetoothAddress.Parse(GetBTMacAddress().ToString());
            BluetoothListener listener = new BluetoothListener(localBtAddr, BluetoothService.SerialPort);
            listener.SetPin(localBtAddr, "1234");
            listener.Start(10);
            listener.BeginAcceptBluetoothClient(new AsyncCallback(AcceptConnection), listener);


            // Wait for callbacks
            bool justPressedAKey = false;

            while (true)
            {
                if (GetAsyncKeyState(0x78)) // F9 for at parre sig.
                {
                    if (!justPressedAKey)
                    {
                        BluetoothEndPoint localEndpoint = new BluetoothEndPoint(BluetoothAddress.Parse(GetBTMacAddress().ToString()), BluetoothService.SerialPort);
                        localClient = new BluetoothClient(localEndpoint);
                        BluetoothComponent localComponent = new BluetoothComponent(localClient);
                        localComponent.DiscoverDevicesAsync(255, true, true, true, true, null);
                        localComponent.DiscoverDevicesProgress += new EventHandler<DiscoverDevicesEventArgs>(component_DiscoverDevicesProgress);
                        localComponent.DiscoverDevicesComplete += new EventHandler<DiscoverDevicesEventArgs>(component_DiscoverDevicesComplete);
                    }

                    justPressedAKey = true;
                }

                else if (GetAsyncKeyState(0x79)) // F10 for at forbinde sig til at devices.
                {
                    if (!justPressedAKey)
                    {
                        foreach (var device in deviceList)
                        {
                            if (device.Authenticated)
                            {
                                BluetoothEndPoint localEndpoint = new BluetoothEndPoint(BluetoothAddress.Parse(GetBTMacAddress().ToString()), BluetoothService.SerialPort);
                                var c = new BluetoothClient(localEndpoint);
                                c.SetPin("1234");
                                c.BeginConnect(device.DeviceAddress, BluetoothService.SerialPort, new AsyncCallback(Connect), device);
                            }
                        }
                    }
                    justPressedAKey = true;
                }

                else justPressedAKey = false;
            }   
        }


        public static PhysicalAddress GetBTMacAddress()
        {

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {

                // Only consider Bluetooth network interfaces
                if (nic.Name.Contains("Bluetooth"))
                {
                    return nic.GetPhysicalAddress();
                }
            }
            return null;
        }
    }
}
