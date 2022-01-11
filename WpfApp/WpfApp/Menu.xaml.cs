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

/*il peer1 riesce a mandare al peer2 la richiesta
il peer2 la riceve ma il peer1 non riceve nulla

 */
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

        //CMessaggio mess;//classe per invio e ricezione dei pacchetti
        UdpClient recevingClient;
        UdpClient sendingClient;
        Thread receivingThread;//thread di ricezione
        ThreadStart start;


        bool connetendosi = false;//mi serve per capire se inviare "y;" oppure "y;nickname"
        bool impostazioni = false;

        bool Mioturno = false;
        public Menu(string nomeUtente)
        {
            InitializeComponent();
            setMenu();
            //thread per ricevere i dati in background => non bloccante
            start = new ThreadStart(Receiver);
            receivingThread = new Thread(start);
            receivingThread.IsBackground = true;
            receivingThread.SetApartmentState(ApartmentState.STA);
            receivingThread.Start();
            this.nomeUtente = nomeUtente;
            lblBentornato.Content = "Bentornato " + nomeUtente;

        }
        public Menu(string nomeUtente, double Tentativi, double Tempo, string ipAvversario) {
            InitializeComponent();
            setMenu();
            //receivingClient = new UdpClient(port);

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
            impostazioni = true;
            lblBentornato.Content = "Bentornato " + nomeUtente;
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
            recevingClient = new UdpClient(2009);//porta dove
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 2009);
            while (true)
            {
                //classe ricevi non va bene è bloccante
                //mess.ricevi();
                byte[] test = recevingClient.Receive(ref endPoint);
                string message = Encoding.ASCII.GetString(test);
                String[] vettElementi = message.Split(';');

                if (vettElementi[0] == "c")//richiesta connesione 
                {
                    //il secondo peer riceve C e il nome utente di chi vuole connettersi
                    if (MessageBox.Show("Vuoi stabilire la connessione?", "Richiesta di connessione", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        nomeUtenteAVV = vettElementi[1];
                        Tempo = Double.Parse(vettElementi[2]);
                        Tentativi = Double.Parse(vettElementi[3]);


                        string daRitornare = "y;" + nomeUtente+";"; //Il secondo peer invia y = yes e il suo Username

                        sendData(ipAvversario, daRitornare);

                    }
                    else
                    {
                        //se il secondo peer rifiuta la connessione manda una n = no
                        sendData(ipAvversario,"n;");
                    }
                }
                else if (vettElementi[0] == "y")//sia la seconda che la terza fase
                {

                    if (connetendosi)//riceve Y;nickname
                    {
                        MessageBox.Show(vettElementi[1]);
                        nomeUtenteAVV = vettElementi[1];
                        sendData(ipAvversario, "y;");
                        Dispatcher.BeginInvoke((Action)(() =>//connessione stabilita-->si va alla schermata di gioco
                        {
                            sendingClient.Close();
                            recevingClient.Close();
                            SchermataGioco m = new SchermataGioco(ipAvversario, Tentativi, Tempo, nomeUtente, nomeUtenteAVV, true);//inizia il turno per prima
                            m.Show();
                            this.Close();
                        }));
                        break;
                    }
                    else
                    {
                        Dispatcher.BeginInvoke((Action)(() =>//connessione stabilita-->si va alla schermata di gioco
                        {
                            sendingClient.Close();
                            recevingClient.Close();
                            SchermataGioco m = new SchermataGioco(ipAvversario, Tentativi, Tempo, nomeUtente, nomeUtenteAVV,false);
                            m.Show();
                            this.Close();
                        }));
                        break; //il thread smette di creare mess
                    }
                }
                else//altri messaggi
                {

                }
            }
        }
        private void BtnInizio_Click(object sender, RoutedEventArgs e)
        {
            if (impostazioni)
            {
                //qui invia richiesta connessone 
                string toSend = "c;" + nomeUtente + ";" + Tempo + ";" + Tentativi + ";";//Primo peer vuole instaurare la connessione
                sendData(ipAvversario, toSend);
                connetendosi = true;//si sta cercando di connettersi
                MessageBox.Show("Connessione...");
            }
            else
            {
                MessageBox.Show("Ciao se vuoi giocare imposta le regole e clicca play \n Oppure aspetta che ti arrivi un invito", "Start game");
            }
        }

        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            receivingThread.Abort();//termina il thread
            recevingClient.Close();
            Impostazioni finestra = new Impostazioni(nomeUtente);
            finestra.Show();
            this.Hide();
            

        }

        private void BtnEsci_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("CHIUDI", "Sei sicuro di voler chiudere il gioco?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.Close();
                //MessageBox.Show("CONNESSIONE","inserisci indirizzo ip dell'avversario",MessageBoxOptions.)
            }
        }
        public void sendData(string ip, string messaggio)
        {
            sendingClient = new UdpClient(ip, 2009);
            string toSend = messaggio;
            byte[] data = Encoding.ASCII.GetBytes(toSend);
            sendingClient.Send(data, data.Length);
        }
    }
}
