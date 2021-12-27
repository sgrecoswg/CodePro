using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Mosaic.CodePro.WPF
{
    public static class ApplicationContext
    {
        public static MainWindow MainWindow { get; set; } = new MainWindow();

        public static void GotoPage(string pageName)
        {
            MainWindow.mainWinFrame.NavigateToPage(pageName);
        }

        public static void GotoPage(Page page)
        {
            MainWindow.mainWinFrame.NavigateToPage(page);
        }
    }

    public static class ApplicationExtensions
    {

        public static void NavigateToPage(this Frame f,string pageName)
        {
            f.Navigate(new Uri($"/Pages/{pageName}.xaml", UriKind.RelativeOrAbsolute));
        }

        public static void NavigateToPage(this Frame f, Page page)
        {
            f.NavigateToPage($"{page.GetType().Name}");
        }
    }
}
