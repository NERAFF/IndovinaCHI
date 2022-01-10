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
    /// Logica di interazione per Impostazioni.xaml
    /// </summary>
    public partial class Impostazioni : Window
    {
        CMessaggio mess;
        string nomeUtente;
        public Impostazioni(string nomeUtente, CMessaggio mess)
        {
            InitializeComponent();
            this.nomeUtente = nomeUtente;
            this.mess = mess;
        }

        private void Slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LblSlider1.Content = SliderTentativi.Value;//tentativi
        }

        private void SliderTempo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LblSliderTempo.Content = SliderTempo.Value;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Menu Finestra = new Menu(nomeUtente, SliderTentativi.Value, SliderTempo.Value, TxtIpavv.Text, mess);
            this.Close();
            Finestra.Show();
            
        }
    }
}
