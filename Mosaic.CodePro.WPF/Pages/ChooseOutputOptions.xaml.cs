using Mosaic.CodePro.Models.CodeWriters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for ChooseOutputOptions.xaml
    /// </summary>
    public partial class ChooseOutputOptions : Page
    {
        public ChooseOutputOptions()
        {
            InitializeComponent();
            DataContext = App.GeneratorOptions;
            
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
           
            App.Navigate("ChooseSourcePage");

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var server = App.SelectedDataBaseContainer;
            App.GeneratorOptions.OutputFolderPath = ucOutPutfolder.FolderName;
            CSharpClassWriter writer = new CSharpClassWriter();
            writer.OutputFolderPath = App.GeneratorOptions.OutputFolderPath;

            //get the interfaces
            List<string> interfacesText = writer.WriteInterfaces(App.SelectedDataBaseContainer);

            //get the domain
            if (App.GeneratorOptions.OutputDomain)
            {
                List<string> classText = writer.WriteDomainClasses(App.SelectedDataBaseContainer);
            }

            //get the dal
            if (App.GeneratorOptions.OutputDAL)
            {                
                List<string> classText = writer.WriteDALEntities(App.SelectedDataBaseContainer);
                List<string> reposText = writer.WriteDALRepositories(App.SelectedDataBaseContainer,App.GeneratorOptions.DALType);
            }

            if (App.GeneratorOptions.OutputCommandSEvents)
            {
                List<string> classText = writer.WriteCommandsAndEvents(App.SelectedDataBaseContainer);
                //List<string> reposText = writer.WriteDALRepositories(App.SelectedDataBaseContainer, App.GeneratorOptions.DALType);
            }

            //get the webapi
            bool viewModelsWritten = false;
            
            if (App.GeneratorOptions.OutputAsWebAPI)
            {                
                writer.WriteWebAPIControllers(App.SelectedDataBaseContainer);
                writer.WriteWebAPIProxies(App.SelectedDataBaseContainer);
                writer.WriteViewModels(App.SelectedDataBaseContainer);
                viewModelsWritten = true;
                //copy common files
            }

            //get the ui
            if (App.GeneratorOptions.OutputAsMVC)
            {
                
                writer.WriteMVCControllers(App.SelectedDataBaseContainer, App.GeneratorOptions.OutputAsWebAPI);
                if (!viewModelsWritten)
                {
                    writer.WriteViewModels(App.SelectedDataBaseContainer);
                    viewModelsWritten = true;
                }
                //copy common files
            }

            if (App.GeneratorOptions.OutputAsWPF)
            {

            }

            if (App.GeneratorOptions.OutputAsJSLibrary)
            {
                writer.WriteJSONClasses(App.SelectedDataBaseContainer);
            }

            App.SetStatus($"Done. Files Saved To {App.GeneratorOptions.OutputFolderPath}");
            Process p = new Process() { StartInfo = new ProcessStartInfo() { Arguments = App.GeneratorOptions.OutputFolderPath, FileName = "explorer.exe" } };
            p.Start();
        }
    }
}
