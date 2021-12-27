﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.Core.Extensions
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class IgnoreAttribute : Attribute
    {

        public bool IgnoreMe { get; set; } = true;
        // This is a positional argument
        public IgnoreAttribute(bool ignoreMe = true)
        {
            IgnoreMe = ignoreMe;
        }


    }
}
