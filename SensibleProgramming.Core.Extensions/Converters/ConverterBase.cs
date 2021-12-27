using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensibleProgramming.Core.Extensions
{
    public abstract class Converter<T> : IConvertTo<T>
    {
        public abstract T Convert();
    }
}
