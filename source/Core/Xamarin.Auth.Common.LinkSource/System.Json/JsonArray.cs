//
// JsonArray.cs
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
	internal class JsonArray : JsonValue, IList<JsonValue>
    #else
    public class JsonArray : JsonValue, IList<JsonValue>
    #endif
    {
        List<JsonValue> list;

        public JsonArray(params JsonValue[] items)
        {
            list = new List<JsonValue>();
            AddRange(items);
        }

        public JsonArray(IEnumerable<JsonValue> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            list = new List<JsonValue>(items);
        }

        public override int Count
        {
            get { return list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public override sealed JsonValue this[int index]
        {
            get { return list[index]; }
            set { list[index] = value; }
        }

        public override JsonType JsonType
        {
            get { return JsonType.Array; }
        }

        public void Add(JsonValue item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            list.Add(item);
        }

        public void AddRange(IEnumerable<JsonValue> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            list.AddRange(items);
        }

        public void AddRange(params JsonValue[] items)
        {
            if (items == null)
                return;

            list.AddRange(items);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(JsonValue item)
        {
            return list.Contains(item);
        }

        public void CopyTo(JsonValue[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public int IndexOf(JsonValue item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, JsonValue item)
        {
            list.Insert(index, item);
        }

        public bool Remove(JsonValue item)
        {
            return list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public override void Save(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            stream.WriteByte((byte)'[');
            for (int i = 0; i < list.Count; i++)
            {
                JsonValue v = list[i];
                if (v != null)
                    v.Save(stream);
                else
                {
                    stream.WriteByte((byte)'n');
                    stream.WriteByte((byte)'u');
                    stream.WriteByte((byte)'l');
                    stream.WriteByte((byte)'l');
                }

                if (i < Count - 1)
                {
                    stream.WriteByte((byte)',');
                    stream.WriteByte((byte)' ');
                }
            }
            stream.WriteByte((byte)']');
        }

        IEnumerator<JsonValue> IEnumerable<JsonValue>.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}