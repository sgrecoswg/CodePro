using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensibleProgramming.Core.Extensions.Validation
{
    public partial class Validate
    {
        public static class File
        {
            public static bool IsExcelFile(string path)
            {

                if (string.IsNullOrEmpty(path)) throw new ArgumentException("Path was null or empty");

                string ext = Path.GetExtension(path);
                if (ext != ".xls")
                    if (ext != ".xlsx")
                        throw new ArgumentException("Path was not pointing to an excel file type supported.");

                //Exists(path);

                return true;
            }

            public static bool IsWordDoc(string path)
            {
                if (string.IsNullOrEmpty(path))
                {
                    throw new ArgumentException("Path was null or empty");
                }

                string ext = Path.GetExtension(path);
                if (ext != ".doc")
                    if (ext != ".docx")
                        throw new ArgumentException("Path was not pointing to a Word document type supported.");

                // Exists(path);

                return true;
            }

            public static bool Exists(string path)
            {
                if (!System.IO.File.Exists(Path.GetFullPath(path)))
                {
                    throw new FileNotFoundException($"File was not found at {path}.");
                }
                return true;
            }
        }
    }
}
