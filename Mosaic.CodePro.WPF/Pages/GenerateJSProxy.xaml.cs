using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
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
    /// Interaction logic for GenerateJSProxy.xaml
    /// </summary>
    public partial class GenerateJSProxy : Page
    {
        public string _dllfilePath { get; set; }
        public GenerateJSProxy()
        {
            InitializeComponent();
        }

        private void fcSelectedFile_FileNameChanged(object sender, RoutedEventArgs e)
        {

            _dllfilePath = fcSelectedFile.FileName;

        }

        private void btnCreateProxy_Click(object sender, RoutedEventArgs e)
        {
            var _assembly = Assembly.LoadFrom(@"C:\Dev\git\EForms\Mosaic.EForms.WebAPI\bin\Mosaic.EForms.WebAPI.dll");
            var _controllerGrouping = _assembly.GetExportedTypes()
                                    .Where(t=>t.BaseType.Name == "ApiController" && !t.IsInterface && !t.IsAbstract)
                                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                                    .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)))
                                    .GroupBy(x => x.DeclaringType)
                                    .Select(x => new {
                                        Controller = x.Key.Name, 
                                        ControllerRoutePrefix = x.Key.GetCustomAttributes().FirstOrDefault(),
                                        Actions = x.Select(s => 
                                        new
                                        {
                                           s.Name,
                                           Route = s.GetCustomAttributes<RouteAttribute>().FirstOrDefault()
                                        }).ToList(),
                                    });

            /*           
                  && !method.IsDefined(typeof(NonActionAttribute))
                  && (
                      method.ReturnType==typeof(ActionResult) ||
                      method.ReturnType == typeof(Task<ActionResult>) ||
                      method.ReturnType == typeof(String) ||
                      )
                  )
               */


            StringBuilder sbProxy = new StringBuilder();
            sbProxy.AppendLine($"import {{ Get,Put, Post ,Delete }} from '@mosaic/request';");
            sbProxy.AppendLine($"import GetSetting from '@mosaic/app-configuration';");
            sbProxy.AppendLine($"let baseUrl = null;");

            sbProxy.AppendLine(@"async function GetBaseUrl() {
                    if (baseUrl)
                        return baseUrl;

                    baseUrl = await GetSetting('<Applications>_API_URL');
                    return baseUrl;
                }");
            sbProxy.AppendLine($"");

            foreach (var c in _controllerGrouping)
            {
                sbProxy.AppendLine($"//{c.Controller}");
                sbProxy.AppendLine($"");
                foreach (var action in c.Actions)
                {
                    sbProxy.AppendLine($"export async function {action}(obj, id) {{");
                    sbProxy.AppendLine("     const baseUrl = await GetBaseUrl();");
                    sbProxy.AppendLine("");

                    var _controllerPrefix = ((RoutePrefixAttribute)c.ControllerRoutePrefix)?.Prefix ?? "";
                    var _actionroute = action;//for now need to get the Route attribute to see what url we are creating
                    var _actionMethod = "Get";//for now need to get the HttpGet,HttpPost etc attributes so we can see what we are going to call

                    sbProxy.AppendLine($"    return await {_actionMethod}(baseUrl + '/{action}';");
                    sbProxy.AppendLine("}");
                    sbProxy.AppendLine($"");
                }
            }
            //string folder = $"{OutputFolderPath}/UI/WebProxies";
            //if (!Directory.Exists(folder))
            //{
            //    Directory.CreateDirectory(folder);
            //}
            //if (saveToDisk) File.WriteAllText($"{folder}/proxy.js", sb.ToString());
        }

        
    }
}
