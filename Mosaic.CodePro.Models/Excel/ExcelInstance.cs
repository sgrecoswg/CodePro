using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensibleProgramming.CodePro.Models.Excel
{   
    public class ExcelInstance : BaseDataBaseContainer
    {
        public string FilePath { get; set; }

        public ExcelInstance() { Tables = new List<IDataBaseTable>(); }
        public ExcelInstance(string filepath) : this()
        { FilePath = filepath; }

        public class ExcelTable : BaseDataBaseTable
        {

        }
    }
}
