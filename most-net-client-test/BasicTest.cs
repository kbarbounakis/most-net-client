using NUnit.Framework;
using System;

namespace Most.Client.Test
{
	[TestFixture ()]
	public class BasicTest
	{
		[Test ()]
		public void TestCase ()
		{
			Most.Client.ClientDataService svc = new ClientDataService ("http://mw-admin-kbarbounakis.c9users.io/");
			Object result = svc.Authenticate("alexis.rees@example.com","user").DoGet("/Group/index.json");
			Console.WriteLine (result.GetType ().ToString ());
		}
	}
}

