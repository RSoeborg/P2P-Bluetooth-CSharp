using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGreenEnvironment
{
    class BluetoothHandler
    {








        private void StartListener()
        {
            // Create listener
            var localBtAddr = BluetoothAddress.Parse(GetBTMacAddress().ToString());
            BluetoothListener listener = new BluetoothListener(localBtAddr, BluetoothService.SerialPort);
            listener.SetPin(localBtAddr, "1234");
            listener.Start(10);
            listener.BeginAcceptBluetoothClient(new AsyncCallback(AcceptConnection), listener);
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
