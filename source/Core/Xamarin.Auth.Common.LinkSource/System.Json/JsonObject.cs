//
// JsonObject.cs
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

using JsonPair = System.Collections.Generic.KeyValuePair<string, System.Json.JsonValue>;
using JsonPairEnumerable = System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, System.Json.JsonValue>>;

namespace System.Json
{
    #if XAMARIN_AUTH_INTERNAL
    internal class JsonObject : JsonValue, IDictionary<string, JsonValue>, ICollection<JsonPair>
    #else
    public class JsonObject : JsonValue, IDictionary<string, JsonValue>, ICollection<JsonPair>
    #endif
    {
        Dictionary<string, JsonValue> map;

        public JsonObject(params JsonPair[] items)
        {
            map = new Dictionary<string, JsonValue>();

            if (items != null)
                AddRange(items);
        }

        public JsonObject(JsonPairEnumerable items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            map = new Dictionary<string, JsonValue>();
            AddRange(items);
        }

        public override int Count
        {
            get { return map.Count; }
        }

        public IEnumerator<JsonPair> GetEnumerator()
        {
            return map.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return map.GetEnumerator();
        }

        public override sealed JsonValue this[string key]
        {
            get { return map[key]; }
            set { map[key] = value; }
        }

        public override JsonType JsonType
        {
            get { return JsonType.Object; }
        }

        public ICollection<string> Keys
        {
            get { return map.Keys; }
        }

        public ICollection<JsonValue> Values
        {
            get { return map.Values; }
        }

        public void Add(string key, JsonValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            map.Add(key, value);
        }

        public void Add(JsonPair pair)
        {
            Add(pair.Key, pair.Value);
        }

        public void AddRange(JsonPairEnumerable items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (var pair in items)
                map.Add(pair.Key, pair.Value);
        }

        public void AddRange(params JsonPair[] items)
        {
            AddRange((JsonPairEnumerable)items);
        }

        public void Clear()
        {
            map.Clear();
        }

        bool ICollection<JsonPair>.Contains(JsonPair item)
        {
            return (map as ICollection<JsonPair>).Contains(item);
        }

        bool ICollection<JsonPair>.Remove(JsonPair item)
        {
            return (map as ICollection<JsonPair>).Remove(item);
        }

        public override bool ContainsKey(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            return map.ContainsKey(key);
        }

        public void CopyTo(JsonPair[] array, int arrayIndex)
        {
            (map as ICollection<JsonPair>).CopyTo(array, arrayIndex);
        }

        public bool Remove(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            return map.Remove(key);
        }

        bool ICollection<JsonPair>.IsReadOnly
        {
            get { return false; }
        }

        public override void Save(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            stream.WriteByte((byte)'{');
            foreach (JsonPair pair in map)
            {
                stream.WriteByte((byte)'"');
                byte[] bytes = Encoding.UTF8.GetBytes(EscapeString(pair.Key));
                stream.Write(bytes, 0, bytes.Length);
                stream.WriteByte((byte)'"');
                stream.WriteByte((byte)',');
                stream.WriteByte((byte)' ');
                if (pair.Value == null)
                {
                    stream.WriteByte((byte)'n');
                    stream.WriteByte((byte)'u');
                    stream.WriteByte((byte)'l');
                    stream.WriteByte((byte)'l');
                }
                else
                    pair.Value.Save(stream);
            }
            stream.WriteByte((byte)'}');
        }

        public bool TryGetValue(string key, out JsonValue value)
        {
            return map.TryGetValue(key, out value);
        }
    }
}