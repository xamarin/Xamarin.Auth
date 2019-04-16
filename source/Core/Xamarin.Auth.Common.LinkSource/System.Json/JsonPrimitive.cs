//
// JsonPrimitive.cs
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright (C) 2007 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;


namespace System.Json
{
    #if XAMARIN_AUTH_INTERNAL
    internal class JsonPrimitive : JsonValue
    #else
    public class JsonPrimitive : JsonValue
    #endif    	
    {
        object value;

        public JsonPrimitive(bool value)
        {
            this.value = value;
        }

        public JsonPrimitive(byte value)
        {
            this.value = value;
        }

        public JsonPrimitive(char value)
        {
            this.value = value;
        }

        public JsonPrimitive(decimal value)
        {
            this.value = value;
        }

        public JsonPrimitive(double value)
        {
            this.value = value;
        }

        public JsonPrimitive(float value)
        {
            this.value = value;
        }

        public JsonPrimitive(int value)
        {
            this.value = value;
        }

        public JsonPrimitive(long value)
        {
            this.value = value;
        }

        public JsonPrimitive(sbyte value)
        {
            this.value = value;
        }

        public JsonPrimitive(short value)
        {
            this.value = value;
        }

        public JsonPrimitive(string value)
        {
            this.value = value;
        }

        public JsonPrimitive(DateTime value)
        {
            this.value = value;
        }

        public JsonPrimitive(uint value)
        {
            this.value = value;
        }

        public JsonPrimitive(ulong value)
        {
            this.value = value;
        }

        public JsonPrimitive(ushort value)
        {
            this.value = value;
        }

        public JsonPrimitive(DateTimeOffset value)
        {
            this.value = value;
        }

        public JsonPrimitive(Guid value)
        {
            this.value = value;
        }

        public JsonPrimitive(TimeSpan value)
        {
            this.value = value;
        }

        public JsonPrimitive(Uri value)
        {
            this.value = value;
        }

        internal object Value
        {
            get { return value; }
        }

        public override JsonType JsonType
        {
            get
            {
                // FIXME: what should we do for null? Handle it as null so far.
                if (value == null)
                    return JsonType.String;

                #if !PCL && !PORTABLE && !NETFX_CORE && !WINDOWS_PHONE && !SILVERLIGHT && !WINDOWS_APP && !WINDOWS_PHONE_APP && !NETSTANDARD1_6
                switch (System.Type.GetTypeCode(value.GetType()))
                {
                    case System.TypeCode.Boolean:
                        return JsonType.Boolean;
                    case System.TypeCode.Char:
                    case System.TypeCode.String:
                    case System.TypeCode.DateTime:
                    case System.TypeCode.Object: // DateTimeOffset || Guid || TimeSpan || Uri
                        return JsonType.String;
                    default:
                        return JsonType.Number;
                }
                #elif SILVERLIGHT && WINDOWS_PHONE && !WINDOWS_PHONE_81
                switch (System.Type.GetTypeCode(value.GetType()))
                {
                    case System.TypeCode.Boolean:
                        return JsonType.Boolean;
                    case System.TypeCode.Char:
                    case System.TypeCode.String:
                    case System.TypeCode.DateTime:
                    case System.TypeCode.Object: // DateTimeOffset || Guid || TimeSpan || Uri
                        return JsonType.String;
                    default:
                        return JsonType.Number;
                }
                #elif NETSTANDARD1_6
                switch (System.Type.GetTypeCode(value.GetType()))
                {
                    case System.TypeCode.Boolean:
                        return JsonType.Boolean;
                    case System.TypeCode.Char:
                    case System.TypeCode.String:
                    case System.TypeCode.DateTime:
                    case System.TypeCode.Object: // DateTimeOffset || Guid || TimeSpan || Uri
                        return JsonType.String;
                    default:
                        return JsonType.Number;
                }
                #else
                switch (Core.Type.GetTypeCode(value.GetType()))
                {
                    case System.TypeCode.Boolean:
                        return JsonType.Boolean;
                    case System.TypeCode.Char:
                    case System.TypeCode.String:
                    case System.TypeCode.DateTime:
                    case System.TypeCode.Object: // DateTimeOffset || Guid || TimeSpan || Uri
                        return JsonType.String;
                    default:
                        return JsonType.Number;
                }
                #endif
            }
        }

        static readonly byte[] true_bytes = Encoding.UTF8.GetBytes("true");
        static readonly byte[] false_bytes = Encoding.UTF8.GetBytes("false");

        public override void Save(Stream stream)
        {
            switch (JsonType)
            {
                case JsonType.Boolean:
                    if ((bool)value)
                        stream.Write(true_bytes, 0, 4);
                    else
                        stream.Write(false_bytes, 0, 5);
                    break;
                case JsonType.String:
                    stream.WriteByte((byte)'\"');
                    byte[] bytes = Encoding.UTF8.GetBytes(EscapeString(value.ToString()));
                    stream.Write(bytes, 0, bytes.Length);
                    stream.WriteByte((byte)'\"');
                    break;
                default:
                    bytes = Encoding.UTF8.GetBytes(GetFormattedString());
                    stream.Write(bytes, 0, bytes.Length);
                    break;
            }
        }

        internal string GetFormattedString()
        {
            switch (JsonType)
            {
                case JsonType.String:
                    if (value is string || value == null)
                        return (string)value;
                    throw new NotImplementedException("GetFormattedString from value type " + value.GetType());
                case JsonType.Number:
                    return ((IFormattable)value).ToString("G", NumberFormatInfo.InvariantInfo);
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
