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

                if (device.Authenticated)
                {
                    // set pin of device to connect with
                    localClient.SetPin("1234");
                    // async connection method
                    localClient.BeginConnect(device.DeviceAddress, BluetoothService.SerialPort, new AsyncCallback(Connect), device);
                }
            }
        }

        private static void Connect(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                // client is connected now :)
                MessageBox.Show("Forbundet");
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


            // mac is mac address of local bluetooth device
            BluetoothEndPoint localEndpoint = new BluetoothEndPoint(BluetoothAddress.Parse(GetBTMacAddress().ToString()), BluetoothService.SerialPort);
            // client is used to manage connections
            localClient = new BluetoothClient(localEndpoint);
            // component is used to manage device discovery
            BluetoothComponent localComponent = new BluetoothComponent(localClient);
            // async methods, can be done synchronously too
            localComponent.DiscoverDevicesAsync(255, true, true, true, true, null);
            localComponent.DiscoverDevicesProgress += new EventHandler<DiscoverDevicesEventArgs>(component_DiscoverDevicesProgress);
            localComponent.DiscoverDevicesComplete += new EventHandler<DiscoverDevicesEventArgs>(component_DiscoverDevicesComplete);






            // Init bluetooth listener
            //environment.StartListening();


            //new System.Threading.Thread(() => {
            //    System.Threading.Thread.Sleep(5000);
            //    System.Diagnostics.Debugger.Break();

            //    environment.Broadcast("Hej Hans");
            //}).Start();


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


        public static PhysicalAddress GetBTMacAddress()
        {

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {

                // Only consider Bluetooth network interfaces
                if (nic.NetworkInterfaceType != NetworkInterfaceType.FastEthernetFx &&
                    nic.NetworkInterfaceType != NetworkInterfaceType.Wireless80211)
                {

                    return nic.GetPhysicalAddress();
                }
            }
            return null;
        }
    }
}
