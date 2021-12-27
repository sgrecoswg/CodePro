using SensibleProgramming.CodePro.Models;
using SensibleProgramming.Core.Extensions;
using Newtonsoft.Json.Linq;
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

namespace SensibleProgramming.CodePro.WPF.Pages
{
    /// <summary>
    /// Interaction logic for GenerateClassFromJson.xaml
    /// </summary>
    public partial class GenerateClassFromJson : Page
    {
        public GenerateClassFromJson()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = string.Empty;
            App.SetStatus("Converting json to class");
            var sb = new JsonClassWriter();
            sb.OnError = (exc) => {
                MessageBox.Show(exc.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            sb.OnNotify = (m) => {
                App.SetStatus(m);
            };

            foreach (var s in sb.WriteClass(txtJson.Text))
            {
                txtResult.Text += s;
            }
            
            App.SetStatus("ready");
        }

       
    }
}
