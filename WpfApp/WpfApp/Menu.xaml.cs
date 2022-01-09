using System;
using System.Collections.Generic;
using System.IO;
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
    /// Logica di interazione per Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        double Tentativi;
        double Tempo;
        string NomeUtente;
        public Menu()
        {
            InitializeComponent();
            
            //WindowState = WindowState.Maximized;//full screen
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
        public Menu(double Tentativi, double Tempo, string NomeUtente) {
            InitializeComponent();

            //WindowState = WindowState.Maximized;//full screen
            var brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\Immagini\\play.png"));
            btnInizio.Background = brush;

            var brush2 = new ImageBrush();
            brush2.ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\Immagini\\settings.png"));
            btnSetting.Background = brush2;

            var brush3 = new ImageBrush();
            brush3.ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\Immagini\\exit.png"));
            btnEsci.Background = brush3;

            this.Tentativi = Tentativi;
            this.Tempo = Tempo;
            this.NomeUtente = NomeUtente;
            
        }

        private void BtnInizio_Click(object sender, RoutedEventArgs e)
        {
            SchermataGioco finestra = new SchermataGioco(Tentativi,Tempo,NomeUtente);
            finestra.Show();
            this.Hide();
           
        }

        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            Impostazioni finestra = new Impostazioni();
            finestra.Show();
            this.Hide();
        }

        private void BtnEsci_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
