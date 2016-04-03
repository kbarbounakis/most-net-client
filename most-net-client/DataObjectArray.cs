using System;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace Most.Client
{
	public class DataObjectArray:System.Collections.Generic.List<DataObject>
	{
		public DataObjectArray ()
		{
			
		}

	

		private object total_;

		public object total {
			get {
				return total_;
			}
		}

		internal void setTotal(object num) {
			total_ = num;
		}

		private object skip_;

		public object skip {
			get {
				return skip_;
			}
		}

		internal void setSkip(object num) {
			skip_ = num;
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

