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
        Persone p;
        List<Button> myButtons = new List<Button>();
        bool OnScreen = false;
        const int width = 100;
        const int height = 25;
        const int Step = 2;
        int currentx = 0;

        
        public SchermataGioco()
        {
            InitializeComponent();
            ButtonArray();
        }
        public void ButtonArray()
        {
            for (int i = 0; i < 6; i++)
            {
                Button Btn = new Button();
                Btn.Content = i.ToString();
                Btn.Name = "Button" + i.ToString();
                Btn.Width = width;
                Btn.Height = height;
                myButtons.Add(Btn);

            }

        }
        private void Visualizza_Click(object sender, RoutedEventArgs e)
        {
            if (!OnScreen)
            {
                foreach (var item in myButtons)
                {
                    Canvas.SetLeft(item, Step * currentx);
                    currentx += 50;
                    MainCanvas.Children.Add(item);
                }
                OnScreen = true;
            }
        }

        private void Rimuovi_Click(object sender, RoutedEventArgs e)
        {
            if (OnScreen)
            {
                foreach (var item in myButtons)
                {

                    MainCanvas.Children.Remove(item);
                }
                OnScreen = false;
                currentx = 0;
            }
        }
    }
}
