using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGreenEnvironment
{
    public class BluetoothDeviceView
    {
        public BluetoothDeviceInfo Device { get; set; }

        public BluetoothDeviceView(BluetoothDeviceInfo Device)
        {
            this.Device = Device;
        }

        public override string ToString()
        {
            var ConnectedStr = Device.Connected ? "Forbundet" : "Ej forbundet";
            var Authenticated = Device.Authenticated ? "Bonded" : "Ikke bonded";
            
            return $"{Device.DeviceName} [{ConnectedStr}] [{Authenticated}]";
        }

    }
}
