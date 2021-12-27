using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensibleProgramming.CodePro.Models
{    public interface ISaveToDisk
    {
        string OutputFolderPath { get; set; }
    }
}
