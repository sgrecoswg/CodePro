using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Core.Extensions.Validation
{
    [Serializable]
    public class ExcelSheetInvalidException : Exception
    {
        public ExcelSheetInvalidException() { }
        public ExcelSheetInvalidException(string message) : base(message) { }
        public ExcelSheetInvalidException(string message, Exception inner) : base(message, inner) { }
        protected ExcelSheetInvalidException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
