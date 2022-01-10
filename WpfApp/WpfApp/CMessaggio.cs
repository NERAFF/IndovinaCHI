using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Properties;

namespace WpfApp
{
    public class CMessaggio
    {
        private UdpClient client;
        public string ipAvversario;
        IPEndPoint iPEndPoint;
        public CMessaggio()
        {
            ipAvversario = "localhost";
            client = new UdpClient(ipAvversario,2009);
            iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
        }
        public void send(string mess)
        {
            Byte[] sendBytes = Encoding.ASCII.GetBytes(mess);
            client.Send(sendBytes, sendBytes.Length);
        }
        public string receive()
        {
           
            byte[] test = client.Receive(ref iPEndPoint);
            string msg = Encoding.Default.GetString(test);
            ipAvversario = iPEndPoint.Address.ToString();
            return msg;
        }
        public void setIpavversario(string ipAvversario)
        {
            this.ipAvversario = ipAvversario;
            client.Connect(ipAvversario, 2009);
        }
        public string getIpavversario()
        {
            return iPEndPoint.Address.ToString();
        }
    }
}
