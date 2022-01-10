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
        int porta;
        private UdpClient client;
        //public string ipAvversario;
        public IPAddress ipAvversario;
        IPEndPoint iPEndPoint;
        private string messaggio;
        public CMessaggio()
        {
            porta = 2009;
            client = new UdpClient(porta);
            iPEndPoint = new IPEndPoint(IPAddress.Any, porta);
        }
        public void invia(string mess)
        {
            client = new UdpClient(ipAvversario.ToString(), porta);
            Byte[] sendBytes = Encoding.ASCII.GetBytes(mess);
            client.Send(sendBytes, sendBytes.Length);
            Console.WriteLine("{0} {1}", ipAvversario.ToString(), mess);
        }
        public void ricevi()
        {
            byte[] test = client.Receive(ref iPEndPoint);
            messaggio = Encoding.Default.GetString(test);
        }
        public void setIpavversario(IPAddress ipAvversario)
        {
            this.ipAvversario = ipAvversario;
        }
        public string getIpavversario()
        {
            return iPEndPoint.Address.ToString();
        }
        public string getMessaggio()
        {
            return messaggio;
        }
    }
}
