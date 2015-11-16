using System;
using System.Collections.Generic;

namespace Xamarin.Utilities.Android
{
	/// <summary>
	/// Object to make passing non serializable objects to Activities easier
	/// </summary>
	internal class ActivityStateRepository<T>
		where T : Java.Lang.Object
	{
		readonly Random rand = new Random ();
		readonly Dictionary<string, T> states = new Dictionary<string, T> ();

		public string Add (T state)
		{
			var key = rand.Next ().ToString ();
			while (states.ContainsKey (key)) {
				key = rand.Next ().ToString ();
			}
			states[key] = state;
			return key;
		}

		public T Remove (string key)
		{
			if (states.ContainsKey (key)) {
				var s = states[key];
				states.Remove (key);
				return s;
			}
			else {
				return default (T);
			}
		}
	}
}

