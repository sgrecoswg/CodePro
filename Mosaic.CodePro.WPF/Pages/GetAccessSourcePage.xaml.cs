using Mosaic.CodePro.Models.Access;
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
    /// Interaction logic for GetAccessSourcePage.xaml
    /// </summary>
    public partial class GetAccessSourcePage : Page
    {
        public AccessInstance AccessInstance { get; set; }
        public GetAccessSourcePage()
        {
            InitializeComponent();
        }

        private void dgDataTables_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void dgStoredProcedures_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void ucFileChooser_FileNameChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                var _dataMngr = new AccessDataManager(ucFileChooser.FileName);
                _dataMngr.OnError = (exc) => {
                    App.SetStatus(exc.Message);
                };
                _dataMngr.OnNotify = (msg) => {
                    App.SetStatus(msg);
                };

                App.SetStatus("Getting tables");
                AccessInstance = _dataMngr.GetInstance();
                dgDataTables.ItemsSource = null;
                dgDataTables.ItemsSource = AccessInstance.Tables;
                App.SetStatus("Ready");
            }
            catch (Exception exc1)
            {

                MessageBox.Show(exc1.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            App.SelectedDataBaseContainer = AccessInstance;
            App.Navigate("ChooseOutputOptions");
        }
    }
}
