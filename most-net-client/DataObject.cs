using System;
using System.ComponentModel;

namespace Most.Client
{
	public class DataObject:System.Collections.Generic.Dictionary<String,Object>
	{
		public DataObject ()
		{
			//
		}

		public T GetValue<T>(string key) {
			if (this.ContainsKey (key)) {
				if (this [key] != null) {
					TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter (this [key]);
					if (converter.CanConvertTo (typeof(T))) {
						return (T)converter.ConvertTo (this [key], typeof(T));
					} else {
						throw new InvalidCastException ();
					}
				}
			}
			return default(T);
		}

		public String GetString(string key) {
			return this.GetValue<String> (key);
		}

		public Int32 GetInteger(string key) {
			return this.GetValue<Int32> (key);
		}

		public Single GetFloat(string key) {
			return this.GetValue<Single> (key);
		}

		public Double GetDouble(string key) {
			return this.GetValue<Double> (key);
		}

		public DateTime GetDateTime(string key) {
			return this.GetValue<DateTime> (key);
		}

	}
}

