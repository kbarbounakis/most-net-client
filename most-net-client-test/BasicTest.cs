using NUnit.Framework;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Configuration;
using System.Dynamic;

namespace Most.Client.Test
{
	[TestFixture ()]
	public class BasicTest
	{
		private ClientDataContext context_;

		private ClientDataContext getContext ()
		{
			if (context_ == null) {
				context_ = Most.Client.ClientDataContext
					.create (ConfigurationManager.AppSettings ["Server"])
					.authenticate (ConfigurationManager.AppSettings ["Username"], ConfigurationManager.AppSettings ["Password"]);
			}
			return context_;
		}

		[Test ()]
		public void TestGetSchema ()
		{

			var result = this.getContext()
				.model("Product").getSchema() as DataObject;
			//enumerate attributes
			List<Object> attributes = result ["attributes"] as List<Object>;
			foreach (DataObject attribute in attributes) {
				Console.WriteLine (attribute["name"]);
			}

			
		}

		[Test ()]
		public void TestSelect ()
		{
			var result =this.getContext()
				.model ("Order")
				.select("id","customer","orderedItem","orderStatus")
				.orderBy ("name")
				.take(25)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestSkip ()
		{
			var result =this.getContext()
				.model ("Order")
				.skip(10)
				.take(10)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestTake ()
		{
			var result =this.getContext()
				.model ("Order")
				.take(10)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestWhere ()
		{
			var result =this.getContext()
				.model ("Order")
				.where("orderedItem/category").equal("Laptops")
				.take(10)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestOr ()
		{
			var result =this.getContext()
				.model("Product")
				.where("category").equal("Desktops")
				.or("category").equal("Laptops")
				.orderBy("price")
				.take(5)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestAnd()
		{
			var result =this.getContext()
				.model("Product")
				.where("category").equal("Desktops")
				.and("price").between(200,750)
				.orderBy("price")
				.take(5)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}


		[Test ()]
		public void TestEqual()
		{
			var result = this.getContext ()
				.model ("Order")
				.where ("id").equal (10)
				.getItem ();
			Console.WriteLine (result.ToString ());
		}


		[Test ()]
		public void TestnotEqual()
		{
			var result = this.getContext ().model("Order")
				.where ("orderStatus/alternateName").notEqual ("orderProblem")
				.orderByDescending ("orderDate")
				.take (10)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestGreaterThan()
		{
			var result = this.getContext ().model("Order")
				.where("orderedItem/price").greaterThan(968)
				.and("orderedItem/category").equal("Laptops")
				.and("orderStatus/alternateName").notEqual("orderCancelled")
				.select("id",
					"orderStatus/name as orderStatusName",
					"customer/description as customerDescription",
					"orderedItem")
				.orderByDescending("orderDate")
				.take (10)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}


		[Test ()]
		public void TestContains()
		{
			var result = this.getContext ().model("Product")
				.where("name").contains("Book")
				.and("category").equal("Laptops")
				.orderBy("price")
				.take (10)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestBetween()
		{
			var result = this.getContext ().model("Product")
					.where("category").equal("Laptops")
				.or("category").equal("Desktops")
				.andAlso("price").between(200,750)
				.orderBy("price")
				.take(5)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestCount()
		{
			var result = this.getContext ().model("Product")
				.select("category", "count(id) as total")
				.groupBy("category")
				.orderByDescending("count(id)")
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestMin()
		{
			var result = this.getContext ().model("Product")
				.select("category", "min(price) as minimumPrice")
					.where("category").equal("Laptops")
				.or("category").equal("Desktops")
				.groupBy("category")
				.orderByDescending("min(price)")
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestMax()
		{
			var result = this.getContext ().model("Product")
				.select("category", "max(price) as maximumPrice")
				.where("category").equal("Laptops")
				.getItem ();
			Console.WriteLine (result.ToString ());
		}


		[Test ()]
		public void TestGreaterOrEqual()
		{
			var result = this.getContext ().model("Product")
					.where("price").greaterOrEqual(1395.9)
				.orderByDescending("price")
				.take (10)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestIndexOf()
		{
			var result = this.getContext ().model("Product")
				.where("name").indexOf("Intel")
				.greaterOrEqual(0)
				.take (10)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestSubstring()
		{
			var result = this.getContext ().model("Product")
				.where("name").substr(6,4)
				.equal("Core")
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestGetProducts ()
		{
			var result =this.getContext()
				.model ("Product").orderBy ("name")
				.getItems ();
			Console.WriteLine (result.ToString ());

		}

		[Test ()]
		public void TestGetTotalProductsByCategory ()
		{
			//Get total products per category
			var result = this.getContext ()
				.model ("Product").orderBy ("name")
				.select ("category", "count(id) as totalCount")
				.groupBy("category")
				.orderBy("category")
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestGetProductsByCategoryandPrice ()
		{
			//Get a list of products with category equal to Laptops and price lower than 500
			var result = this.getContext ()
				.model ("Product").where("category").equal("Laptops")
				.and("price").lowerThan(500)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestStartsWith ()
		{
			//Get a list of products where name starts with 'Apple'
			var result = this.getContext ()
				.model ("Product").where("name")
				.startsWith("Apple")
				.equal(true)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestEndsWith ()
		{
			//Get a list of products where name ends with 'Apple'
			var result = this.getContext ()
				.model ("Product").where("name")
				.endsWith("(7-Inch)")
				.equal(true)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestLowerCase ()
		{
			var result = this.getContext ().model ("Product")
					.where ("category").toLowerCase ()
				.equal ("laptops")
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestUpperCase ()
		{
			var result = this.getContext ().model ("Product")
					.where("category").toUpperCase()
				.equal("LAPTOPS")
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestGetDate ()
		{
			var result = this.getContext().model("Order")
				.where("orderDate").getDate()
				.equal("2015-04-18")
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestGetMonth ()
		{
			var result = this.getContext().model("Order")
					.where("orderDate").getMonth()
				.equal(4)
				.orderByDescending("orderDate")
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestGetDay ()
		{
			var result = this.getContext().model("Order")
					.where("orderDate").getMonth().equal(4)
				.and("orderDate").getDay().lowerThan(15)
				.orderByDescending("orderDate")
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestGetYear ()
		{
			var result = this.getContext().model("Order")
					.where("orderDate").getMonth().equal(5)
				.and("orderDate").getDay().lowerOrEqual(10)
				.and("orderDate").getFullYear().equal(2015)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestGetHours ()
		{
			var result = this.getContext().model("Order")
					.where("orderDate").getMonth().equal(5)
				.and("orderDate").getDay().lowerOrEqual(10)
				.and("orderDate").getHours().between(10,18)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestGetMinutes ()
		{
			var result = this.getContext().model("Order")
					.where("orderDate").getMonth().equal(5)
				.and("orderDate").getHours().between(9,17)
				.and("orderDate").getMinutes().between(1,30)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestGetSeconds ()
		{
			var result = this.getContext().model("Order")
					.where("orderDate").getMonth().equal(5)
				.and("orderDate").getHours().between(9,17)
				.and("orderDate").getMinutes().between(1,30)
				.and("orderDate").getSeconds().between(1,45)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestRound ()
		{
			var result = this.getContext().model("Product")
					.where("price").round().lowerOrEqual(177)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestExpand ()
		{
			var result = this.getContext().model("Order")
					.where("customer").equal(337)
					.orderByDescending("orderDate")
					.expand("customer")
					.getItems ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestGetList ()
		{
			var result = this.getContext ().model ("Product")
					.where ("category").equal ("Laptops")
				.skip (5)
				.take (5)
				.getList ();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestGetOrderAndUpdate ()
		{
			var order = this.getContext ().model ("Order")
					.where ("id").equal (21)
				.getItem<DataObject>();
			order ["orderStatus"] = new {
				alternateName = "OrderDelivered"

			};
			this.getContext ().model ("Order").save (order);
		}

		[Test ()]
		public void TestRemoveOrder ()
		{
			var context = this.getContext ();
			var order = new {
				id = 22
			};
			context.model ("Order").remove (order);
		}

		[Test ()]
		public void TestFirst ()
		{
			var result = this.getContext().model("User")
					.where("name").equal("alexis.rees@example.com")
				.first<DataObject>();
			Console.WriteLine (result.ToString ());
		}

		[Test ()]
		public void TestAndAlso ()
		{
			var result = this.getContext ().model ("Product")
				.where ("category").equal ("Laptops")
				.or ("category").equal ("Desktops")
				.andAlso ("price").greaterOrEqual (177)
				.getItems ();
			Console.WriteLine (result.ToString ());
		}

	}
}

