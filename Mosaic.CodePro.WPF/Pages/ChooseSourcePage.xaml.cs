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

namespace Mosaic.CodePro.WPF.Pages
{
    /// <summary>
    /// Interaction logic for ChooseSourcePage.xaml
    /// </summary>
    public partial class ChooseSourcePage : Page
    {
        public ChooseSourcePage()
        {
            InitializeComponent();
            DataContext = App.GeneratorOptions.CodeSource;
        }
        

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            switch (App.GeneratorOptions.CodeSource)
            {
                case Models.CodeSources.Dll:
                    //ApplicationContext.GotoPage("GenerateCsharp");
                    ApplicationContext.GotoPage("GenerateJSProxy");
                    break;
                case Models.CodeSources.Json:
                    ApplicationContext.GotoPage("GenerateClassFromJson");
                    break;
                case Models.CodeSources.SQL:
                    ApplicationContext.GotoPage("GetSqlDataSourcePage");
                    break;
                case Models.CodeSources.Access:
                    ApplicationContext.GotoPage("GetAccessSourcePage");
                    break;
                case Models.CodeSources.Excel:
                    ApplicationContext.GotoPage("GetExcelSourcePage");
                    break;
                case Models.CodeSources.Oracle:
                    ApplicationContext.GotoPage("GetOracleSourcePage");
                    break;
                default:
                    break;
            }
        }
    }
}
