# define PCL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace System.Reflection
{
	public static partial class Extensions
	{
		public static readonly Dictionary<Type, TypeCode> TypeCodes =
		  new Dictionary<Type, TypeCode>
        {
            { typeof(Boolean), TypeCode.Boolean },
            { typeof(Char), TypeCode.Char },
            { typeof(Byte), TypeCode.Byte },
            { typeof(Int16), TypeCode.Int16 },
            { typeof(Int32), TypeCode.Int32 },
            { typeof(Int64), TypeCode.Int64 },
            { typeof(SByte), TypeCode.SByte },
            { typeof(UInt16), TypeCode.UInt16 },
            { typeof(UInt32), TypeCode.UInt32 },
            { typeof(UInt64), TypeCode.UInt64 },
            { typeof(Single), TypeCode.Single },
            { typeof(Double), TypeCode.Double },
            { typeof(DateTime), TypeCode.DateTime },
            { typeof(Decimal), TypeCode.Decimal },
            { typeof(String), TypeCode.String },
        };

        public static TypeCode GetTypeCode(this Type type)
        {
            #if (NETFX_CORE || PCL) && !NETSTANDARD1_6
            if (type == null)
			{
				return TypeCode.Empty;
			}

			TypeCode result;
			if (!TypeCodes.TryGetValue(type, out result))
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