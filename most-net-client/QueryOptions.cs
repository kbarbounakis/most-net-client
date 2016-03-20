using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Most.Client
{
	internal class QueryOptions
	{
		public QueryOptions ()
		{
			//
		}

		public string filter {
			get;
			set;
		}

		public string select {
			get;
			set;
		}

		public string expand {
			get;
			set;
		}

		public string order {
			get;
			set;
		}

		public string group {
			get;
			set;
		}

		public object inlinecount {
			get;
			set;
		}

		public object top {
			get;
			set;
		}

		public object skip {
			get;
			set;
		}

		public object first {
			get;
			set;
		}

		public NameValueCollection ToNameValueCollection() {
			NameValueCollection result = new NameValueCollection();
			foreach (var prop in this.GetType().GetProperties()) {
				if (prop.GetValue (this) != null) {
					result.Add ("$" + prop.Name, ClientDataQueryable.Escape(prop.GetValue (this), true));
				}
			}
			return result;
		}

	}
}

