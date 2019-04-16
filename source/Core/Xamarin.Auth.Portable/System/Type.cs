# define PCL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace Core
{
	public partial class Type
	{
		public static TypeCode GetTypeCode(System.Type type)
		{
            #if NETFX_CORE || PCL || PORTABLE
			if (type == null)
			{
				return TypeCode.Empty;
			}

			TypeCode result;
			if (!System.Reflection.Extensions.TypeCodes.TryGetValue(type, out result))
			{
				result = TypeCode.Object;
			}

			return result;

#else
				return Type.GetTypeCode(type);
#endif
		}

	}
}