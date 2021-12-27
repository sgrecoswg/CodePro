using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensibleProgramming.CodePro.Models.Access
{
    public class AccessInstance : BaseDataBaseContainer
    {
        public string FilePath { get; set; }

        public AccessInstance() { Tables = new List<IDataBaseTable>(); }
        public AccessInstance(string filepath) : this()
        { FilePath = filepath; }

        public class AccessTable : BaseDataBaseTable
        {
            
        }
    }


}
