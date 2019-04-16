using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public partial class Decimal
    {
        public static System.Decimal Round(System.Decimal d, int p)
        {

            return System.Math.Round(d, p);
        }
    }
}