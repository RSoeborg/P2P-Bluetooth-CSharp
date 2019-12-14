using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectGreenEnvironment
{
    public partial class FrmConnect : Form
    {
        private readonly BluetoothHandler bluetooth;

        private string _status = "Idle";
        private string Status {
            get { return _status; }
            set
            {
                Invoke((MethodInvoker)(() => {
                    lblStatus.Text = $"Status: {value}";
                }));
                    
                _status = value;
            }
        }
        private bool IsIdle() => Status == "Idle";


        public FrmConnect(BluetoothHandler bluetooth)
        {
            this.bluetooth = bluetooth;
            InitializeComponent();

            bluetooth.DiscoverProgress += (s, e) => {
                BluetoothDeviceInfo[] devices = (BluetoothDeviceInfo[])s;
                foreach (var device in devices)
                {
                    lblScanResults.Items.Add(new BluetoothDeviceView(device));
                }
            };
            bluetooth.DiscoverComplete += (s, e) => {
                SetIdle();
            };
            bluetooth.AcceptedConnection += (s, e) => {
                var device = (BluetoothClient)s;
                new Thread(() =>
                {
                    bluetooth.StartListeningTo(device);
                }).Start();

                Invoke((MethodInvoker)(() => {
                    lblListening.Items.Add(new ConnectedBluetoothDeviceView() { Client = device });
                }));
            }; 
            bluetooth.StartListener();
            bluetooth.ConnectedTo += (s, e) => {
                SetIdle();
                var Response = (BluetoothConnectedData)((IAsyncResult)s).AsyncState;

                Invoke((MethodInvoker)(() => {
                    lblConnected.Items.Add(new ConnectedBluetoothDeviceView(Response.Device) { Client = Response.SocketClient });
                }));  
            };

            btnConnect.Click += (s, e) => {
                if (IsIdle())
                {
                    var connectToDevice = ((BluetoothDeviceView)lblScanResults.SelectedItem).Device;
                    Status = $"Forbinder til {connectToDevice.DeviceName} ...";
                    bluetooth.BeginConnect(connectToDevice);
                }
            };
            btnScan.Click += (s, e) => {
                if (IsIdle())
                {
                    lblScanResults.Items.Clear();
                    bluetooth.BeginDiscoveringDevices();
                    Status = "Skanner..";
                }
            };
        }

        private void SetIdle()
        {
            Status = "Idle";
        }
    }
}
