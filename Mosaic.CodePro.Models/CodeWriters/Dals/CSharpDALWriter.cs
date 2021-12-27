using Mosaic.Data.SQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.CodePro.Models.CodeWriters.Dals
{
    public abstract class CSharpDALWriter : ClassWriter, IWriteDALS
    {
        public CSharpDALWriter()
        {

        }

        public CSharpDALWriter(string outputPath) : base(outputPath)
        {

        }

        public List<string> WriteRepositories(BaseDataBaseContainer instance, bool saveToDisk)
        {
            List<string> results = new List<string>();
            foreach (var tbl in instance.Tables.Where(x => x.IsSelected))
            {
                results.Add(WriteRepository(instance, tbl, saveToDisk));
            }
            return results;
        }

        public virtual string WriteBaseRepository(BaseDataBaseContainer instance, bool saveToDisk)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using System;");
            sb.AppendLine($"using System.Collections.Generic;");
            sb.AppendLine($"using System.Data.SqlClient;");
            sb.AppendLine($"namespace Mosaic.{instance.Name}.SQL.Repositories");
            sb.AppendLine("{");
            sb.AppendLine($"\tpublic abstract class BaseRepository");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tpublic BaseRepository(){}");
            sb.AppendLine("\t\tpublic BaseRepository(string connectionString){");
            sb.AppendLine("\t\t\tConnectionString = connectionString;");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t\tpublic string ConnectionString { get; set; }");
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            if (!Directory.Exists($"{OutputFolderPath}/DAL/Repositories"))
            {
                Directory.CreateDirectory($"{OutputFolderPath}/DAL/Repositories");
            }
            if (saveToDisk) File.WriteAllText($"{OutputFolderPath}/DAL/Repositories/BaseRepository.cs", sb.ToString());
            return sb.ToString();
        }

        public abstract string WriteRepository(BaseDataBaseContainer instance, IDataBaseTable tbl, bool saveToDisk);

    }
}
