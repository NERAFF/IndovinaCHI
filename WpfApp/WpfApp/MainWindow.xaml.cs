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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
          
        }
        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)//doppio click per andare nella finestra menu
        {
            //Menu finestra = new Menu();
            SchermataGioco finestra = new SchermataGioco();
            finestra.Show();
            this.Hide();
        }
    }
}
