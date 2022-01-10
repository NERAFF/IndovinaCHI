using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        double Tempo;
        string NomeUtente;
        string NomeUtenteAvversario;
        int nIncognite = 24;

        CMessaggio messaggio;
        public SchermataGioco(string ipAvversario, double Tentativi, double Tempo, string NomeUtente, string NomeUtenteAvversario, CMessaggio messaggio)
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;//full screen
            CreaSagome();
            CreaPersonaggi();

            this.Tentativi = Tentativi;
            this.Tempo = Tempo;
            this.NomeUtente = NomeUtente;
            this.NomeUtenteAvversario = NomeUtenteAvversario;
            LblTentativiMiei.Content = Tentativi;
            LblTentativiAvversario.Content = Tentativi;
            this.messaggio = messaggio;
            LblMio.Content = NomeUtente;
            Random rnd = new Random();//classe per il random del personaggio da indovinare
            int numeroRandom = rnd.Next(0, 25);
            ImageRandom.Source = new BitmapImage(new Uri(cond.p.getPersonaggio(numeroRandom).getPercorso()));
            incognita = cond.p.getPersonaggio(numeroRandom);

        }
        public void CreaSagome()//sagome opponent
        {

            int c = 0 ;
            for (int j = 0; j < nIncognite; j++)//creazione sagoma avversario
            {
                c++;
                    Button Btn = new Button();//creazione bottone
                    //Btn.Content = (j+1).ToString();
                    Btn.Name = "Button" + (j+1).ToString();
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


                if (j % 6 == 0 )//ogni 6 bottoni creati va a capo
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
            for (int i=0;i<nIncognite;i++)
            {
                if(cond.p.getPersonaggio(i).nome==btn.Name)
                {
                    if(cond.p.getPersonaggio(i).isAttivo()==true)
                    { 
                        cond.p.getPersonaggio(i).setAttivo(false);
                        brush.ImageSource = new BitmapImage(new Uri("Immagini\\sagomaO.jpg", UriKind.Relative));
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

        }
    }
}
