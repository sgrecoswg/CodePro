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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SensibleProgramming.CodePro.WPF.Usercontrols
{
    /// <summary>
    /// Interaction logic for FileChooser.xaml
    /// </summary>
    public partial class FileChooser : UserControl
    {
        public static readonly RoutedEvent FileNameChangedEvent = EventManager.RegisterRoutedEvent("FileNameChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FileChooser));
        public event RoutedEventHandler FileNameChanged
        {
            add { AddHandler(FileNameChangedEvent, value); }
            remove { RemoveHandler(FileNameChangedEvent, value); }
        }

        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        public static readonly DependencyProperty FileNameProperty =  DependencyProperty.Register("FileName", typeof(string), typeof(FileChooser),
                                                                    new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public FileChooser()
        {
            InitializeComponent();
            btnBrowse.Click += new RoutedEventHandler(btnBrowse_Click);
            txtFileName.TextChanged += new TextChangedEventHandler(txtFileName_TextChanged);
        }

      

        void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDIalog = new OpenFileDialog();
            //fileDIalog.Filter = "Image files (*.bmp, *.jpg)|*.bmp;*.jpg|Doc Files (*.doc;*.docx)|*.doc;*.docx";
            fileDIalog.AddExtension = true;
            if (fileDIalog.ShowDialog() == true)
            {
                FileName = fileDIalog.FileName;
                txtFileName.Text = FileName;                
            }
        }

        void txtFileName_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;

            base.RaiseEvent(new RoutedEventArgs(FileNameChangedEvent));
        }

    }
}
