using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using InTheHand.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using InTheHand.Net.Bluetooth;

namespace ProjectGreenEnvironment
{
    class GreenEnvironment
    {
        public string PeerName { get; private set; }

        private readonly Guid OurServiceClassId = new Guid("{E075D486-E23D-4887-8AF5-DAA1F6A5B172}");
        private readonly string OurServiceName = "GreenEnv";
        
        public string GetFolderPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "GreenEnvironment");
        }

        public void Setup(string name)
        {
            var path = GetFolderPath();
            Directory.CreateDirectory(path);

            using (var sw = new StreamWriter(TranslateFile("env.txt")))
            {
                sw.Write(name);
            }

            File.Create(TranslateFile($"{name}.txt"));
        }
        public void Load()
        {
            using (var sr = new StreamReader(TranslateFile("env.txt")))
            {
                PeerName = sr.ReadToEnd();
            }
        }

        public string TranslateFile(string file)
        {
            return Path.Combine(GetFolderPath(), file);
        }


        BluetoothClient client;
        public void CreateClient()
        {
            client = new BluetoothClient();
        }
        
        public BluetoothDeviceInfo[] FindPairedDevices()
        { 
            return client.DiscoverDevices();
        }

        private List<BluetoothClient> connected = new List<BluetoothClient>();
        private List<BluetoothClient> listening = new List<BluetoothClient>();

        public void ConnectTo(BluetoothAddress address) {
            client.Connect(address, OurServiceClassId);
            connected.Add(client);
        }
        
 
        public void StartListening()
        {
            var listener = new BluetoothListener(OurServiceClassId);
            listener.Start();

            var listenThread = new Thread(() => {
                while (true)
                {
                    if (listener.Pending())
                    {
                        MessageBox.Show("Rasmus har forbundet til dig");
                        listening.Add(listener.AcceptBluetoothClient());
                    }
                    Thread.Sleep(50);
                }
            });
            listenThread.Start();


            var getMessages = new Thread(() => {
                while (true)
                {
                    foreach (var device in listening)
                    {
                        if (device.Available > 0)
                        {
                            using (var sr = new StreamReader(device.GetStream()))
                            {
                                MessageBox.Show(sr.ReadToEnd());
                            }


                        }
                    }
                    Thread.Sleep(50);
                }
            });
        }

        public void Broadcast(string message)
        {
            foreach (var device in connected)
            {
                using (var sw = new StreamWriter(device.GetStream()))
                {
                    sw.Write(message);
                }
                
            }
        }
        

        
    }
}
