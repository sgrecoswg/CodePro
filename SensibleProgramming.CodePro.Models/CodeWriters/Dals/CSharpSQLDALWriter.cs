using SensibleProgramming.Data.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SensibleProgramming.CodePro.Models.CodeWriters.Dals
{
    public class CSharpSQLDALWriter : CSharpDALWriter, IWriteDALS
    {
        public CSharpSQLDALWriter()
        {

        }

        public CSharpSQLDALWriter(string outputPath) : base(outputPath)
        {

        }

        public override string WriteRepository(BaseDataBaseContainer instance, IDataBaseTable tbl, bool saveToDisk)
        {
            string result = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using System;");
            sb.AppendLine($"using System.Collections.Generic;");            
            sb.AppendLine($"using System.Data.SqlClient;");
            sb.AppendLine($"namespace SensibleProgramming.{instance.Name}.SQL.Repositories");
            sb.AppendLine("{");

            sb.AppendLine($"\tpublic class I{tbl.Name}Repository");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tList<{tbl.Name}Entity> GetAll(){{}}");
            sb.AppendLine($"\t\t{tbl.Name}Entity GetById(int id){{}}");
            sb.AppendLine($"\t\t{tbl.Name}Entity Create({tbl.Name}Entity model){{}}");
            sb.AppendLine($"\t\t{tbl.Name}Entity Edit({tbl.Name}Entity model){{}}");
            sb.AppendLine($"\t\t{tbl.Name}Entity Delete(int id){{}}");
            sb.AppendLine($"\t\t{tbl.Name}Entity Delete({tbl.Name}Entity model){{}}");
            sb.AppendLine("\t}");

            sb.AppendLine($"\tpublic class {tbl.Name}Repository : BaseRepository");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tpublic {tbl.Name}Repository(){{}}");
            sb.AppendLine($"\t\tpublic {tbl.Name}Repository(string connectionString) :base(connectionString){{}}");
            sb.AppendLine($"\t\tpublic List<{tbl.Name}Entity> GetAll(){{}}");
            sb.AppendLine($"\t\tpublic {tbl.Name}Entity GetById(int id){{}}");
            sb.AppendLine($"\t\tpublic {tbl.Name}Entity Create({tbl.Name}Entity model){{}}");
            sb.AppendLine($"\t\tpublic {tbl.Name}Entity Edit({tbl.Name}Entity model){{}}");
            sb.AppendLine($"\t\tpublic {tbl.Name}Entity Delete(int id){{}}");
            sb.AppendLine($"\t\tpublic {tbl.Name}Entity Delete({tbl.Name}Entity model){{}}");
            sb.AppendLine("\t}");

            sb.AppendLine($"\tpublic class {tbl.Name}ReadOnlyRepository : BaseRepository");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tpublic {tbl.Name}ReadOnlyRepository(){{}}");
            sb.AppendLine($"\t\tpublic {tbl.Name}ReadOnlyRepository(string connectionString) :base(connectionString){{}}");
            sb.AppendLine($"\t\tpublic IQueryable<{tbl.Name}Entity> GetAll(){{}}");
            sb.AppendLine($"\t\tpublic {tbl.Name}Entity GetById(int id){{}}");
            sb.AppendLine("\t}");

            sb.AppendLine("}");
            string folder = $"{OutputFolderPath}/DAL/Repositories/SQL";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (saveToDisk) File.WriteAllText($"{OutputFolderPath}/DAL/SQL/Repositories/{tbl.Name}Repository.cs", sb.ToString());
            return sb.ToString();
        }
    }
}
