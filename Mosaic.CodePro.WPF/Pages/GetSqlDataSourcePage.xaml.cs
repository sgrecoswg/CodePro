using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Mosaic.CodePro.Models;
using Mosaic.Data.SQL;
using static Mosaic.Data.SQL.SQLInstance;

namespace Mosaic.CodePro.WPF.Pages
{
    /// <summary>
    /// Interaction logic for GetSqlDataSourcePage.xaml
    /// </summary>
    public partial class GetSqlDataSourcePage : Page
    {
        SQLManager _sqlService = new SQLManager();

        List<SQLInstance> instances = new List<SQLInstance>();    
        public string SQLServer { get { return cbxServers.SelectedItem.ToString(); } }
        public string SQLDataBase { get { return cbxDataBases.SelectedItem.ToString(); } }

        BackgroundWorker bgGetSQLData;

        public GetSqlDataSourcePage()
        {
            InitializeComponent();
            if (App.SqlInstances == null || App.SqlInstances.Count == 0)
            {
                bgGetSQLData = new BackgroundWorker();
                bgGetSQLData.DoWork += bgGetSQLData_DoWork;
                bgGetSQLData.WorkerReportsProgress = true;
                bgGetSQLData.RunWorkerCompleted += bgGetSQLData_RunWorkerCompleted;
                bgGetSQLData.ProgressChanged += bgGetSQLData_ProgressChanged;

                App.SetStatus("Getting sql servers");
                bgGetSQLData.RunWorkerAsync();
            }
            else
            {
                //if (App.SelectedDataBaseContainer != null)
                //{
                //    if (App.SelectedDataBaseContainer.SelectedDataBase != null)
                //    {
                //        cbxDataBases.SelectedItem = App.SelectedDataBaseContainer.SelectedDataBase;

                //        if (App.SelectedDataBaseContainer.SelectedDataBase != null)
                //        {

                //        }
                //    }
                //}

                bgGetSQLData = new BackgroundWorker();
                bgGetSQLData.DoWork += bgGetSQLData_DoWork;
                bgGetSQLData.WorkerReportsProgress = true;
                bgGetSQLData.RunWorkerCompleted += bgGetSQLData_RunWorkerCompleted;
                bgGetSQLData.ProgressChanged += bgGetSQLData_ProgressChanged;

                App.SetStatus("Getting sql servers");
                bgGetSQLData.RunWorkerAsync();
            }
           
        }

        #region bg
        private void bgGetSQLData_DoWork(object sender, DoWorkEventArgs e)
        {
            _sqlService.GetInstancesOfSQL().ForEach(delegate (string server)
            {
                App.SqlInstances.Add(new SQLInstance()
                {
                    ServerName = server,
                    DataBases = new List<SQLServerDataBase>()
                });
            });

        }

        private void bgGetSQLData_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void bgGetSQLData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //spGettingData.Visibility = System.Windows.Visibility.Hidden;
            //spChooseDataSource.Visibility = System.Windows.Visibility.Visible;
            //cbxServers.Items.Clear();
            //App.SqlInstances.ForEach(delegate (SQLInstance instance)
            //{
            //    cbxServers.Items.Add(instance.ServerName);
            //});
            cbxServers.ItemsSource = App.SqlInstances;
            App.SetStatus("ready");


        }
        #endregion

        private void cbxServers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void cbxDataBases_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            App.SelectedDataBaseContainer = App.SelectedSQLServer.DataBases.FirstOrDefault(x => x.Name == ((SQLServerDataBase)cbxDataBases.SelectedItem).Name);
            App.SelectedSQLServer.SelectedDataBase.Name = App.SelectedDataBaseContainer.Name;
            App.SelectedDataBaseContainer.Tables = new List<IDataBaseTable>();

            App.SetStatus("getting tables");
            _sqlService.GetListOfDataTablesFromSQL(App.SelectedSQLServer.ServerName, App.SelectedSQLServer.SelectedDataBase.Name).ForEach(delegate (string dt) {
                App.SelectedDataBaseContainer.Tables.Add(new SQLServerDataBase.SQLDataBaseTable() {
                    ServerName = App.SelectedSQLServer.ServerName,
                    DatabaseName = App.SelectedDataBaseContainer.Name,
                    IsSelected = false,
                    Name = dt });
            });

            App.SetStatus("getting stored procedures");

            //_sqlService.GetListOfStoredProceduresFromSQL(App.SelectedDataBaseContainer.ServerName, App.SelectedDataBaseContainer.SelectedDataBase.Name).ForEach(delegate (string dt) {
            //    App.SelectedDataBaseContainer.StoredProcedures.Add(new SQLServerDataBase.SQLDataBaseStoredProcedure() { IsSelected = false, Name = dt });
            //});

            this.dgDataTables.ItemsSource = null;
            this.dgDataTables.ItemsSource = App.SelectedDataBaseContainer.Tables;

            //this.dgStoredProcedures.ItemsSource = null;
            //this.dgStoredProcedures.ItemsSource = App.SelectedDataBaseContainer.SelectedDataBase.StoredProcedures;
            App.SetStatus("ready");
        }
       


        private void dgDataTables_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void dgStoredProcedures_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }



        private void txtServerName_LostFocus(object sender, RoutedEventArgs e)
        {
            App.SetStatus("getting databases");

            //(SQLInstance)cbxServers.SelectedItem        
            SQLInstance selectedServer = App.SqlInstances.FirstOrDefault(x => x.ServerName == txtServerName.Text);
            if (selectedServer == null)
            {
                var newSQL = new SQLInstance() { ServerName = txtServerName.Text, DataBases = new List<SQLInstance.SQLServerDataBase>() };
                App.SqlInstances.Add(newSQL);
                App.SelectedSQLServer = newSQL;
            }
            else
            {
                App.SelectedSQLServer = selectedServer;
            }

            cbxServers.ItemsSource = null;
            cbxServers.ItemsSource = App.SqlInstances;
            cbxServers.SelectedItem = selectedServer;

            App.SelectedSQLServer.DataBases = new List<SQLServerDataBase>();
            _sqlService.GetListOfDataBasesFromSQL(App.SelectedSQLServer.ServerName).ForEach(delegate (string db) {
                App.SelectedSQLServer.DataBases.Add(new SQLServerDataBase() { Name = db });
            });
            
            this.cbxDataBases.ItemsSource = null;
            this.cbxDataBases.ItemsSource = App.SelectedSQLServer.DataBases;

            App.SetStatus("ready");
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate("ChooseOutputOptions");
        }
    }
}
