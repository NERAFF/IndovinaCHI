using System;
using System.Collections.Generic;
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
    /// Logica di interazione per SchermataGioco.xaml
    /// </summary>
    public partial class SchermataGioco : Window
    {
        List<Button> myButtons = new List<Button>();//opponent
        List<Button> myButtons2 = new List<Button>();//local
        Condivisa cond = new Condivisa();
        Persona incognita = new Persona();

        //cordinate dei bottoni
        const int width = 50;
        const int height = 70;
        const int Step = 2;
        int currentx = 0;
        int currenty = 0;
        int currentx2 = 550;
        int currenty2 = 0;

        //impostazioni
        double Tentativi;
        double TentativiMiei;
        double TentativiAvversario;
        double Tempo;
        string NomeUtente;
        string NomeUtenteAvversario;
        int nIncognite = 24;
        bool turno = false;
        //comunicazioni
        const int port = 2009;
        string mioUsername;
        string altroClient;
        UdpClient receivingClient;
        UdpClient sendingClient;
        Thread receivingThread;
        string ipAvversario;


        public SchermataGioco(string ipAvversario, double Tentativi, double Tempo, string NomeUtente, string NomeUtenteAvversario, bool turno)
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;//full screen
            WindowStyle = WindowStyle.None;//no bordo
            CreaSagome();
            CreaPersonaggi();

            //set attributi
            this.ipAvversario = ipAvversario;
            this.Tentativi = Tentativi;
            this.TentativiMiei = Tentativi;
            this.TentativiAvversario = Tentativi;
            this.Tempo = Tempo;
            this.NomeUtente = NomeUtente;
            this.NomeUtenteAvversario = NomeUtenteAvversario;
            this.turno = turno;
            lblChatt.Content = "Chat con avversario " + NomeUtenteAvversario;
            LblTentativiMiei.Content = TentativiMiei+"/"+Tentativi;
            LblTentativiAvversario.Content = TentativiMiei+"/"+Tentativi;
            lblMatch.Content = NomeUtenteAvversario+"   VS   "+NomeUtente;
            if (turno)//Set stato turno
                lblStatoTurno.Content = "e' il tuo turno";
            else
                lblStatoTurno.Content = "è il turno dell'avversario";
            Random rnd = new Random();//classe per il random del personaggio da indovinare
            int numeroRandom = rnd.Next(0, 25);
            ImageRandom.Source = new BitmapImage(new Uri(cond.p.getPersonaggio(numeroRandom).getPercorso()));
            incognita = cond.p.getPersonaggio(numeroRandom);//persona da indovinare

            //thread per ricevere i dati in background => non bloccante
            ThreadStart start = new ThreadStart(Receiver);
            receivingThread = new Thread(start);
            receivingThread.IsBackground = true;
            receivingThread.Start();

        }
        private void Receiver()//protocollo comunicazione
        {
            receivingClient = new UdpClient(port);

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            while (true)
            {
                byte[] data = receivingClient.Receive(ref endPoint);
                string message = Encoding.ASCII.GetString(data);
                string daRitornare;

                String[] vettElementi = message.Split(';');
                if (vettElementi[0] == "d")//esegue il secondo peer riceve un messaggio
                {
                    bool risposta;
                    //Dispatcher per modificare la grafica
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        txt_all.Text += NomeUtenteAvversario+":"+vettElementi[1]+"\n"; 
                        
                    }));
                    if (MessageBox.Show(vettElementi[1], "Messaggio arrivato", MessageBoxButton.YesNo) == MessageBoxResult.Yes)//invia la risposta
                    {
                        daRitornare = "m;y";
                        risposta =true;
                    }
                    else
                    {
                        daRitornare = "m;n";
                        risposta = false;
                    }
                    sendData(ipAvversario, daRitornare);
                    Dispatcher.BeginInvoke((Action)(() =>//scrivo nella chat la risposta che do
                    {
                        if(risposta)
                        txt_all.Text += NomeUtente+":SI"; 
                        else
                        txt_all.Text += NomeUtente + ":NO"; //vado a visualizzare chi ha mandato il messaggio e il messaggio stesso
                    }));
                }
                else if (vettElementi[0] == "m")//se il peer riceve "m" ha la risposta alla domanda e abbassa le pedine
                {
                    //Dispatcher per modificare il thread grafico
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        txt_all.Text += NomeUtenteAvversario + ":" + vettElementi[1] + "\n";
                    }));
                }
                else if (vettElementi[0] == "f")//aggiorna tabella avversario
                {
                    //Dispatcher per modificare il thread grafico
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        //deve cambiare la griglia avversaria
                    }));
                }
                else if (vettElementi[0] == "t")//tentativo arrivato
                {
                    TentativiAvversario--;
                    string nomeTentativo = vettElementi[1];
                    if(incognita.getNome()==nomeTentativo)//peer 2 ha vinto la partita indovinato la persona
                    {
                        sendData(ipAvversario, "e;w;");
                        MessageBox.Show(NomeUtenteAvversario + " hai perso il game avversario l'avversario ha indovinato");
                        this.Close();//esci dal gioco
                        break;//finita la partita finisce il thread
                    }
                    else//tentativo sbagliato
                    {
                        if(TentativiAvversario==0)
                        {
                            sendData(ipAvversario, "e;t;"); //ho finito i tentativi  mi disconnetto
                        }
                        else
                        {
                            sendData(ipAvversario, "h;");
                        }
                    }
                    //Dispatcher per modificare il thread grafico
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        LblTentativiAvversario.Content = TentativiAvversario + "/" + Tentativi;
                    }));
                }
                else if (vettElementi[0] == "h")//risposta tentativo arrivata
                {
                    TentativiMiei--;
                    
                    //non c'è bisogno il controllo altrimenti mi sarebbe arrivata la lettera e;t
                        Dispatcher.BeginInvoke((Action)(() =>
                        {
                            LblTentativiMiei.Content = TentativiMiei + "/" + Tentativi;
                        }));
                    
                }
               
                else if (vettElementi[0] == "e")//arrendersi
                {
                    string motivazione = vettElementi[1];
                    //Dispatcher per modificare il thread grafico
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        if (vettElementi[1] == "a")
                            MessageBox.Show("avversario si è disconnesso");
                        else if(vettElementi[1]=="t")
                        {
                            MessageBox.Show("avversario ha finito i tentativi");
                        }
                        else if(vettElementi[1]=="w")
                        {

                        }
                    }));
                    break; //break serve per far terminare il Thread
                }
                if(!turno)
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        btnInvia.IsEnabled = false;
                        btnRound.IsEnabled = false;
                        btnTentativo.IsEnabled = false;
                    }));        
                }
                else
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        btnInvia.IsEnabled = true;
                        btnRound.IsEnabled = true;
                        btnTentativo.IsEnabled = true;
                    }));
                }
            }
        }
       
        public void CreaSagome()//sagome opponent
        {

            int c = 0;
            for (int j = 0; j < nIncognite; j++)//creazione sagoma avversario
            {
                c++;
                Button Btn = new Button();//creazione bottone
                                          //Btn.Content = (j+1).ToString();
                Btn.Name = "Button" + (j + 1).ToString();
                Btn.Width = width;
                Btn.Height = height;
                var brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("Immagini\\sagoma.png", UriKind.Relative));
                Btn.Background = brush;
                Btn.Click += BtnClickSagome;
                //Btn.Click += new System.EventHandler();
                myButtons.Add(Btn);//aggiunge all array di bottoni



                if (j % 6 == 0)//ogni 6 bottoni creati va a capo
                {
                    c = 0;
                    currenty += 40;
                    currentx = 50;
                }
                else
                {
                    currentx += 30;
                }

                Canvas.SetTop(Btn, Step * currenty);//aggiunge il bottone al pannello
                Canvas.SetLeft(Btn, Step * currentx);
                MainCanvas.Children.Add(Btn);

            }
        }
        public void CreaPersonaggi()//personaggi local
        {

            int c = 0;
            //creazione sagome personaggi
            for (int j = 0; j < 24; j++)
            {
                c++;
                //creazione bottone
                Button Btn2 = new Button();
                //Btn2.Content = (j + 1).ToString();
                Btn2.Name = cond.p.getPersonaggio(j).nome;//nome bottone=nome del personaggio
                Btn2.Width = width;
                Btn2.Height = height;
                var brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri(cond.p.getPersonaggio(j).getPercorso(), UriKind.Relative));
                Btn2.Background = brush;
                Btn2.Click += BtnClickPersonaggi;
                myButtons2.Add(Btn2);//aggiunge all'array di bottoni


                if (j % 6 == 0)//ogni 6 bottoni creati va a capo
                {
                    c = 0;
                    currenty2 += 40;
                    currentx2 = 550;
                }
                else
                {
                    currentx2 += 30;
                }
                //aggiunge il bottone al pannello
                Canvas.SetTop(Btn2, Step * currenty2);
                Canvas.SetLeft(Btn2, Step * currentx2);
                MainCanvas.Children.Add(Btn2);

            }
        }

        private void BtnClickSagome(object sender, RoutedEventArgs e)//metodo bottone opponent
        {
            //Button btn = (Button)sender;
            //MessageBox.Show(btn.Name);

        }
        private void BtnClickPersonaggi(object sender, RoutedEventArgs e)//metodo bottone local
        {
            Button btn = (Button)sender;
            var brush = new ImageBrush();
            for (int i = 0; i < nIncognite; i++)
            {
                if (cond.p.getPersonaggio(i).nome == btn.Name && cond.p.getPersonaggio(i).isEliminato()==false)//permette di alzare e abbassare la pedina solo se non è eliminato
                {
                    if (cond.p.getPersonaggio(i).isAttivo() == true)
                    {
                        cond.p.getPersonaggio(i).setAttivo(false);
                        brush.ImageSource = new BitmapImage(new Uri("Immagini\\sagomaX.jpg", UriKind.Relative));
                        btn.Background = brush;
                    }
                    else
                    {
                        cond.p.getPersonaggio(i).setAttivo(true);
                        brush.ImageSource = new BitmapImage(new Uri(cond.p.getPersonaggio(i).getPercorso()));
                    }
                    btn.Background = brush;
                    myButtons2.ElementAt(i).Background = brush;
                }
            }
        }
        private void BtnInvia_Click(object sender, RoutedEventArgs e)
        {
            if (turno)
            {
                txt_all.Text += NomeUtente + ": " + txtMessaggio.Text + "\n";//visualizzo il messaggio
                sendData(ipAvversario, "m;" + txtMessaggio.Text);//invio i dati del messaggio all'altro peer
            }
        }

        private void BtnRound_Click(object sender, RoutedEventArgs e)
        {
            turno =! turno;//cambia turno
            string cordinate=getCordinateDaInviare();
            sendData(ipAvversario, "f;" + cordinate);//invio i dati del messaggio all'altro peer

        }
        private string getCordinateDaInviare()//metodo per mettere la sagoma permanente e sapere le cordinate da inviare
        {
            string cordinatedaInviare = "";
            var brush = new ImageBrush();//per mettere la sagoma permanente
            for (int i = 0; i < nIncognite; i++)
            {
                    if (cond.p.getPersonaggio(i).isAttivo() == true && cond.p.getPersonaggio(i).isEliminato()==false)//invia le cordinate di tutte le pedine abbassate ma non eliminate definitivamente
                    {
                        cordinatedaInviare += i + ";";
                        cond.p.getPersonaggio(i).setEliminato(true);
                        brush.ImageSource = new BitmapImage(new Uri("Immagini\\sagomaO.jpg", UriKind.Relative));
                        myButtons2.ElementAt(i).Background = brush;
                    }                  
            }
            return cordinatedaInviare;
        }
        private void BtnTentativo_Click(object sender, RoutedEventArgs e)
        {
            turno = !turno;
            sendData(ipAvversario, "t;"+txtMessaggio.Text);
        }

        private void sendData(string ip, string messaggio)//per inviare pacchetti
        {
            sendingClient = new UdpClient(ip, port);
            string toSend = messaggio;
            byte[] data = Encoding.ASCII.GetBytes(toSend);
            sendingClient.Send(data, data.Length);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Sei sicuro? \n perderai la partita a tavolino", "Esci", MessageBoxButton.YesNo) == MessageBoxResult.Yes)//invia la risposta
            {
                sendData(ipAvversario, "e;a");
                this.Close();
            }
        }
    }
}
