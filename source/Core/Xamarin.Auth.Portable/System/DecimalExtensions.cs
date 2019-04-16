using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	public static partial class DecimalExtensions
	{
		public static System.Decimal Round(this System.Decimal d)
		{

			return System.Math.Round(d);
		}

		public static System.Decimal Round(this System.Decimal d, int p)
		{

			return System.Math.Round(d, p);
		}

	}
}