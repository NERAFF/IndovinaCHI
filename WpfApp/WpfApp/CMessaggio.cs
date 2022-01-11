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
        public UdpClient client;
        //public string ipAvversario;
        public IPAddress ipAvversario;
        private string messaggio;
        public CMessaggio(int porta)
        {
            client = new UdpClient(porta);
        }
        public void setConnessione()
        {
            client = new UdpClient(ipAvversario.ToString(), porta);
        }
        public void invia(string messaggio)
        {
            client = new UdpClient(ipAvversario.ToString(), 2009);
            this.messaggio = messaggio;
            Byte[] sendBytes = Encoding.ASCII.GetBytes(messaggio);
            client.Send(sendBytes, sendBytes.Length);
        }
        public void ricevi(byte[] test)
        {
            messaggio = Encoding.Default.GetString(test);
        }
        public void setIpavversario(IPAddress ipAvversario)
        {
            this.ipAvversario = ipAvversario;
        }
        public IPAddress getIpavversario()
        {
            return ipAvversario;
        }
        public string getMessaggio()
        {
            return messaggio;
        }
    }
}
