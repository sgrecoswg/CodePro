using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Core.Extensions
{
    public abstract class Converter<T> : IConvertTo<T>
    {
        public abstract T Convert();
    }
}
