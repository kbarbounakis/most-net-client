using System;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace Most.Client
{
	public class DataObject:System.Collections.Generic.Dictionary<String,Object>
	{
		public DataObject ()
		{
			//
		}

		public T getValue<T>(string key) {
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

		public Object getValue(string key) {
			return this.getValue<Object> (key);
		}

		public String getString(string key) {
			return this.getValue<String> (key);
		}

		public Int32 getInteger(string key) {
			return this.getValue<Int32> (key);
		}

		public Single getFloat(string key) {
			return this.getValue<Single> (key);
		}

		public Double getDouble(string key) {
			return this.getValue<Double> (key);
		}

		public DateTime getDateTime(string key) {
			return this.getValue<DateTime> (key);
		}

		public override string ToString ()
		{
			StringBuilder sb = new StringBuilder ();
			Newtonsoft.Json.JsonSerializer sr = new JsonSerializer ();
			sr.Formatting = Formatting.Indented;
			using (var writer = new StringWriter(sb))
			{
				using (var textWriter = new JsonTextWriter(writer))
				{
					sr.Serialize (textWriter, this);
				}
			}
			return sb.ToString();
		}

	}
}

