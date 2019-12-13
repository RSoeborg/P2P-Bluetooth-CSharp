using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGreenEnvironment
{
    class BluetoothHandler
    {
        private readonly string Device_Pin;

        public BluetoothHandler(string Device_Pin)
        {
            this.Device_Pin = Device_Pin;

            BluetoothEndPoint localEndpoint = new BluetoothEndPoint(BluetoothAddress.Parse(GetBTMacAddress().ToString()), BluetoothService.SerialPort);
            localClient = new BluetoothClient(localEndpoint);
        }

        private List<BluetoothDeviceInfo> deviceList = new List<BluetoothDeviceInfo>();
        private BluetoothClient localClient;
        
        public event EventHandler DiscoverComplete;
        public event EventHandler AcceptedConnection;
        public event EventHandler ConnectedTo;

        public event EventHandler RecievedData;
        
        private void component_DiscoverDevicesProgress(object sender, DiscoverDevicesEventArgs e)
        {
            // log and save all found devices
            for (int i = 0; i < e.Devices.Length; i++)
            {
                deviceList.Add(e.Devices[i]);
            }
        }
        private void component_DiscoverDevicesComplete(object sender, DiscoverDevicesEventArgs e)
        {
            DiscoverComplete?.Invoke(deviceList, EventArgs.Empty);
        }

        private void Connect(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                ConnectedTo.Invoke(null, EventArgs.Empty);
            }
        }
        private void AcceptConnection(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                BluetoothClient remoteDevice = ((BluetoothListener)result.AsyncState).EndAcceptBluetoothClient(result);
                AcceptedConnection.Invoke(remoteDevice, EventArgs.Empty);
            }
        }
        
        public void StartListeningTo(BluetoothClient client, int delay=50)
        {
            while (client.Connected)
            {
                if (client.Available > 0)
                {
                    using (var sr = new StreamReader(client.GetStream()))
                    {
                        var data = new BluetoothData()
                        {
                            Contents = sr.ReadToEnd(),
                            Sender = client
                        };

                        RecievedData?.Invoke(data, EventArgs.Empty);
                    }
                }
                
                System.Threading.Thread.Sleep(delay);
            }
        }

        public void SendData(string Content, BluetoothClient client)
        {
            using (var sw = new StreamWriter(client.GetStream()))
            {
                sw.Write(Content);
                sw.Flush();
            }
        }


        public void BeginDiscoveringDevices()
        {
            deviceList.Clear();
            BluetoothComponent localComponent = new BluetoothComponent(localClient);
            localComponent.DiscoverDevicesAsync(255, true, true, true, true, null);
            localComponent.DiscoverDevicesProgress += new EventHandler<DiscoverDevicesEventArgs>(component_DiscoverDevicesProgress);
            localComponent.DiscoverDevicesComplete += new EventHandler<DiscoverDevicesEventArgs>(component_DiscoverDevicesComplete);
        }
        public void PairDevices(BluetoothDeviceInfo[] devicesToPair)
        {
            // get a list of all paired devices
            BluetoothDeviceInfo[] paired = localClient.DiscoverDevices(255, false, true, false, false);

            // check every discovered device if it is already paired 
            foreach (BluetoothDeviceInfo device in devicesToPair)
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
                    // Fast synchronous method for paring.
                    isPaired = BluetoothSecurity.PairRequest(device.DeviceAddress, Device_Pin);
                    if (isPaired)
                    {
                        // now it is paired
                    }
                    else
                    {
                        // pairing failed
                    }
                }
            }
        }
        public void StartListener()
        {
            // Create listener
            BluetoothListener listener = new BluetoothListener(GetBluetoothAddress(), BluetoothService.SerialPort);
            listener.SetPin(GetBluetoothAddress(), Device_Pin);
            listener.Start(10);
            listener.BeginAcceptBluetoothClient(new AsyncCallback(AcceptConnection), listener);
        }

        public void BeginConnect(BluetoothDeviceInfo device, string overridePin = "")
        {
            if (!device.Authenticated) 
                throw new Exception("Tried to connect to a device that is not authenticated."); 
            
            BluetoothEndPoint localEndpoint = new BluetoothEndPoint(BluetoothAddress.Parse(GetBTMacAddress().ToString()), BluetoothService.SerialPort);
            var c = new BluetoothClient(localEndpoint);
            c.SetPin(overridePin == string.Empty ? Device_Pin : overridePin);
            c.BeginConnect(device.DeviceAddress, BluetoothService.SerialPort, new AsyncCallback(Connect), device);
        }

        private BluetoothAddress GetBluetoothAddress()
        {
            return BluetoothAddress.Parse(GetBTMacAddress().ToString());
        }
        private PhysicalAddress GetBTMacAddress()
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
