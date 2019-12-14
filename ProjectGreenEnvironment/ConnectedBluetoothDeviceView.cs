using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InTheHand.Net.Sockets;

namespace ProjectGreenEnvironment
{
    public class ConnectedBluetoothDeviceView
    {
        public BluetoothDeviceInfo Device { get; set; }
        public BluetoothClient Client { get; set; }

        public ConnectedBluetoothDeviceView(BluetoothDeviceInfo Device)
        {
            this.Device = Device;
        }

        public ConnectedBluetoothDeviceView() { }

        public override string ToString()
        {
            var ConnectedStr = Client.Connected ? "Forbundet" : "Ej forbundet";
            var Auth = Client.Authenticate ? "Auth" : "Ikke Auth";

            if (Client.Connected)
            {
                return $"{Client.RemoteMachineName} - [{ConnectedStr}] [{Auth}]";
            } else
            {
                if (Device == null) return $"Ukendt Person";
                return $"{Device.DeviceName} - [{ConnectedStr}] [{Auth}]"; ;
            }
        }
    }
}
