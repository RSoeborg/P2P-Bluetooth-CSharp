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
using System.Windows.Forms;

namespace ProjectGreenEnvironment
{
    public class BluetoothHandler
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
        public event EventHandler DiscoverProgress;
        public event EventHandler ConnectedTo;

        public event EventHandler RecievedData;
        
        private void component_DiscoverDevicesProgress(object sender, DiscoverDevicesEventArgs e)
        {
            // log and save all found devices
            for (int i = 0; i < e.Devices.Length; i++)
            {
                if (e.Devices[i].Remembered)
                {
                    deviceList.Add(e.Devices[i]);
                }
            }

            DiscoverProgress?.Invoke(e.Devices, EventArgs.Empty);
        }
        private void component_DiscoverDevicesComplete(object sender, DiscoverDevicesEventArgs e)
        {
            DiscoverComplete?.Invoke(deviceList, EventArgs.Empty);
        }

        private void Connect(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                ConnectedTo.Invoke(result, EventArgs.Empty);
            }
        }
        private void AcceptConnection(IAsyncResult result)
        {
            if (!result.IsCompleted) return;
            BluetoothClient remoteDevice = ((BluetoothListener)result.AsyncState).EndAcceptBluetoothClient(result);
            AcceptedConnection.Invoke(remoteDevice, EventArgs.Empty);
        }
        
        public void StartListeningTo(BluetoothClient client, int delay=50)
        {
            StringBuilder builder = new StringBuilder();
            while (client.Connected)
            {
                if (client.Available > 0)
                {
                    byte[] buffer = new byte[client.Available];
                    client.GetStream().Read(buffer, 0, buffer.Length);

                    var recieved_data_buffer = Encoding.UTF8.GetString(buffer);
                    builder.Append(recieved_data_buffer);
                }

                if (builder.ToString().Replace("\0", string.Empty).EndsWith("$END"))
                {
                    var data = new BluetoothData()
                    {
                        Content = builder.ToString(),
                        Sender = client
                    };
                    RecievedData?.Invoke(data, EventArgs.Empty);
                    builder.Clear();
                }
                
                System.Threading.Thread.Sleep(delay);
            }
        }
        public void SendData(string Content, BluetoothClient client)
        {
            try
            {

                var stream = client.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes(Content);

                List<byte[]> chunks = new List<byte[]>();

                byte[] currentChunk = new byte[1024];
                int chunkIndex = 0;

                for (int i = 0; i < buffer.Length; i++)
                {
                    currentChunk[chunkIndex++] = buffer[i];

                    if (chunkIndex >= 1024)
                    {
                        chunks.Add(currentChunk);
                        chunkIndex = 0;
                        currentChunk = new byte[1024];
                    }
                }

                if (chunkIndex != 0) //part of a chunk (ending chunk)
                {
                    chunks.Add(currentChunk);
                }

                foreach (var chunk in chunks)
                {
                    stream.Write(chunk, 0, chunk.Length);
                    stream.Flush();
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception)
            {

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
            BluetoothDeviceInfo[] paired = localClient.DiscoverDevices(10, false, true, false, false);

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
                        if (device.Authenticated)
                        {
                            MessageBox.Show($"Paired korrekt med {device.DeviceName}");
                        } else
                        {
                            MessageBox.Show($"Pairing returned true men kunne ikke bonde med {device.DeviceName}");
                        }
                    }
                    else
                    {
                        // pairing failed
                        MessageBox.Show($"Paired failed {device.DeviceName}");

                    }
                }
            }
        }

        private BluetoothListener listener;
        public void StartListener()
        {
            // Create listener
            listener = new BluetoothListener(GetBluetoothAddress(), BluetoothService.SerialPort);
            //listener.SetPin(GetBluetoothAddress(), Device_Pin);
            listener.Start(10);
            listener.BeginAcceptBluetoothClient(new AsyncCallback(AcceptConnection), listener);
        }

        public void RetryListening()
        {
            listener.BeginAcceptBluetoothClient(AcceptConnection, listener);
        }

        public void BeginConnect(BluetoothDeviceInfo device, string overridePin = "")
        {
            if (!device.Authenticated) 
                throw new Exception("Tried to connect to a device that is not authenticated."); 
            
            var localEndpoint = new BluetoothEndPoint(BluetoothAddress.Parse(GetBTMacAddress().ToString()), BluetoothService.SerialPort);
            var c = new BluetoothClient(localEndpoint) {Authenticate = true};
            //c.SetPin(overridePin == string.Empty ? Device_Pin : overridePin);
            c.BeginConnect(device.DeviceAddress, BluetoothService.SerialPort, Connect,new BluetoothConnectedData() { SocketClient = c, Device = device });
        }

        public BluetoothAddress GetBluetoothAddress()
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
