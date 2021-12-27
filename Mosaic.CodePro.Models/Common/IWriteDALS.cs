using SensibleProgramming.Data.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensibleProgramming.CodePro.Models
{
    public interface IWriteDALS
    {
        List<string> WriteRepositories(BaseDataBaseContainer instance, bool saveToDisk);
        string WriteRepository(BaseDataBaseContainer instance, IDataBaseTable tbl, bool saveToDisk);
    }
}
