using SensibleProgramming.CodePro.Models.Oracle;
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
    /// Interaction logic for GetOracleSourcePage.xaml
    /// </summary>
    public partial class GetOracleSourcePage : Page
    {
        OracleInstance _instance = new OracleInstance();
        OracleDataManager _mgr = new OracleDataManager();
        public GetOracleSourcePage()
        {
            InitializeComponent();
        }
    }
}
