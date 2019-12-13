using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGreenEnvironment
{
    class BluetoothConnectedData
    {
        public BluetoothClient SocketClient { get; set; }
        public BluetoothDeviceInfo Device { get; set; }
    }
}
