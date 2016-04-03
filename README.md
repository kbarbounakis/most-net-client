# MOST Web Framework .NET Client

A .NET client library for connecting to a remote [MOST Web Framework](https://github.com/kbarbounakis/most-web) server

![MOST Web Framework Logo](https://www.themost.io/assets/images/most_logo_sw_240.png)

### Usage

If you don't have a MOST Web Framework application clone [MOST Web Framework OMS Demo](https://github.com/kbarbounakis/most-web-oms-demo) application and follow the installation instructions.

Create a new .NET Framework project and use MOST Web Framework .NET Client to communicate with your application.

### Authentication

Use basic authentication:

	using Most.Client;

	var context = new ClientDataContext("http://127.0.0.1:3000");
    context.authenticate("alexis.rees@example.com","user")

Use cookie based authentication (node.js environment):

    context.getService().setCookie(cookie);

### ClientDataContext Class

#### model(name)

Gets an instance of ClientDataModel class based on the given name.

	using Most.Client;

    DataObjectArray result = context.model("Order").where("orderStatus").equal(1).getItems();

#### getService()

Gets the instance of ClientDataService associated with this data context.

	using Most.Client;

	var context = new ClientDataContext("http://127.0.0.1:3000");
    Console.WriteLine (context.getService().getBase());

#### setService(service)

Associates the given ClientDataService instance with this data context.

	using Most.Client;

	var context = new ClientDataContext();
    context.setService(new MyDataService("http://data.example.com"));

### ClientDataModel Class

#### getName()

Gets a string which represents the name of this data model.

#### getService()

Gets the instance of ClientDataService associated with this data model.

#### remove(obj)

Removes the given item or array of items.

    using Most.Client;
	using System.Dynamic;

	var order = new {
		id = 22
	};
	context.model ("Order").remove (order);

#### save(obj)

Creates or updates the given item or array of items.

	using Most.Client;
	using System.Dynamic;

    var order = context.model ("Order")
				.where ("id").equal (21)
				.getItem<DataObject>();
	order ["orderStatus"] = new {
		alternateName = "OrderDelivered"
	};
	context.model ("Order").save (order);

#### getSchema()

Returns the JSON schema of this data model.

	var result = context
		.model("Product").getSchema() as DataObject;
	//enumerate attributes
	List<Object> attributes = result ["attributes"] as List<Object>;
	foreach (DataObject attribute in attributes) {
		Console.WriteLine (attribute["name"]);
	}


#### select(...attr)

Initializes and returns an instance of ClientDataQueryable class by selecting an attribute or a collection of attributes.

    var result =this.getContext()
		.model ("Order")
		.select("id","customer","orderedItem","orderStatus")
		.orderBy ("name")
		.take(25)
		.getItems ();
	Console.WriteLine (result.ToString ());

#### skip(num)

Initializes and returns an instance of ClientDataQueryable class by specifying the number of records to be skipped.

    var result =this.getContext()
				.model ("Order")
				.skip(10)
				.take(10)
				.getItems ();
	Console.WriteLine (result.ToString ());

#### take(num)

Initializes and returns an instance of ClientDataQueryable class by specifying the number of records to be taken.

	var result =this.getContext()
		.model ("Order")
		.take(10)
		.getItems ();
	Console.WriteLine (result.ToString ());

#### where(attr)

Initializes a comparison expression by using the given attribute as left operand
and returns an instance of ClientDataQueryable class.

    var result =this.getContext()
		.model ("Order")
		.where("orderedItem/category").equal("Laptops")
		.take(10)
		.getItems ();
	Console.WriteLine (result.ToString ());

### ClientDataQueryable Class

ClientDataQueryable class enables developers to perform simple and extended queries against data models.
The ClienDataQueryable class follows [DataQueryable](https://docs.themost.io/most-data/DataQueryable.html)
which is introduced by [MOST Web Framework ORM server-side module](https://github.com/kbarbounakis/most-data).

#### Logical Operators

Or:

    var result =this.getContext()
		.model("Product")
		.where("category").equal("Desktops")
		.or("category").equal("Laptops")
		.orderBy("price")
		.take(5)
		.getItems ();
	Console.WriteLine (result.ToString ());

And:

    var result =this.getContext()
		.model("Product")
		.where("category").equal("Desktops")
		.and("price").between(200,750)
		.orderBy("price")
		.take(5)
		.getItems ();
	Console.WriteLine (result.ToString ());

#### Comparison Operators

Equal:

    var result = this.getContext ()
		.model ("Order")
		.where ("id").equal (10)
		.getItem ();
	Console.WriteLine (result.ToString ());

Not equal:

    var result = this.getContext ().model("Order")
		.where ("orderStatus/alternateName").notEqual ("orderProblem")
		.orderByDescending ("orderDate")
		.take (10)
		.getItems ();
	Console.WriteLine (result.ToString ());

Greater than:

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

Greater or equal:

   var result = this.getContext ().model("Product")
		.where("price").greaterOrEqual(1395.9)
		.orderByDescending("price")
		.take (10)
		.getItems ();
	Console.WriteLine (result.ToString ());

Lower than:

    var result = this.getContext ().model("Order")
		.where("orderedItem/price").lowerThan(968)
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

Lower or equal:

    var result = this.getContext ().model("Product")
		.where("price").lowerOrEqual(1395.9)
		.orderByDescending("price")
		.take (10)
		.getItems ();
	Console.WriteLine (result.ToString ());

Contains:

    var result = this.getContext ().model("Product")
		.where("name").contains("Book")
		.and("category").equal("Laptops")
		.orderBy("price")
		.take (10)
		.getItems ();
	Console.WriteLine (result.ToString ());

Between:

    var result = this.getContext ().model("Product")
		.where("category").equal("Laptops")
		.or("category").equal("Desktops")
		.andAlso("price").between(200,750)
		.orderBy("price")
		.take(5)
		.getItems ();
	Console.WriteLine (result.ToString ());

#### Aggregate Functions

Count:

    var result = this.getContext ().model("Product")
		.select("category", "count(id) as total")
		.groupBy("category")
		.orderByDescending("count(id)")
		.getItems ();
	Console.WriteLine (result.ToString ());

Min:

    var result = this.getContext ().model("Product")
		.select("category", "min(price) as minimumPrice")
		.where("category").equal("Laptops")
		.or("category").equal("Desktops")
		.groupBy("category")
		.orderByDescending("min(price)")
		.getItems ();
	Console.WriteLine (result.ToString ());

Max:

    var result = this.getContext ().model("Product")
		.select("category", "max(price) as maximumPrice")
		.where("category").equal("Laptops")
		.getItem ();
	Console.WriteLine (result.ToString ());

### String Functions:

Index Of:

    var result = this.getContext ().model("Product")
		.where("name").indexOf("Intel")
		.greaterOrEqual(0)
		.take (10)
		.getItems ();
	Console.WriteLine (result.ToString ());

Substring:

    var result = this.getContext ().model("Product")
		.where("name").substr(6,4)
		.equal("Core")
		.getItems ();
	Console.WriteLine (result.ToString ());

Starts with:

    var result = this.getContext ()
		.model ("Product").where("name")
		.startsWith("Apple")
		.equal(true)
		.getItems ();
	Console.WriteLine (result.ToString ());

Ends with:

    var result = this.getContext ()
		.model ("Product").where("name")
		.endsWith("(7-Inch)")
		.equal(true)
		.getItems ();
	Console.WriteLine (result.ToString ());

Lower case:

    var result = this.getContext ().model ("Product")
		.where("category").toLowerCase()
		.equal("laptops")
		.getItems ();
	Console.WriteLine (result.ToString ());

Upper case:

    var result = this.getContext ().model ("Product")
		.where("category").toUpperCase()
		.equal("LAPTOPS")
		.getItems ();
	Console.WriteLine (result.ToString ());

#### Date Functions:

Date:

    var result = this.getContext().model("Order")
		.where("orderDate").getDate()
		.equal("2015-04-18")
		.getItems ();
	Console.WriteLine (result.ToString ());

Month:

    var result = this.getContext().model("Order")
		.where("orderDate").getMonth()
		.equal(4)
		.orderByDescending("orderDate")
		.getItems ();
	Console.WriteLine (result.ToString ());

Day:

    var result = this.getContext().model("Order")
		.where("orderDate").getMonth().equal(4)
		.and("orderDate").getDay().lowerThan(15)
		.orderByDescending("orderDate")
		.getItems ();
	Console.WriteLine (result.ToString ());

Year:

    var result = this.getContext().model("Order")
		.where("orderDate").getMonth().equal(5)
		.and("orderDate").getDay().lowerOrEqual(10)
		.and("orderDate").getFullYear().equal(2015)
		.getItems ();
	Console.WriteLine (result.ToString ());

Hours:

    var result = this.getContext().model("Order")
		.where("orderDate").getMonth().equal(5)
		.and("orderDate").getDay().lowerOrEqual(10)
		.and("orderDate").getHours().between(10,18)
		.getItems ();
	Console.WriteLine (result.ToString ());

Minutes:

    var result = this.getContext().model("Order")
		.where("orderDate").getMonth().equal(5)
		.and("orderDate").getHours().between(9,17)
		.and("orderDate").getMinutes().between(1,30)
		.getItems ();
	Console.WriteLine (result.ToString ());

Seconds:

    var result = this.getContext().model("Order")
		.where("orderDate").getMonth().equal(5)
		.and("orderDate").getHours().between(9,17)
		.and("orderDate").getMinutes().between(1,30)
		.and("orderDate").getSeconds().between(1,45)
		.getItems ();
	Console.WriteLine (result.ToString ());

#### Math Functions

Round:

    var result = this.getContext().model("Product")
		.where("price").round().lowerOrEqual(177)
		.getItems ();
	Console.WriteLine (result.ToString ());

Floor:

    var result = this.getContext().model("Product")
		.where("price").floor().lowerOrEqual(177)
		.getItems ();
	Console.WriteLine (result.ToString ());

Ceiling:

    var result = this.getContext().model("Product")
		.where("price").ceil().lowerOrEqual(177)
		.getItems ();
	Console.WriteLine (result.ToString ());

#### Methods

##### and(name)

Prepares a logical AND expression.

Parameters:
- name: The name of field that is going to be used in this expression

##### andAlso(name)

Prepares a logical AND expression.
If an expression is already defined, it will be wrapped with the new AND expression

Parameters:
- name: The name of field that is going to be used in this expression

   	var result = this.getContext ().model ("Product")
		.where ("category").equal ("Laptops")
		.or ("category").equal ("Desktops")
		.andAlso ("price").greaterOrEqual (177)
		.getItems ();
	Console.WriteLine (result.ToString ());

##### expand(...attr)

Parameters:
- attr: A param array of strings which represents the field or the array of fields that are going to be expanded.
If attr is missing then all the previously defined expandable fields will be removed

Defines an attribute or an array of attributes to be expanded in the final result. This operation should be used
when a non-expandable attribute is required to be expanded in the final result.

    var result = this.getContext().model("Order")
			.where("customer").equal(337)
			.orderByDescending("orderDate")
			.expand("customer")
			.getItems ();
	Console.WriteLine (result.ToString ());

##### first()

Executes the specified query and returns the first item.

    var result = this.getContext().model("User")
		.where("name").equal("alexis.rees@example.com")
		.first();
	Console.WriteLine (result.ToString ());

##### getItem()

Executes the specified query and returns the first item.

    var result = this.getContext().model("User")
        .where("name").equal("alexis.rees@example.com").getItem();
    Console.WriteLine (result.ToString ());

##### getItems()

Executes the specified query and returns an array of items.

    var result = this.getContext().model("Product")
        .where("category").equal("Laptops")
        .take(10)
        .getItems();
    Console.WriteLine (result.ToString ());

##### getList()

Executes the underlying query and returns a result set based on the specified paging parameters.

    var result = this.getContext ().model ("Product")
		.where ("category").equal ("Laptops")
		.skip (5)
		.take (5)
		.getList ();
	Console.WriteLine (result.total);
	Console.WriteLine (result.ToString ());

##### skip(val)

Prepares a paging operation by skipping the specified number of records

Parameters:
- val: The number of records to be skipped

	    var result =this.getContext()
				.model ("Order")
				.skip(10)
				.take(10)
				.getItems ();
		Console.WriteLine (result.ToString ());

##### take(val)

Prepares a data paging operation by taking the specified number of records

Parameters:
- val: The number of records to take

	    var result =this.getContext()
				.model ("Order")
				.take(10)
				.getItems ();
		Console.WriteLine (result.ToString ());

##### groupBy(...attr)

Prepares a group by expression

    var result = this.getContext ()
		.model ("Product").orderBy ("name")
		.select ("category", "count(id) as totalCount")
		.groupBy("category")
		.orderBy("category")
		.getItems ();
	Console.WriteLine (result.ToString ());

##### orderBy(...attr)

Prepares an ascending sorting operation

    var result = this.getContext ()
		.model ("Product").where("category").equal("Laptops")
		.and("price").lowerThan(500)
		.orderBy("price")
		.getItems ();
	Console.WriteLine (result.ToString ());

##### thenBy(...attr)

 Continues a descending sorting operation

    var result = this.getContext ()
		.model ("Product").where("category").equal("Laptops")
		.and("price").lowerThan(500)
		.orderBy("category")
		.thenBy("name")
		.getItems ();
	Console.WriteLine (result.ToString ());

##### orderByDescending(...attr)

 Prepares an descending sorting operation

    var result = this.getContext ()
		.model ("Product").where("category").equal("Laptops")
		.and("price").lowerThan(500)
		.orderByDescending("price")
		.getItems ();
	Console.WriteLine (result.ToString ());

##### thenByDescending(...attr)

 Continues a descending sorting operation

    var result = this.getContext ()
		.model ("Product").where("category").equal("Laptops")
		.and("price").lowerThan(500)
		.orderByDescending("category")
		.thenByDescending("name")
		.getItems ();
	Console.WriteLine (result.ToString ());
