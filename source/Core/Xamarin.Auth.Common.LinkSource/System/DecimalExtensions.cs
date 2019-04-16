using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public static partial class DecimalExtensions
    {
        public static System.Decimal Round(this System.Decimal d)
        {
            return System.Math.Round(d);
        }
    }
}
