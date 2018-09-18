using System;
using System.Collections.Generic;

namespace Xamarin.Auth
{
    internal class ActivityStateRepository<T>
    {
        private readonly Dictionary<string, T> states = new Dictionary<string, T>();

        public string Add(T state)
        {
            var key = Guid.NewGuid().ToString();
            while (states.ContainsKey(key))
            {
                key = Guid.NewGuid().ToString();
            }

            states[key] = state;
            return key;
        }

        public T Remove(string key)
        {
            if (states.Remove(key, out var value))
                return value;

            return default;
        }
    }
}
