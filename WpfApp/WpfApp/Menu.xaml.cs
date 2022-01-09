using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// Logica di interazione per Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        //attributi che servono per giocare
        double Tentativi;
        double Tempo;
        string ipAvversario;
        string nomeUtente;

        string nomeUtenteAVV = "";
        const int port = 2009;
        UdpClient receivingClient;
        UdpClient sendingClient;
        Thread receivingThread;

        bool impostazioni = false;
        public Menu(string nomeUtente)
        {
            InitializeComponent();
            setMenu();
            this.nomeUtente = nomeUtente;
            lblBentornato.Content = "Bentornato " + nomeUtente;

        }
        public Menu(string nomeUtente,double Tentativi, double Tempo, string ipAvversario) {
            InitializeComponent();
            setMenu();
            receivingClient = new UdpClient(port);
            //thread per ricevere i dati in background => non bloccante
            ThreadStart start = new ThreadStart(Receiver);
            receivingThread = new Thread(start);
            receivingThread.IsBackground = true;
            receivingThread.SetApartmentState(ApartmentState.STA);
            receivingThread.Start();
            this.nomeUtente = nomeUtente;
            this.Tentativi = Tentativi;
            this.Tempo = Tempo;
            this.ipAvversario = ipAvversario;
            lblBentornato.Content = "Bentornato " + nomeUtente;
            impostazioni = true;
        }
        public void setMenu()
        {
            WindowState = WindowState.Maximized;//full screen
            var brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\Immagini\\play.png"));
            btnInizio.Background = brush;

            var brush2 = new ImageBrush();
            brush2.ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\Immagini\\settings.png"));
            btnSetting.Background = brush2;

            var brush3 = new ImageBrush();
            brush3.ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\Immagini\\exit.png"));
            btnEsci.Background = brush3;
        }

        private void Receiver()//thread ricezione 
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            while (true)
            {
                byte[] data = receivingClient.Receive(ref endPoint);
                string message = Encoding.ASCII.GetString(data);
                ipAvversario = endPoint.Address.ToString();
                if (message[0] == 'c')//richiesta connesione 
                {
                    //il secondo peer riceve C e il nome utente di chi vuole connettersi
                    if (MessageBox.Show("Vuoi stabilire la connessione?", "Richiesta di connessione", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        nomeUtente = message.Substring(2);
                        string daRitornare = "y;" + nomeUtente; //Il secondo peer invia y = yes e il suo Username
                        sendData(ipAvversario, daRitornare); //invia il y;mioUsername al primo peer

                        Dispatcher.BeginInvoke((Action)(() =>
                        {
                            sendingClient.Close();
                            receivingClient.Close();
                            SchermataGioco m = new SchermataGioco(ipAvversario, Tentativi, Tempo, nomeUtente, nomeUtenteAVV);
                            m.Show();
                            this.Hide();
                        }));
                        break; //il thread smette di creare mess
                    }
                    else
                    {
                        //se il secondo peer rifiuta la connessione manda una n = no
                        sendData(ipAvversario, "n");
                    }
                }
                else if (message[0] == 'y')//se arriva una y il peer2 ha accettato la richiesta e quindi si puo giocare
                {
                    
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        sendingClient.Close();
                        receivingClient.Close();
                        SchermataGioco m = new SchermataGioco(ipAvversario, Tentativi, Tempo, nomeUtente, nomeUtenteAVV);
                        m.Show();
                        this.Hide();
                    }));
                    break; //il thread smette di creare mess
                }
                else//altri messaggi
                {

                }
            }
        }
        private void sendData(string ip, string messaggio) //funzione per inviare le risposte
        {
            sendingClient = new UdpClient(ip, port);
            string toSend = messaggio;
            byte[] data = Encoding.ASCII.GetBytes(toSend);
            sendingClient.Send(data, data.Length);
        }

        private void BtnInizio_Click(object sender, RoutedEventArgs e)
        {
            if (impostazioni)
            {
                //qui invia richiesta connessone 
                sendingClient = new UdpClient(ipAvversario, port);
                string toSend = "c;" +nomeUtente+";"+Tempo+";"+Tentativi; //Primo peer vuole instaurare la connessione
                byte[] data = Encoding.ASCII.GetBytes(toSend);
                sendingClient.Send(data, data.Length);
                MessageBox.Show("Connessione...");

            }
            else
            {
                MessageBox.Show("setta le regole!");
            }
        }

        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            Impostazioni finestra = new Impostazioni(nomeUtente);
            this.Hide();
            finestra.Show();
        }

        private void BtnEsci_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("CHIUDI", "Sei sicuro di voler chiudere il gioco?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.Close();
                //MessageBox.Show("CONNESSIONE","inserisci indirizzo ip dell'avversario",MessageBoxOptions.)
            }
        }
    }
}
