using Mosaic.Core.Models;
using Mosaic.Data.SQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.CodePro.Models.CodeWriters.Dals
{
    public class CSharpDapperDALWriter: CSharpDALWriter, IWriteDALS
    {
        public CSharpDapperDALWriter()
        {

        }

        public CSharpDapperDALWriter(string outputPath) : base(outputPath)
        {

        }

        public override string WriteRepository(BaseDataBaseContainer instance, IDataBaseTable tbl, bool saveToDisk)
        {
            string result = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using System;");
            sb.AppendLine($"using System.Collections.Generic;");
            sb.AppendLine($"using Dapper;");
            sb.AppendLine($"namespace Mosaic.{instance.Name}.DAL.Repositories");
            sb.AppendLine("{");

            sb.AppendLine($"\tpublic interface I{tbl.Name}Repository");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tList<{tbl.Name}Entity> GetAll();");
            sb.AppendLine($"\t\t{tbl.Name}Entity GetById(int id);");
            sb.AppendLine($"\t\t{tbl.Name}Entity Create({tbl.Name}Entity model);");
            sb.AppendLine($"\t\t{tbl.Name}Entity Edit({tbl.Name}Entity model);");
            sb.AppendLine($"\t\t{tbl.Name}Entity Delete(int id);");
            sb.AppendLine($"\t\t{tbl.Name}Entity Delete({tbl.Name}Entity model);");
            sb.AppendLine("\t}");

            sb.AppendLine($"\tpublic class {instance.Name}Repository : BaseRepository,I{tbl.Name}Repository");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tpublic {instance.Name}Repository(){{}}");
            sb.AppendLine($"\t\tpublic {instance.Name}Repository(string connectionString) :base(connectionString){{}}");
            sb.AppendLine($"\t\tpublic List<{instance.Name}Entity> GetAll(){{}}");
            sb.AppendLine($"\t\tpublic {instance.Name}Entity GetById(int id){{}}");
            sb.AppendLine($"\t\tpublic {instance.Name}Entity Create({instance.Name}Entity model){{}}");
            sb.AppendLine($"\t\tpublic {instance.Name}Entity Edit({instance.Name}Entity model){{}}");
            sb.AppendLine($"\t\tpublic {instance.Name}Entity Delete(int id){{}}");
            sb.AppendLine($"\t\tpublic {instance.Name}Entity Delete({instance.Name}Entity model){{}}");
            sb.AppendLine("\t}");

            sb.AppendLine($"\tpublic interface I{tbl.Name}ReadonlyRepository");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tList<{tbl.Name}Entity> GetAll();");
            sb.AppendLine($"\t\t{tbl.Name}Entity GetById(int id);");
            sb.AppendLine("\t}");

            sb.AppendLine($"\tpublic class {instance.Name}ReadOnlyRepository : BaseRepository,I{tbl.Name}ReadOnlyRepository");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tpublic {instance.Name}ReadOnlyRepository(){{}}");
            sb.AppendLine($"\t\tpublic {instance.Name}ReadOnlyRepository(string connectionString) :base(connectionString){{}}");
            sb.AppendLine($"\t\tpublic IQueryable<{instance.Name}Entity> GetAll(){{}}");
            sb.AppendLine($"\t\tpublic {instance.Name}Entity GetById(int id){{}}");
            sb.AppendLine("\t}");

            sb.AppendLine("}");
            string folder = $"{OutputFolderPath}/DAL/Repositories/Dapper";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (saveToDisk) File.WriteAllText($"{folder}/{tbl.Name}Repository.cs", sb.ToString());
            return sb.ToString();
        }
    }

}
