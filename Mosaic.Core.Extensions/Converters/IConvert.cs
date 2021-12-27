using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Xml;
using System.Data;
using System.IO;

namespace Mosaic.Core.Extensions
{
    public interface IConvertTo<T>
    {
        T Convert();
    }
}
