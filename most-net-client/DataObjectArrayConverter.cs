using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;

namespace Most.Client
{
	public class DataObjectArrayConverter:JsonConverter
	{
		public DataObjectArrayConverter ()
		{
			
		}
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			DataObjectArray result = new DataObjectArray ();
			if (reader.TokenType == JsonToken.StartArray) {
				while (reader.Read ()) {
					if (reader.TokenType == JsonToken.StartObject) {
						result.Add (serializer.Deserialize<DataObject> (reader));
					} else if (reader.TokenType == JsonToken.EndArray) {
						return result;
					}
				}
			}
			else if (reader.TokenType == JsonToken.StartObject) {
				object total = 0;
				object skip = 0;
				while (reader.Read ()) {
					if (reader.TokenType == JsonToken.PropertyName) {
						if ((String)reader.Value == "total") {
							//next token
							reader.Read ();
							total = reader.Value;
						} else if ((String)reader.Value == "skip") {
							//next token
							reader.Read ();
							skip = reader.Value;
						} else if ((String)reader.Value == "records") {
							//next token
							reader.Read ();
							if (reader.TokenType == JsonToken.StartArray) {
								result = (DataObjectArray)this.ReadJson (reader, typeof(DataObjectArray), null, serializer);
							}
						}
					} else if (reader.TokenType == JsonToken.EndObject) {
						result.setTotal (total);
						result.setSkip (skip);
						return result;
					}
				}
			}
			return result;
		}

		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer) {
			throw new NotImplementedException ();
		}

		public override bool CanConvert (Type objectType) {
			if (objectType.Equals (typeof(DataObjectArray))) {
				return true;
			}
			return false;
		}
	}
}

