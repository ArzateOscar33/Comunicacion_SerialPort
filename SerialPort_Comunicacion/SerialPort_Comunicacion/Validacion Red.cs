using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace SerialPort_Comunicacion
{
    internal class Validacion_Red
    {
        private TcpListener _tcpListener;
        private Thread _acceptThread;
        public List<string> Red = new List<string>();
       public void ValidacionRed()
       {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            
            foreach (NetworkInterface interfaz in interfaces)
            {
                if(interfaz.OperationalStatus == OperationalStatus.Up)
                {
                    if(interfaz.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        IPInterfaceProperties propiedades = interfaz.GetIPProperties();

                        IPAddress IpAddress = propiedades.UnicastAddresses.FirstOrDefault(add => 
                        add.Address.AddressFamily == AddressFamily.InterNetwork)?.Address;

                        Red.Add(IpAddress.ToString());
                      
                        
                    }
                }
            }
       }


    }
}
