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
using SensibleProgramming.CodePro.Models;

namespace SensibleProgramming.CodePro.WPF.Usercontrols
{
    /// <summary>
    /// Interaction logic for UIOptions.xaml
    /// </summary>
    public partial class UIOptions : UserControl
    {

       
        public UIOptions()
        {
            InitializeComponent();
            DataContext = App.GeneratorOptions;
        }
    }
}
