using Mosaic.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.CodePro.Models
{
    public abstract class ClassWriter : Notifier, ISaveToDisk
    {
        public ClassWriter() { }
        public ClassWriter(string outputPath)
        {
            if (string.IsNullOrEmpty(outputPath))
            {
                throw new ArgumentException("outputPath was empty or null.");
            }
            OutputFolderPath = outputPath;
        }

        public string OutputFolderPath { get; set; }
    }
}
