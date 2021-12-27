using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using forms = System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mosaic.CodePro.WPF.Usercontrols
{
    /// <summary>
    /// Interaction logic for FolderChooser.xaml
    /// </summary>
    public partial class FolderChooser : UserControl
    {
        public static readonly RoutedEvent FolderNameChangedEvent = EventManager.RegisterRoutedEvent("FolderNameChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FolderChooser));
        public event RoutedEventHandler FolderNameChanged
        {
            add { AddHandler(FolderNameChangedEvent, value); }
            remove { RemoveHandler(FolderNameChangedEvent, value); }
        }

        public string FolderName
        {
            get { return (string)GetValue(FolderNameProperty); }
            set { SetValue(FolderNameProperty, value); }
        }

        public static readonly DependencyProperty FolderNameProperty =  DependencyProperty.Register("FolderName", typeof(string), typeof(FolderChooser),
                                                                    new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public FolderChooser()
        {
            InitializeComponent();
            btnBrowse.Click += new RoutedEventHandler(btnBrowse_Click);
            txtFolderName.TextChanged += new TextChangedEventHandler(txtFolderName_TextChanged);
        }

      

        void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            forms.FolderBrowserDialog FolderDialog = new forms.FolderBrowserDialog();
            //FolderDIalog.Filter = "Image Folders (*.bmp, *.jpg)|*.bmp;*.jpg|Doc Folders (*.doc;*.docx)|*.doc;*.docx";
            //FolderDialog.AddExtension = true;
            var result = FolderDialog.ShowDialog();
            if (result == forms.DialogResult.OK)
            {
                FolderName = FolderDialog.SelectedPath;
                txtFolderName.Text = FolderName;
            }
        }

        void txtFolderName_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;

            base.RaiseEvent(new RoutedEventArgs(FolderNameChangedEvent));
        }

    }
}
