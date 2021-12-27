using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.CodePro.Models
{    public interface ISaveToDisk
    {
        string OutputFolderPath { get; set; }
    }
}
