using Mosaic.CodePro.Models;
using Mosaic.CodePro.WPF.Pages;
using Mosaic.Data.SQL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using _app = Mosaic.CodePro.WPF.ApplicationContext;

namespace Mosaic.CodePro.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ObservableCollection<SQLInstance> SqlInstances { get; internal set; } = new ObservableCollection<SQLInstance>();
        public static SQLInstance SelectedSQLServer { get; internal set; } = new SQLInstance();

        public static BaseDataBaseContainer SelectedDataBaseContainer { get; internal set; }

        public static string StatusMessage { get; set; } = "ready";

        public static CodeGenerationOptions GeneratorOptions { get; set; } = new CodeGenerationOptions();

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            MessageBox.Show("Exit Event Raised", "Exit");
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _app.MainWindow = new MainWindow();
            

            _app.GotoPage("ChooseSourcePage");
            _app.MainWindow.Show();

            Current.MainWindow.Title = "Code Pro";

            Current.MainWindow.Closed += (s, a) =>
            {
                MessageBox.Show("Shutting Down", "Shutdown", MessageBoxButton.OK, MessageBoxImage.Information);
                Shutdown();
            };
        }

        public static void SetStatus(string msg)
        {
            _app.MainWindow.txtstatus.Text = msg;
        }


        public static void Navigate(string pageName)
        {
            _app.GotoPage(pageName);
        }


        /*
          App._mainWindow.CurrentUri = new Uri(new Uri("pack://application:,,,/"), "/Pages/CodeGeneratorOptionMVC.xaml");
            App._mainWindow.fMain.Source = App._mainWindow.CurrentUri;
         */

    }

   
}
