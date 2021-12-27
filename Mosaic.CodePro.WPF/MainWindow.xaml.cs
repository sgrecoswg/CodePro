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

namespace Mosaic.CodePro.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void mainWinFrame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (e.OriginalSource is Button)
                //{
                //    Button btn = (Button)e.OriginalSource;

                //    if ((btn.CommandParameter != null) && (btn.CommandParameter.Equals("Order")))
                //    {

                //        mainWinFrame.Navigate(OrderPage);
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
    }
}
