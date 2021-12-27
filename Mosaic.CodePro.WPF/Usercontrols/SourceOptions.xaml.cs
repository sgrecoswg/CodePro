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

namespace Mosaic.CodePro.WPF.Usercontrols
{
    /// <summary>
    /// Interaction logic for SourceOptions.xaml
    /// </summary>
    public partial class SourceOptions : UserControl
    {
        public static readonly RoutedEvent CodeSourceChangedEvent = EventManager.RegisterRoutedEvent("CodeSourceChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FileChooser));
        public event RoutedEventHandler CodeSourceChanged
        {
            add { AddHandler(CodeSourceChangedEvent, value); }
            remove { RemoveHandler(CodeSourceChangedEvent, value); }
        }

        //public CodeSources CodeSource
        //{
        //    get { return (CodeSources)GetValue(CodeSourceProperty); }
        //    set { SetValue(CodeSourceProperty, value); }
        //}

        //public static readonly DependencyProperty CodeSourceProperty = DependencyProperty.Register("CodeSource", typeof(CodeSources), typeof(FileChooser),
        //                                                            new FrameworkPropertyMetadata(CodeSources.Dll, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public SourceOptions()
        {
            InitializeComponent();
            DataContext = App.GeneratorOptions;
            //rbtnDll.Checked += Rbtn_Checked;
            //rbtnAccess.Checked += Rbtn_Checked;
            //rbtnExcel.Checked += Rbtn_Checked;
            //rbtnJson.Checked += Rbtn_Checked;
            //rbtnSQL.Checked += Rbtn_Checked;

        }

        private void Rbtn_Checked(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            base.RaiseEvent(new RoutedEventArgs(CodeSourceChangedEvent));
        }
    }

    public enum CodeSources
    {
        Dll, Json, SQL, Access, Excel
    }
}
