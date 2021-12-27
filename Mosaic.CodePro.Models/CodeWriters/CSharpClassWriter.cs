using Mosaic.CodePro.Models.CodeWriters.Dals;
using Mosaic.Core.Models;
using Mosaic.Data.SQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.CodePro.Models.CodeWriters
{

    public class CSharpClassWriter : ClassWriter
    {
        
        #region Interfaces

        public List<string> WriteInterfaces(BaseDataBaseContainer instance,bool saveToDisk = true)
        {
            StringBuilder sb = new StringBuilder();
            List<string> results = new List<string>();            

            foreach (var tbl in instance.Tables.Where(x => x.IsSelected))
            {
                sb.AppendLine(WriteInterface(tbl, saveToDisk));
            }
            results.Add(sb.ToString());
            return results;
        }

        public string WriteInterface(IDataBaseTable tbl, bool saveToDisk = true)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using System;");
            sb.AppendLine($"namespace Mosaic.{tbl.DatabaseName}.Core.Interfaces.Models");
            sb.AppendLine("{");
            sb.AppendLine($"\tpublic interface I{tbl.Name}");
            sb.AppendLine("\t{");

            if (tbl is Access.AccessInstance.AccessTable ||
                tbl is Excel.ExcelInstance.ExcelTable)
            {
                foreach (var p in tbl.Columns)
                {
                    sb.AppendLine($"\t\t{p.DataType} {p.Name} {{get;set;}}");
                }

            }
            else if (tbl is SQLInstance.SQLServerDataBase.SQLDataBaseTable)
            {
                var props = new SQLManager().GetTablePropertiesFromSQL(tbl.ServerName, tbl.DatabaseName, tbl.Name);
                foreach (var p in props)
                {
                    sb.AppendLine($"\t\t{p} {{get;set;}}");
                }
            }
           
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            string result =  sb.ToString();
            if (!Directory.Exists($"{OutputFolderPath}/Core/Interfaces/Models"))
            {
                Directory.CreateDirectory($"{OutputFolderPath}/Core/Interfaces/Models");
            }
            if (saveToDisk) File.WriteAllText($"{OutputFolderPath}/Core/Interfaces/Models/I{tbl.Name}.cs", result);
            return result;
        }

        public List<string> WriteCommandsAndEvents(BaseDataBaseContainer instance, bool saveToDisk = true)
        {
            StringBuilder sb = new StringBuilder();
            List<string> results = new List<string>();

            foreach (var tbl in instance.Tables.Where(x => x.IsSelected))
            {
                sb.AppendLine(WriteCommandAndEvent(tbl, saveToDisk));
            }
            results.Add(sb.ToString());
            return results;
        }

        public string WriteCommandAndEvent(IDataBaseTable tbl, bool saveToDisk = true)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using System;");
            sb.AppendLine($"using Mosaic.Core.Bus;");            
            sb.AppendLine($"namespace Mosaic.{tbl.DatabaseName}.Commands");
            sb.AppendLine("{");

            sb.AppendLine($"\tpublic class Create{tbl.Name}CommandHandler : CommandHandler");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tICanHandle<Create{tbl.Name}Command>");
            sb.AppendLine($"\t\t.ICanHandle<{tbl.Name}CreatedEvent>");
            sb.AppendLine($"\t\t,ICanHandle<Update{tbl.Name}Command>");
            sb.AppendLine($"\t\t,ICanHandle<{tbl.Name}UpdatedEvent>");
            sb.AppendLine("\t}");
            sb.AppendLine(Environment.NewLine);


            sb.AppendLine($"\tpublic class Create{tbl.Name}Command : Command");
            sb.AppendLine("\t{");

            if (tbl is Access.AccessInstance.AccessTable ||
                tbl is Excel.ExcelInstance.ExcelTable)
            {
                foreach (var p in tbl.Columns)
                {
                    sb.AppendLine($"\t\t{p.DataType} {p.Name} {{get;set;}}");
                }

            }
            else if (tbl is SQLInstance.SQLServerDataBase.SQLDataBaseTable)
            {
                var props = new SQLManager().GetTablePropertiesFromSQL(tbl.ServerName, tbl.DatabaseName, tbl.Name);
                foreach (var p in props)
                {
                    sb.AppendLine($"\t\t{p} {{get;set;}}");
                }
                sb.AppendLine(Environment.NewLine);
                sb.AppendLine($"\t\tpublic override void Validate()");
                sb.AppendLine($"\t\t{{");
                sb.AppendLine($"\t\t\tthrow new NotImplementedException();");
                sb.AppendLine($"\t\t}}");
            }

            sb.AppendLine("\t}");

            sb.AppendLine($"\tpublic class {tbl.Name}CreatedEvent : Event");
            sb.AppendLine("\t{");
            if (tbl is Access.AccessInstance.AccessTable ||
                tbl is Excel.ExcelInstance.ExcelTable)
            {
                foreach (var p in tbl.Columns)
                {
                    sb.AppendLine($"\t\t{p.DataType} {p.Name} {{get;set;}}");
                }
                sb.AppendLine(Environment.NewLine);
                sb.AppendLine($"\t\tpublic override void Validate()");
                sb.AppendLine($"\t\t{{");
                sb.AppendLine($"\t\t\tthrow new NotImplementedException();");
                sb.AppendLine($"\t\t}}");

            }
            else if (tbl is SQLInstance.SQLServerDataBase.SQLDataBaseTable)
            {
                var props = new SQLManager().GetTablePropertiesFromSQL(tbl.ServerName, tbl.DatabaseName, tbl.Name);
                foreach (var p in props)
                {
                    sb.AppendLine($"\t\t{p} {{get;set;}}");
                }
                sb.AppendLine(Environment.NewLine);
                sb.AppendLine($"\t\tpublic override void Validate()");
                sb.AppendLine($"\t\t{{");
                sb.AppendLine($"\t\t\tthrow new NotImplementedException();");
                sb.AppendLine($"\t\t}}");
            }

            sb.AppendLine("\t}");

            sb.AppendLine($"\tpublic class Update{tbl.Name}Command : Command");
            sb.AppendLine("\t{");
            if (tbl is Access.AccessInstance.AccessTable ||
                tbl is Excel.ExcelInstance.ExcelTable)
            {
                foreach (var p in tbl.Columns)
                {
                    sb.AppendLine($"\t\t{p.DataType} {p.Name} {{get;set;}}");
                }
                sb.AppendLine(Environment.NewLine);
                sb.AppendLine($"\t\tpublic override void Validate()");
                sb.AppendLine($"\t\t{{");
                sb.AppendLine($"\t\t\tthrow new NotImplementedException();");
                sb.AppendLine($"\t\t}}");

            }
            else if (tbl is SQLInstance.SQLServerDataBase.SQLDataBaseTable)
            {
                var props = new SQLManager().GetTablePropertiesFromSQL(tbl.ServerName, tbl.DatabaseName, tbl.Name);
                foreach (var p in props)
                {
                    sb.AppendLine($"\t\t{p} {{get;set;}}");
                }
                sb.AppendLine(Environment.NewLine);
                sb.AppendLine($"\t\tpublic override void Validate()");
                sb.AppendLine($"\t\t{{");
                sb.AppendLine($"\t\t\tthrow new NotImplementedException();");
                sb.AppendLine($"\t\t}}");
            }

            sb.AppendLine("\t}");

            sb.AppendLine($"\tpublic class {tbl.Name}UpdatedEvent : Event");
            sb.AppendLine("\t{");
            if (tbl is Access.AccessInstance.AccessTable ||
                tbl is Excel.ExcelInstance.ExcelTable)
            {
                foreach (var p in tbl.Columns)
                {
                    sb.AppendLine($"\t\t{p.DataType} {p.Name} {{get;set;}}");
                }
                sb.AppendLine(Environment.NewLine);
                sb.AppendLine($"\t\tpublic override void Validate()");
                sb.AppendLine($"\t\t{{");
                sb.AppendLine($"\t\t\tthrow new NotImplementedException();");
                sb.AppendLine($"\t\t}}");

            }
            else if (tbl is SQLInstance.SQLServerDataBase.SQLDataBaseTable)
            {
                var props = new SQLManager().GetTablePropertiesFromSQL(tbl.ServerName, tbl.DatabaseName, tbl.Name);
                foreach (var p in props)
                {
                    sb.AppendLine($"\t\t{p} {{get;set;}}");
                }
                sb.AppendLine(Environment.NewLine);
                sb.AppendLine($"\t\tpublic override void Validate()");
                sb.AppendLine($"\t\t{{");
                sb.AppendLine($"\t\t\tthrow new NotImplementedException();");
                sb.AppendLine($"\t\t}}");
            }

           

            sb.AppendLine("\t}");

            sb.AppendLine("}");
            string result = sb.ToString();
            if (!Directory.Exists($"{OutputFolderPath}/Commands"))
            {
                Directory.CreateDirectory($"{OutputFolderPath}/Commands");
            }
            if (saveToDisk) File.WriteAllText($"{OutputFolderPath}/Commands/{tbl.Name}CommandsEvents.cs", result);
            
            return result;
        }

        #endregion

        #region Domain
        public List<string> WriteDomainClasses(BaseDataBaseContainer instance, bool saveToDisk = true)
        {
            StringBuilder sb = new StringBuilder();
            List<string> results = new List<string>();
                        
            foreach (var tbl in instance.Tables.Where(x=>x.IsSelected))
            {
                sb.AppendLine(WriteDomainClass(tbl,saveToDisk));
            }
            results.Add(sb.ToString());
            return results;
        }

        public string WriteDomainClass(IDataBaseTable tbl, bool saveToDisk = true)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using System;");
            sb.AppendLine($"namespace Mosaic.{tbl.DatabaseName}.Models");
            sb.AppendLine("{");
            if (tbl is Access.AccessInstance.AccessTable ||
                tbl is Excel.ExcelInstance.ExcelTable)
            {
                sb.AppendLine($"\tpublic partial class {tbl.Name}");
                sb.AppendLine("\t{");
                foreach (var p in tbl.Columns)
                {
                    sb.AppendLine($"\t\t public virtual {p.DataType} {p.Name.Replace(" ","")} {{get;set;}}");
                }
                sb.AppendLine("\t}");
            }
            else if (tbl is SQLInstance.SQLServerDataBase.SQLDataBaseTable)
            {
                string text = new SQLManager().GetClassFromSql(tbl.ServerName, tbl.DatabaseName, tbl.Name);
                sb.AppendLine($"\t{text}");
            }
           
            sb.AppendLine("}");
           
            if (!Directory.Exists($"{OutputFolderPath}/Models"))
            {
                Directory.CreateDirectory($"{OutputFolderPath}/Models");
            }
            if (saveToDisk) File.WriteAllText($"{OutputFolderPath}/Models/{tbl.Name}.cs", sb.ToString());
            return sb.ToString();
        }

        public string WriteStoredProcedureAsMethods(SQLInstance.SQLServerDataBase.SQLDataBaseStoredProcedure sp, bool saveToDisk = true)
        {
            
            StringBuilder sb = new StringBuilder();

            string functionName = sp.Name;
            //write signature line
            //[]
            //$"public dynamic {functionName}(
            if (sp.Parameters.Count > 0)
            {
                //add params to line
                foreach (var p in sp.Parameters)
                {

                }
            }
            //)
            //{
            //  create parameters list
            //  call the stroed procedure
            //  return result
            //}


            return sb.ToString();
        }

        #endregion

        #region Dal

        public List<string> WriteDALRepositories(BaseDataBaseContainer instance, DALTypes dalType = DALTypes.Dapper,bool saveToDisk = true)
        {
            switch (dalType)
            {
                case DALTypes.EntityFramework:
                    var efDalWriter = new CSharpEntityFrameworkDALWriter(OutputFolderPath);
                    efDalWriter.WriteBaseRepository(instance, saveToDisk);
                    return efDalWriter.WriteRepositories(instance, saveToDisk);
                case DALTypes.Linq:
                    var linqDalWriter = new CSharpLinqDALWriter(OutputFolderPath);
                    linqDalWriter.WriteBaseRepository(instance, saveToDisk);
                    return linqDalWriter.WriteRepositories(instance, saveToDisk);
                case DALTypes.Dapper:
                    var dapperDalWriter = new CSharpDapperDALWriter(OutputFolderPath);
                    dapperDalWriter.WriteBaseRepository(instance, saveToDisk);
                    return dapperDalWriter.WriteRepositories(instance, saveToDisk);
                case DALTypes.Ado:
                    var sqlDalWriter = new CSharpSQLDALWriter(OutputFolderPath);
                    sqlDalWriter.WriteBaseRepository(instance, saveToDisk);
                    return sqlDalWriter.WriteRepositories(instance, saveToDisk);
                default:
                    var defaultWriter = new CSharpSQLDALWriter(OutputFolderPath);
                    defaultWriter.WriteBaseRepository(instance, saveToDisk);
                    return defaultWriter.WriteRepositories(instance, saveToDisk);
            }
        }

        public List<string> WriteDALEntities(BaseDataBaseContainer instance, bool saveToDisk = true) {

            StringBuilder sb = new StringBuilder();
            List<string> results = new List<string>();

            foreach (var tbl in instance.Tables.Where(x => x.IsSelected))
            {
                sb.AppendLine(WriteDALEntity(tbl, saveToDisk));
            }
            results.Add(sb.ToString());
            return results;
        }

        public string WriteDALEntity(IDataBaseTable tbl, bool saveToDisk = true)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using System;");
            sb.AppendLine($"using System.ComponentModel.DataAnnotations;");
            sb.AppendLine($"using System.ComponentModel.DataAnnotations.Schema;");
            sb.AppendLine($"using Mosaic.{tbl.DatabaseName}.Core.Interfaces.Models;");
            sb.AppendLine($"namespace Mosaic.{tbl.DatabaseName}.DAL.Models");
            sb.AppendLine("{");

            if (tbl is Access.AccessInstance.AccessTable ||
                tbl is Excel.ExcelInstance.ExcelTable)
            {
                sb.AppendLine($"\tpublic partial class {tbl.Name}Entity : I{tbl.Name}");
                sb.AppendLine("\t{");
                foreach (var p in tbl.Columns)
                {
                    sb.AppendLine($"\t\t public virtual {p.DataType} {p.Name.Replace(" ", "")} {{get;set;}}");
                }
                sb.AppendLine("\t}");
            }
            else if (tbl is SQLInstance.SQLServerDataBase.SQLDataBaseTable)
            {
                string text = new SQLManager().GetDTOClassFromSql(tbl.ServerName, tbl.DatabaseName, tbl.Name);
                sb.AppendLine($"\t{text}");
            }

            sb.AppendLine("}");
            
            if (!Directory.Exists($"{OutputFolderPath}/DAL/Models"))
            {
                Directory.CreateDirectory($"{OutputFolderPath}/DAL/Models");
            }
            if (saveToDisk) File.WriteAllText($"{OutputFolderPath}/DAL/Models/{tbl.Name}Entity.cs", sb.ToString());
            return sb.ToString();
        }

        public string WriteStoredProcedure(SQLInstance.SQLServerDataBase.SQLDataBaseStoredProcedure sp, bool saveToDisk = true)
        {

            StringBuilder sb = new StringBuilder();

            string functionName = sp.Name;
            //write signature line
            //[]
            //$"public dynamic {functionName}(
            if (sp.Parameters.Count > 0)
            {
                //add params to line
                foreach (var p in sp.Parameters)
                {

                }
            }
            //)
            //{
            //  create parameters list
            //  call the stroed procedure
            //  return result
            //}


            return sb.ToString();
        }
        #endregion

        #region UI

        public List<string> WriteWebAPIControllers(BaseDataBaseContainer instance, bool saveToDisk = true)
        {
            StringBuilder sb = new StringBuilder();
            List<string> results = new List<string>();

            foreach (var tbl in instance.Tables.Where(x => x.IsSelected))
            {
                sb.AppendLine(WriteWebAPIController(instance, tbl, saveToDisk));
            }
            results.Add(sb.ToString());
            return results;
        }

        public string WriteWebAPIController(BaseDataBaseContainer instance, IDataBaseTable tbl, bool saveToDisk = true)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using System;");
            sb.AppendLine($"using System.Collections.Generic;");
            sb.AppendLine($"using System.Linq;");
            sb.AppendLine($"using System.Threading.Tasks;");
            sb.AppendLine($"using System.Web;");
            sb.AppendLine($"using System.Web.Http;");
            sb.AppendLine($"using System.Net;");
            sb.AppendLine($"using System.Net.Http;");
            sb.AppendLine($"using Mosaic.{tbl.DatabaseName}.DAL.Repositories;");
            sb.AppendLine($"using Mosaic.{tbl.DatabaseName}.Models;");
            sb.AppendLine($"using Mosaic.{tbl.DatabaseName}.MVC.Models;");
            sb.AppendLine($"");
            sb.AppendLine($"namespace Mosaic.{tbl.DatabaseName}.WebAPI.Controllers");
            sb.AppendLine("{");

            sb.AppendLine($"\t[RoutePrefix(\"{tbl.Name}s\")]");
            sb.AppendLine($"\tpublic class {tbl.Name}ApiController : ApiController");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tI{tbl.Name}Repository _{tbl.Name.ToLower()}Repository;");
            sb.AppendLine($"\t\tI{tbl.Name}ReadOnlyRepository _{tbl.Name.ToLower()}ReadOnlyRepository;");
            sb.AppendLine("\t\t");
            sb.AppendLine($"\t\tpublic {tbl.Name}ApiController(){{}}");
            sb.AppendLine("\t\t");
            sb.AppendLine($"\t\tpublic {tbl.Name}ApiController(I{tbl.Name}Repository {tbl.Name.ToLower()}Repository,I{tbl.Name}ReadOnlyRepository {tbl.Name.ToLower()}ReadOnlyRepository){{");
            sb.AppendLine($"\t\t\t_{tbl.Name.ToLower()}Repository = {tbl.Name.ToLower()}Repository;");
            sb.AppendLine($"\t\t\t_{tbl.Name.ToLower()}ReadOnlyRepository = {tbl.Name.ToLower()}ReadOnlyRepository;");
            sb.AppendLine("\t\t}");
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("\t\t[HttpGet]");
            sb.AppendLine($"\t\tpublic async Task<HttpResponseMessage> Get()");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\tvar _found = _{tbl.Name.ToLower()}ReadOnlyRepository.GetAll();");
            sb.AppendLine("\t\t\t\tvar model = (from f in _found");
            sb.AppendLine($"\t\t\t\t            select new {tbl.Name}ViewModel{{");
            sb.AppendLine("\t\t\t\t");
            sb.AppendLine("\t\t\t\t             }).AsQueryable();");
            sb.AppendLine("\t\t\t\t return Request.CreateResponse(HttpStatusCode.OK, model);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch(Exception e){");
            sb.AppendLine("\t\t\t\treturn Request.CreateErrorResponse(HttpStatusCode.Accepted, e);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("\t\t [Route(\"{id}\"), HttpGet]");
            sb.AppendLine($"\t\tpublic async Task<HttpResponseMessage> Get(int id)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\tvar _found = _{tbl.Name.ToLower()}ReadOnlyRepository.GetAll().FirstOrDefault(x=>x.Id==id);");
            sb.AppendLine($"\t\t\t\tvar model = new {tbl.Name}ViewModel(){{");
            sb.AppendLine("\t\t\t\t");
            sb.AppendLine("\t\t\t};");
            sb.AppendLine("\t\t\treturn Request.CreateResponse(HttpStatusCode.OK, model);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch(Exception e){");
            sb.AppendLine("\t\t\t\treturn Request.CreateErrorResponse(HttpStatusCode.Accepted, e);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("\t\t[HttpPost]");
            sb.AppendLine($"\t\tpublic async Task<HttpResponseMessage> Post({tbl.Name}ViewModel model)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            //sb.AppendLine($"\t\t\t_{tbl.Name.ToLower()}Repository.Create(new {tbl.Name}Entity(){});");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\t\treturn Request.CreateResponse(HttpStatusCode.OK, model);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch(Exception e){");
            sb.AppendLine("\t\t\t\treturn Request.CreateErrorResponse(HttpStatusCode.Accepted, e);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("\t\t[HttpPut]");
            sb.AppendLine($"\t\tpublic async Task<HttpResponseMessage> Put({tbl.Name}ViewModel model)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            //sb.AppendLine($"\t\t\t_{tbl.Name.ToLower()}Repository.Edit(new {tbl.Name}Entity(){});");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\t\treturn Request.CreateResponse(HttpStatusCode.OK, model);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch(Exception e){");
            sb.AppendLine("\t\t\t\treturn Request.CreateErrorResponse(HttpStatusCode.Accepted, e);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("\t\t[HttpDelete,Route(\"{id}\")]");
            sb.AppendLine($"\t\tpublic async Task Delete(int id)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            //sb.AppendLine($"\t\t\t\t_{tbl.Name.ToLower()}Repository.Delete(id);");
            sb.AppendLine("\t\t\t\treturn Request.CreateResponse(HttpStatusCode.OK, model);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch(Exception e){");
            sb.AppendLine("\t\t\t\treturn Request.CreateErrorResponse(HttpStatusCode.Accepted, e);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");

            sb.AppendLine("\t\t");
            sb.AppendLine("\t}");
            sb.AppendLine("}");           

            string folder = $"{OutputFolderPath}/UI/WebAPI/Controllers";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (saveToDisk) File.WriteAllText($"{folder}/{tbl.Name}APIController.cs", sb.ToString());
            return sb.ToString();
        }


        public List<string> WriteWebAPIProxies(BaseDataBaseContainer instance, bool saveToDisk = true)
        {
            StringBuilder sb = new StringBuilder();
            List<string> results = new List<string>();
            

            foreach (var tbl in instance.Tables.Where(x => x.IsSelected))
            {
                sb.AppendLine(WriteWebAPIProxy(instance, tbl, saveToDisk));                
            }

            results.Add(sb.ToString());

            //StringBuilder sbProxy = new StringBuilder();
            //foreach (var tbl in instance.Tables.Where(x => x.IsSelected))
            //{
            //    sbProxy.AppendLine($"import {{ Get,Put, Post ,Delete }} from '@mosaic/request';");
            //    sbProxy.AppendLine($"import GetSetting from '@mosaic/app-configuration';");
            //    sbProxy.AppendLine($"let baseUrl = null;");

            //    sbProxy.AppendLine(@"async function GetBaseUrl() {
            //        if (baseUrl)
            //            return baseUrl;
                
            //        baseUrl = await GetSetting('<Applications>_API_URL');
            //        return baseUrl;
            //    }");
            //    sbProxy.AppendLine($"");
            //    sbProxy.AppendLine(WriteWebAPIJavaScriptProxy(instance, tbl, saveToDisk));
            //}
            //string folder = $"{OutputFolderPath}/UI/WebProxies";
            //if (!Directory.Exists(folder))
            //{
            //    Directory.CreateDirectory(folder);
            //}
            //if (saveToDisk) File.WriteAllText($"{folder}/proxy.js", sb.ToString());

            return results;
        }

        public string WriteWebAPIProxy(BaseDataBaseContainer instance, IDataBaseTable tbl, bool saveToDisk = true)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using System;");
            sb.AppendLine($"using System.Collections.Generic;");
            sb.AppendLine($"using System.Linq;");
            sb.AppendLine($"using System.Threading.Tasks;");
            sb.AppendLine($"using System.Web;");
            sb.AppendLine($"using System.Web.Http;");
            sb.AppendLine($"using System.Security.Principal;");
            //sb.AppendLine($"using System.;");
            //sb.AppendLine($"using System.;");
            sb.AppendLine($"using Mosaic.{tbl.DatabaseName}.Models;");
            sb.AppendLine($"");
            sb.AppendLine($"namespace Mosaic.{tbl.DatabaseName}.WebProxy");
            sb.AppendLine("{");
            sb.AppendLine($"\tpublic static class {tbl.Name}WebProxy");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tpublic async static Task<IEnumerable<{tbl.Name}>> Get(WindowsIdentity identity)");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\treturn await Configuration.Client.Get<IEnumerable<{tbl.Name}>>($\"{tbl.Name}\", Configuration.ConverterConfiguration, identity);");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t\t");
            sb.AppendLine($"\t\tpublic async static Task<{tbl.Name}> Get(int id,WindowsIdentity identity)");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\treturn await Configuration.Client.Get<{tbl.Name}>($\"{tbl.Name}/{{id}}\", Configuration.ConverterConfiguration, identity);");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t\t");
            sb.AppendLine($"\t\tpublic async static Task<int> Post({tbl.Name} model,WindowsIdentity identity)");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\treturn await Configuration.Client.Post<int>(\"{tbl.Name}\", model, identity);");
            sb.AppendLine("\t\t}");

            sb.AppendLine($"\t\tpublic async static Task<{tbl.Name}> Put({tbl.Name} model,WindowsIdentity identity)");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\treturn await Configuration.Client.Put<{tbl.Name}>(\"{tbl.Name}\", model, identity);");
            sb.AppendLine("\t\t}");

            sb.AppendLine($"\t\tpublic async static Task<bool> Delete(int id,WindowsIdentity identity)");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\treturn await Configuration.Client.Delete<bool>(\"{tbl.Name}/{{id}}\", identity);");
            sb.AppendLine("\t\t}");

            sb.AppendLine("\t}");
            sb.AppendLine("}");


            string folder = $"{OutputFolderPath}/UI/WebProxies";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (saveToDisk) File.WriteAllText($"{folder}/{tbl.Name}WebProxy.cs", sb.ToString());
            return sb.ToString();
        }

        public string WriteWebAPIJavaScriptProxy(BaseDataBaseContainer instance, IDataBaseTable tbl, bool saveToDisk = true)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"export async function Get{tbl.Name}(obj, id) {{");
            sb.AppendLine("     const baseUrl = await GetBaseUrl();");
            sb.AppendLine("");
            sb.AppendLine($"    return await Get(baseUrl + '/{tbl.Name}';");
            sb.AppendLine("}");
            sb.AppendLine($"");
            sb.AppendLine($"export async function Add{tbl.Name}(obj) {{");
            sb.AppendLine("     const baseUrl = await GetBaseUrl();");
            sb.AppendLine("");
            sb.AppendLine($"    return await Post(baseUrl + '/{tbl.Name}', {{ obj: obj }});");
            sb.AppendLine("}");
            sb.AppendLine($"");
            sb.AppendLine($"export async function Update{tbl.Name}(obj, id) {{");
            sb.AppendLine("     const baseUrl = await GetBaseUrl();");
            sb.AppendLine("");
            sb.AppendLine($"    return await Put(baseUrl + '/{tbl.Name}', {{ obj: obj, id: id }});");
            sb.AppendLine("}");
            sb.AppendLine($"");
            sb.AppendLine($"export async function Delete{tbl.Name}(id) {{");
            sb.AppendLine("     const baseUrl = await GetBaseUrl();");
            sb.AppendLine("");
            sb.AppendLine($"    return await Delete(baseUrl + '/{tbl.Name}/' + id;");
            sb.AppendLine("}");
            sb.AppendLine($"");


            return sb.ToString();
        }


        public List<string> WriteMVCControllers(BaseDataBaseContainer instance, bool useProxy=true, bool saveToDisk = true)
        {
            StringBuilder sb = new StringBuilder();
            List<string> results = new List<string>();

            foreach (var tbl in instance.Tables.Where(x => x.IsSelected))
            {
                if (useProxy)
                {
                    sb.AppendLine(WriteWebMVCControllerUsingWebProxy(instance, tbl, saveToDisk));

                }
                else
                {
                    sb.AppendLine(WriteWebMVCController(instance, tbl, saveToDisk));
                }
            }
            results.Add(sb.ToString());
            return results;
        }

        public string WriteWebMVCController(BaseDataBaseContainer instance, IDataBaseTable tbl, bool saveToDisk = true)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using System;");
            sb.AppendLine($"using System.Collections.Generic;");
            sb.AppendLine($"using System.Linq;");
            sb.AppendLine($"using System.Threading.Tasks;");
            sb.AppendLine($"using System.Web;");
            sb.AppendLine($"using System.Web.Http;");
            sb.AppendLine($"using Mosaic.{tbl.DatabaseName}.DAL.Repositories;");
            sb.AppendLine($"using Mosaic.{tbl.DatabaseName}.Models;");
            sb.AppendLine($"using Mosaic.{tbl.DatabaseName}.MVC.Models;");
            sb.AppendLine($"");
            sb.AppendLine($"namespace Mosaic.{tbl.DatabaseName}.MVC.Controllers");
            sb.AppendLine("{");

            sb.AppendLine($"\t[RoutePrefix(\"{tbl.Name}\"]");
            sb.AppendLine($"\tpublic class {tbl.Name}Controller : Controller");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\t");
            sb.AppendLine($"\t\tpublic {tbl.Name}Controller(){{}}");
            sb.AppendLine("\t\t");           

            sb.AppendLine("\t\t[HttpGet]");
            sb.AppendLine($"\t\tpublic async Task<ActionResult> Index()");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            //sb.AppendLine($"\t\t\t\tvar _found = _{tbl.Name.ToLower()}ReadOnlyRepository.GetAll();");
            //sb.AppendLine("\t\t\t\tvar model = (from f in _found");
            //sb.AppendLine($"\t\t\t\t            select new {tbl.Name}ViewModel{{");
            //sb.AppendLine("\t\t\t\t");
            //sb.AppendLine("\t\t\t\t             }).AsQueryable();");
            sb.AppendLine("\t\t\t\t return View();");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch(Exception e){");
            sb.AppendLine("\t\t\t\tthrow e;");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");

            sb.AppendLine("\t\t [Route(\"{{id}}\"), HttpGet]");
            sb.AppendLine($"\t\tpublic async Task<ActionResult> Details(int id)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\tvar _found = _{tbl.Name.ToLower()}ReadOnlyRepository.GetAll().FirstOrDefault(x=>x.Id==id);");
            sb.AppendLine($"\t\t\t\tvar model = new {tbl.Name}ViewModel(){{");
            sb.AppendLine("\t\t\t\t");
            sb.AppendLine("\t\t\t\t};");
            sb.AppendLine("\t\t\t\treturn View();");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch(Exception e){");
            sb.AppendLine("\t\t\t\tthrow e;");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");

            sb.AppendLine("\t\t[HttpPost]");
            sb.AppendLine($"\t\tpublic async Task<ActionResult> Post({tbl.Name}ViewModel model)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            //sb.AppendLine($"\t\t\t_{tbl.Name.ToLower()}Repository.Create(new {tbl.Name}Entity(){});");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\t\t return RedirectToAction(\"Details\",model);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch(Exception e){");
            sb.AppendLine("\t\t\t\treturn View(model);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");

            sb.AppendLine("\t\t[HttpPut]");
            sb.AppendLine($"\t\tpublic async Task<ActionResult> Put({tbl.Name}ViewModel model)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            //sb.AppendLine($"\t\t\t_{tbl.Name.ToLower()}Repository.Edit(new {tbl.Name}Entity(){});");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\t\t return RedirectToAction(\"Details\",model);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch(Exception e){");
            sb.AppendLine("\t\t\t\treturn View(model);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");

            sb.AppendLine("\t\t[HttpDelete,Route(\"{id}\")]");
            sb.AppendLine($"\t\tpublic async Task<ActionResult> Delete(int id)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            //sb.AppendLine($"\t\t\t\t_{tbl.Name.ToLower()}Repository.Delete(id);");
            sb.AppendLine("\t\t\t\t return RedirectToAction(\"Index\");");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch(Exception e){");
            sb.AppendLine("\t\t\t\treturn View(model);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");


            sb.AppendLine("\t\t");
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            string folder = $"{OutputFolderPath}/UI/MVC/Controllers";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (saveToDisk) File.WriteAllText($"{folder}/{tbl.Name}Controller.cs", sb.ToString());
            return sb.ToString();
        }

        public string WriteWebMVCControllerUsingWebProxy(BaseDataBaseContainer instance, IDataBaseTable tbl, bool saveToDisk = true)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using System;");
            sb.AppendLine($"using System.Collections.Generic;");
            sb.AppendLine($"using System.Linq;");
            sb.AppendLine($"using System.Threading.Tasks;");
            sb.AppendLine($"using System.Web;");
            sb.AppendLine($"using System.Web.Http;");
            sb.AppendLine($"using Mosaic.{tbl.DatabaseName}.DAL.Repositories;");
            sb.AppendLine($"using Mosaic.{tbl.DatabaseName}.Models;");
            sb.AppendLine($"using Mosaic.{tbl.DatabaseName}.MVC.Models;");
            sb.AppendLine($"using Mosaic.{tbl.DatabaseName}.WebProxy;");            
            sb.AppendLine($"");
            sb.AppendLine($"namespace Mosaic.{tbl.DatabaseName}.MVC.Controllers");
            sb.AppendLine("{");

            sb.AppendLine($"\t[RoutePrefix(\"{tbl.Name}\"]");
            sb.AppendLine($"\tpublic class {tbl.Name}Controller : Controller");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\t");
            sb.AppendLine($"\t\tpublic {tbl.Name}Controller(){{}}");
            sb.AppendLine("\t\t");

            sb.AppendLine("\t\t[HttpGet]");
            sb.AppendLine($"\t\tpublic async Task<ActionResult> Index()");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\tvar _found = {tbl.Name}WebProxy.Get();//need to pass in the windows identity");
            sb.AppendLine("\t\t\t\tvar model = (from f in _found");
            sb.AppendLine($"\t\t\t\t            select new {tbl.Name}ViewModel{{");
            sb.AppendLine("\t\t\t\t");
            sb.AppendLine("\t\t\t\t             }).AsQueryable();");
            sb.AppendLine("\t\t\t\t return View();");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch(Exception e){");
            sb.AppendLine("\t\t\t\tthrow e;");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");

            sb.AppendLine("\t\t [Route(\"{{id}}\"), HttpGet]");
            sb.AppendLine($"\t\tpublic async Task<ActionResult> Details(int id)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\t{tbl.Name} _found = {tbl.Name}WebProxy.Get(id,);//need to pass in the windows identity");
            sb.AppendLine($"\t\t\t\tvar model = new {tbl.Name}ViewModel(){{");
            sb.AppendLine("\t\t\t\t");
            sb.AppendLine("\t\t\t};");
            sb.AppendLine("\t\t\t\t return View();");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch(Exception e){");
            sb.AppendLine("\t\t\t\tthrow e;");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");

            sb.AppendLine("\t\t[HttpPost]");
            sb.AppendLine($"\t\tpublic async Task<ActionResult> Post({tbl.Name}ViewModel model)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\t{tbl.Name} _newItem = {tbl.Name}WebProxy.Post(model,);//need to pass in the windows identity");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\t\t return RedirectToAction(\"Details\",model);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch(Exception e){");
            sb.AppendLine("\t\t\t\treturn View(model);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");

            sb.AppendLine("\t\t[HttpPut]");
            sb.AppendLine($"\t\tpublic async Task<ActionResult> Put({tbl.Name}ViewModel model)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\t{tbl.Name} _updatedModel = {tbl.Name}WebProxy.Put(model,);//need to pass in the windows identity");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\t\t return RedirectToAction(\"Details\",model);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch(Exception e){");
            sb.AppendLine("\t\t\t\treturn View(model);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");

            sb.AppendLine("\t\t[HttpDelete,Route(\"{id}\")]");
            sb.AppendLine($"\t\tpublic async Task<ActionResult> Delete(int id)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\tvar _success = {tbl.Name}WebProxy.Delete(id,);//need to pass in the windows identity");
            sb.AppendLine("\t\t\t\t return RedirectToAction(\"Index\");");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch(Exception e){");
            sb.AppendLine("\t\t\t\treturn View(model);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");


            sb.AppendLine("\t\t");
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            string folder = $"{OutputFolderPath}/UI/MVC/Controllers";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (saveToDisk) File.WriteAllText($"{folder}/{tbl.Name}Controller.cs", sb.ToString());
            return sb.ToString();
        }

        public List<string> WriteViewModels(BaseDataBaseContainer instance, bool saveToDisk = true)
        {
            StringBuilder sb = new StringBuilder();
            List<string> results = new List<string>();
            

            foreach (var tbl in instance.Tables.Where(x => x.IsSelected))
            {
                sb.AppendLine(WriteViewModel(instance, tbl, saveToDisk));
            }
            results.Add(sb.ToString());
            return results;
        }

        public string WriteViewModel(BaseDataBaseContainer instance, IDataBaseTable tbl, bool saveToDisk = true)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using System;");
            sb.AppendLine($"using System.ComponentModel.DataAnnotations;");
            sb.AppendLine($"using System.ComponentModel.DataAnnotations.Schema;");
            sb.AppendLine($"namespace Mosaic.{tbl.DatabaseName}.MVC.Models");
            sb.AppendLine("{");

            if (tbl is Access.AccessInstance.AccessTable ||
                tbl is Excel.ExcelInstance.ExcelTable)
            {
                sb.AppendLine($"\tpublic partial class {tbl.Name}ViewModel");
                sb.AppendLine("\t{");
                foreach (var p in tbl.Columns)
                {
                    sb.AppendLine($"\t\t public virtual {p.DataType} {p.Name.Replace(" ", "")} {{get;set;}}");
                }
                sb.AppendLine("\t}");
            }
            else if (tbl is SQLInstance.SQLServerDataBase.SQLDataBaseTable)
            {
                string text = new SQLManager().GetViewModelClassFromSql(tbl.ServerName, tbl.DatabaseName, tbl.Name);
                sb.AppendLine($"\t{text}");
            }
            
            sb.AppendLine("}");

            string folder = $"{OutputFolderPath}/UI/ViewModels";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (saveToDisk) File.WriteAllText($"{folder}/{tbl.Name}ViewModel.cs", sb.ToString());
            return sb.ToString();
        }

        #endregion

        #region JSON

        public List<string> WriteJSONClasses(BaseDataBaseContainer instance, bool saveToDisk = true)
        {
            StringBuilder sb = new StringBuilder();
            List<string> results = new List<string>();
            

            foreach (var tbl in instance.Tables.Where(x => x.IsSelected))
            {
                sb.AppendLine(WriteJSONClass(instance, tbl, saveToDisk));
            }
            results.Add(sb.ToString());
            return results;
        }

        public string WriteJSONClass(BaseDataBaseContainer instance, IDataBaseTable tbl, bool saveToDisk = true)
        {
            StringBuilder sb = new StringBuilder();

            if (tbl is Access.AccessInstance.AccessTable ||
                tbl is Excel.ExcelInstance.ExcelTable)
            {
                sb.AppendLine($"\tvar {tbl.Name} = {{");                
                foreach (var p in tbl.Columns)
                {
                    sb.AppendLine($"\t\t{p.Name.Replace(" ", "")} :  null,//{p.DataType}");
                }
                sb.AppendLine("\t};");
            }
            else if (tbl is SQLInstance.SQLServerDataBase.SQLDataBaseTable)
            {
                string text = new SQLManager().GetJSONClassFromSql(tbl.ServerName, tbl.DatabaseName, tbl.Name);                
                sb.AppendLine($"\t{text}");
            }

            

            if (!Directory.Exists($"{OutputFolderPath}/UI/js/models"))
            {
                Directory.CreateDirectory($"{OutputFolderPath}/UI/js/models");
            }
            if (saveToDisk) File.WriteAllText($"{OutputFolderPath}/UI/js/models/{tbl.Name}.js", sb.ToString());
            if (saveToDisk) File.WriteAllText($"{OutputFolderPath}/UI/js/models/{tbl.Name}.js", sb.ToString());
            return sb.ToString();
        }

        #endregion
    }
}
