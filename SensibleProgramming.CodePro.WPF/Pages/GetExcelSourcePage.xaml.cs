using SensibleProgramming.CodePro.Models.Excel;
using SensibleProgramming.Core.Extensions;
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
    /// Interaction logic for GetExcelSourcePage.xaml
    /// </summary>
    public partial class GetExcelSourcePage : Page
    {
        public ExcelInstance ExcelInstance { get; set; }
        public GetExcelSourcePage()
        {
            InitializeComponent();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            App.SelectedDataBaseContainer = ExcelInstance;
            App.Navigate("ChooseOutputOptions");
        }

        private void dgStoredProcedures_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void dgDataTables_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private async void ucFileChooser_FileNameChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                var _dataMngr = new ExcelDataManager(ucFileChooser.FileName);
                _dataMngr.OnError = (exc) =>
                {
                    App.SetStatus(exc.Message);
                };
                _dataMngr.OnNotify = (msg) =>
                {
                    App.SetStatus(msg);
                };

                App.SetStatus("Getting tables");
                ExcelInstance = await _dataMngr.GetInstance();
                
                dgDataTables.ItemsSource = null;
                dgDataTables.ItemsSource = ExcelInstance.Tables;
                App.SetStatus("Ready");
            }
            catch (Exception exc1)
            {

                MessageBox.Show(exc1.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
