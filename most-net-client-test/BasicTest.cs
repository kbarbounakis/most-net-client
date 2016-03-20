using NUnit.Framework;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Configuration;

namespace Most.Client.Test
{
	[TestFixture ()]
	public class BasicTest
	{
		[Test ()]
		public void TestCase ()
		{

			var result = Most.Client.ClientDataContext
				.Create (ConfigurationManager.AppSettings["Server"])
				.Authenticate (ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"])
				.Model("Group").Schema();
			Console.WriteLine (result.ToString ());
			
		}

		[Test ()]
		public void TestGetProducts ()
		{

			var result = Most.Client.ClientDataContext
				.Create (ConfigurationManager.AppSettings ["Server"])
				.Authenticate (ConfigurationManager.AppSettings ["Username"], ConfigurationManager.AppSettings ["Password"])
				.Model ("Product").OrderBy ("name")
				.GetItems ();
			Console.WriteLine (result.ToString ());

		}

	}
}

