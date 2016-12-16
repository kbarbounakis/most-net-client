using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;

namespace Most.Client
{
	public class DataObjectConverter:JsonConverter
	{
		public DataObjectConverter ()
		{
			//
		}

		private static System.Text.RegularExpressions.Regex isDateStringRegex
			= new System.Text.RegularExpressions.Regex("^(\\d{4})(?:-?W(\\d+)(?:-?(\\d+)D?)?|(?:-(\\d+))?-(\\d+))(?:[T ](\\d+):(\\d+)(?::(\\d+)(?:\\.(\\d+))?)?)?(?:Z(-?\\d*))?([+-](\\d+):(\\d+))?$");

		private object ReadJsonArray(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			var result = new List<Object> ();
			Object value;
			while (reader.Read ()) {
				if (reader.TokenType == JsonToken.StartObject) {
					value = serializer.Deserialize<DataObject> (reader);
					result.Add (value);
				} else if (reader.TokenType == JsonToken.StartArray) {
					result.Add (this.ReadJsonArray (reader, typeof(List<Object>), null, serializer));
				} else if (reader.TokenType == JsonToken.EndArray) {
					return result;
				} else {
					if (reader.TokenType == JsonToken.String) {
						if (DataObjectConverter.isDateStringRegex.IsMatch ((String)reader.Value)) {
							result.Add (DateTime.Parse ((String)reader.Value));
						} else {
							result.Add (reader.Value);
						}
					} else {
						result.Add (reader.Value);
					}
				}
			}
			return result;
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var result = new DataObject ();
			String key;
			Object value;
			while (reader.Read ()) {
				if (reader.TokenType == JsonToken.PropertyName) {
					key = reader.Value as String;
					//next token
					reader.Read ();
					if (reader.TokenType == JsonToken.StartObject) {
						value = serializer.Deserialize<DataObject> (reader);
						result.Add (key, value);
					} else if (reader.TokenType == JsonToken.Comment) {
						//do nothing
					} else if (reader.TokenType == JsonToken.StartArray) {
						value = this.ReadJsonArray (reader, typeof(List<Object>), null, serializer);
						result.Add (key, value);
					} else if (reader.TokenType == JsonToken.StartConstructor) {
						throw new NotImplementedException ("Start constructor serialization is not supported.");
					} else if (reader.TokenType == JsonToken.String) {
						if (DataObjectConverter.isDateStringRegex.IsMatch((String)reader.Value)) {
							result.Add (key, DateTime.Parse ((String)reader.Value));
						} else {
							result.Add (key, reader.Value);
						}

					} else {
						result.Add (key, reader.Value);
					}
				} else if (reader.TokenType == JsonToken.EndObject) {
					return result;
				}
			}
			return result;
		}

		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer) {
			throw new NotImplementedException ();
		}

		public override bool CanConvert (Type objectType) {
			if (objectType.Equals (typeof(DataObject))) {
				return true;
			}
			return false;
		}
	}
}

