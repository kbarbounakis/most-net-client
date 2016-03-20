using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Net;
using System.Collections.Specialized;

namespace Most.Client
{
	public class ServiceExecuteOptions
	{
		public ServiceExecuteOptions ()
		{
			this.Headers = new WebHeaderCollection ();
			this.Query = new NameValueCollection ();
			this.Method = HttpMethod.Get;
		}

		public string Url {
			get;
			set;
		}

		public HttpMethod Method {
			get;
			set;
		}

		public object Data {
			get;
			set;
		}

		public NameValueCollection Query {
			get;
			set;
		}

		public WebHeaderCollection Headers {
			get;
			set;
		}

	}
}

