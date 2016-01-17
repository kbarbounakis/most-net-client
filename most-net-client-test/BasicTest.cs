using NUnit.Framework;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Most.Client.Test
{
	[TestFixture ()]
	public class BasicTest
	{
		[Test ()]
		public void TestCase ()
		{
			Most.Client.ClientDataService svc = new ClientDataService ("http://mw-admin-kbarbounakis.c9users.io/");
			Object result = svc.authenticate("alexis.rees@example.com","user").get("/Group/index.json", new Dictionary<String,Object>());
			Console.WriteLine (result.GetType ().ToString ());
		}

		[Test ()]
		public void TestValueSerialization ()
		{
			JsonSerializerSettings settings = new JsonSerializerSettings ();
			Console.WriteLine (JsonConvert.SerializeObject (null));
			Console.WriteLine (JsonConvert.SerializeObject (100));
			Console.WriteLine (JsonConvert.SerializeObject ("Hello World!", Newtonsoft.Json.Formatting.None));
			Console.WriteLine (JsonConvert.SerializeObject (DateTime.Now));
		}
	}
}

