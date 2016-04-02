using System;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Most.Client
{
	public class ClientDataQueryable
	{
		

		private class QueryExpression {

			public QueryExpression() {
				
			}

			public QueryExpression(string left) {
				this.left = left;
			}

			public string left {
				get;
				set;
			}
			public string op {
				get;
				set;
			}
			public object right {
				get;
				set;
			}


		}

		private string model;
		private IClientDataService service;
		private QueryExpression expression;
		private QueryOptions options;
		private string lop;
		private string url;
		private string lastop;

		public ClientDataQueryable (string model, IClientDataService service)
		{
			Args.NotNull (model, "The target model");
			this.model = model;
			this.service = service;
			this.url = "/" + model + "/index.json";
			this.expression = new QueryExpression ();
			this.options = new QueryOptions ();
		}

		public string getModel() {
			return this.model;
		}

		public IClientDataService getService() {
			return this.service;
		}

		public string getUrl() {
			return this.url;
		}

		public ClientDataQueryable setUrl(string value) {
			Args.NotEmpty(value,"Model URL");
			this.url = value;
			return this;
		}

		public ClientDataQueryable where(string name) {
			Args.NotNull(name,"Name");
			this.expression = new QueryExpression (name);
			return this;
		}

		public ClientDataQueryable and(string name) {
			Args.NotNull(name,"Name");
			this.lop = "and";
			this.expression = new QueryExpression (name);
			return this;
		}

		public ClientDataQueryable andAlso(string name) {
			Args.NotNull(name,"Name");
			if (this.options.filter != null) {
				this.options.filter = "(" + this.options.filter + ")";
			}
			this.lop = "and";
			this.expression = new QueryExpression (name);
			return this;
		}

		public ClientDataQueryable orElse(string name) {
			Args.NotNull(name,"Name");
			if (this.options.filter != null) {
				this.options.filter = "(" + this.options.filter + ")";
			}
			this.lop = "or";
			this.expression = new QueryExpression (name);
			return this;
		}

		public ClientDataQueryable or(string name) {
			Args.NotNull(name,"Name");
			this.lop = "or";
			this.expression = new QueryExpression (name);
			return this;
		}

		private ClientDataQueryable append() {
			Args.NotNull (this.expression, "Query Expression");
			var expr = "";
			if (this.expression.left != null) {
				if (this.expression.op == "in") {
					var exprs = new List<String> ();
					foreach (var right in (Object[])this.expression.right) {
						exprs.Add (this.expression.left + " eq " + ClientDataQueryable.escape (right));
					}
					expr = '(' + String.Join (" or ", exprs) + ')';
				} else if (this.expression.op == "nin") {
					var exprs = new List<String> ();
					foreach (var right in (Object[])this.expression.right) {
						exprs.Add (this.expression.left + " ne " + ClientDataQueryable.escape (right));
					}
					expr = '(' + String.Join (" and ", exprs) + ')';
				} else {
					expr = this.expression.left + " " + this.expression.op + " " + ClientDataQueryable.escape (this.expression.right);
				}

				if (this.options.filter == null) {
					this.options.filter = expr;
				} else {
					if (this.lop == null) {
						this.lop = "and";
					}
					if (this.lastop == null) {
						this.lastop = this.lop;
					}
					if (this.lop == this.lastop) {
						this.options.filter = this.options.filter + " " + this.lop + " " + expr;
					} else {
						this.options.filter = "(" + this.options.filter + ") " + this.lop + " " + expr;
					}
					this.lastop = this.lop;
				}

			}
			this.expression = new QueryExpression ();
			this.lop = null;
			return this;
		}

		public ClientDataQueryable filter(string filter) {
			this.options.filter = filter;
			return this;
		}

		public ClientDataQueryable equal(object value) {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.op = "eq";
			this.expression.right = value;
			return this.append();
		}

		public ClientDataQueryable between(object value1, object value2) {
			Args.NotNull(this.expression.left,"Left operand");
			String s = (new ClientDataQueryable (this.getModel (), null))
				.where (this.expression.left).greaterOrEqual (value1)
				.and (this.expression.left).lowerOrEqual (value2).options.filter;
			if (this.lop == null) {
				this.lop = "and";
			}
			if (String.IsNullOrEmpty (this.options.filter)) {
				this.options.filter = "(" + s + ")";
			} else {
				this.options.filter = "(" + this.options.filter + ") " + this.lop + " (" + s + ")";
			}
			this.expression = new QueryExpression ();
			this.lop = null;
			return this;
		}



		public ClientDataQueryable notEqual(object value) {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.op = "ne";
			this.expression.right = value;
			return this.append();
		}

		public ClientDataQueryable greaterThan(object value) {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.op = "gt";
			this.expression.right = value;
			return this.append();
		}

		public ClientDataQueryable greaterOrEqual(object value) {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.op = "ge";
			this.expression.right = value;
			return this.append();
		}

		public ClientDataQueryable lowerThan(object value) {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.op = "lt";
			this.expression.right = value;
			return this.append();
		}

		public ClientDataQueryable lowerOrEqual(object value) {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.op = "le";
			this.expression.right = value;
			return this.append();
		}

		public ClientDataQueryable @in(params object[] values) {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.op = "in";
			this.expression.right = values;
			return this.append();
		}

		public ClientDataQueryable notIn(object[] values) {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.op = "nin";
			this.expression.right = values;
			return this.append();
		}

		public ClientDataQueryable startsWith(object value) {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.left = String.Format("startswith({0},{1})", this.expression.left,ClientDataQueryable.escape (value));
			return this;
		}

		public ClientDataQueryable indexOf(object value) {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.left = String.Format("indexof({0},{1})", this.expression.left,ClientDataQueryable.escape (value));
			return this;
		}

		public ClientDataQueryable endsWith(object value) {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.left = String.Format("endswith({0},{1})", this.expression.left,ClientDataQueryable.escape (value));
			return this;
		}

		public ClientDataQueryable substr(int pos, int length) {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.left = String.Format("substring({0},{1},{2})", this.expression.left, pos, length);
			return this;
		}

		public ClientDataQueryable contains(object value) {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.op = "ge";
			this.expression.left = String.Format("indexof({0},{1})", this.expression.left,ClientDataQueryable.escape (value));
			this.expression.right = 0;
			return this.append();
		}

		public ClientDataQueryable select(params string[] args) {
			if (args.Length > 0)
				this.options.select = String.Join (",", args);
			else
				this.options.select = null;
			return this;
		}

		public ClientDataQueryable expand(params string[] args) {
			if (args.Length > 0)
				this.options.expand = String.Join (",", args);
			else
				this.options.expand = null;
			return this;
		}

		public ClientDataQueryable groupBy(params string[] args) {
			if (args.Length > 0)
				this.options.group = String.Join (",", args);
			else
				this.options.group = null;
			return this;
		}

		public ClientDataQueryable orderBy(string arg) {
			Args.NotEmpty(arg,"Order operand");
			this.options.order = arg;
			return this;
		}

		public ClientDataQueryable thenBy(string arg) {
			Args.NotEmpty(arg,"Order operand");
			if (this.options.order != null) {
				this.options.order += "," + arg;
			} else {
				return this.orderBy (arg);
			}
			return this;
		}

		public ClientDataQueryable orderByDescending(string arg) {
			Args.NotEmpty(arg,"Order operand");
			this.options.order = arg + " desc";
			return this;
		}

		public ClientDataQueryable thenByDescending(string arg) {
			Args.NotEmpty(arg,"Order operand");
			if (this.options.order != null) {
				this.options.order += "," + arg + " desc";
			} else {
				return this.orderByDescending (arg);
			}
			return this;
		}

		public ClientDataQueryable take(int num) {
			Args.NotNegative(num,"Page size");
			this.options.first = null;
			this.options.top = num;
			return this;
		}

		public object all() {
			this.options.top = -1;
			this.options.skip = null;
			this.options.inlinecount = null;
			this.options.first = null;
			return this.getItems();
		}

		public ClientDataQueryable skip(int num) {
			Args.NotNegative(num,"Skip records");
			this.options.first = null;
			this.options.skip = num;
			return this;
		}

		public object first() {
			return this.getItem ();
		}



		public object getItem() {
			this.options.top = null;
			this.options.skip = null;
			this.options.inlinecount = null;
			this.options.first = true;
			var options = new ServiceExecuteOptions () {
				Url = this.getUrl (),
				Query = this.options.ToNameValueCollection ()
			};
			return this.getService ().execute (options);
		}

		public object item() {
			return this.getItem ();
		}

		public object getItems() {
			this.options.inlinecount = null;
			var options = new ServiceExecuteOptions () {
				Url = this.getUrl (),
				Query = this.options.ToNameValueCollection ()
			};
			return this.getService ().execute (options);
		}

		public object items() {
			return this.getItems ();
		}

		public object getList() {
			this.options.inlinecount = true;
			var options = new ServiceExecuteOptions () {
				Url = this.getUrl (),
				Query = this.options.ToNameValueCollection ()
			};
			return this.getService ().execute (options);
		}

		public object list() {
			return this.getList ();
		}

		public ClientDataQueryable toLowerCase() {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.left = String.Format("tolower({0})",this.expression.left);
			return this;
		}

		public ClientDataQueryable toLocaleLowerCase() {
			return this.toLowerCase ();
		}

		public ClientDataQueryable toLocaleUpperCase() {
			return this.toUpperCase ();
		}

		public ClientDataQueryable toUpperCase() {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.left = String.Format("toupper({0})",this.expression.left);
			return this;
		}

		public ClientDataQueryable getDate() {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.left = String.Format("date({0})",this.expression.left);
			return this;
		}

		public ClientDataQueryable getDay() {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.left = String.Format("day({0})",this.expression.left);
			return this;
		}

		public ClientDataQueryable getMonth() {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.left = String.Format("month({0})",this.expression.left);
			return this;
		}

		public ClientDataQueryable getYear() {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.left = String.Format("year({0})",this.expression.left);
			return this;
		}

		public ClientDataQueryable getFullYear() {
			return this.getYear ();
		}

		public ClientDataQueryable getHours() {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.left = String.Format("hour({0})",this.expression.left);
			return this;
		}

		public ClientDataQueryable getMinutes() {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.left = String.Format("minute({0})",this.expression.left);
			return this;
		}

		public ClientDataQueryable getSeconds() {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.left = String.Format("second({0})",this.expression.left);
			return this;
		}

		public ClientDataQueryable floor() {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.left = String.Format("floor({0})",this.expression.left);
			return this;
		}

		public ClientDataQueryable ceil() {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.left = String.Format("ceiling({0})",this.expression.left);
			return this;
		}

		public ClientDataQueryable round() {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.left = String.Format("round({0})",this.expression.left);
			return this;
		}

		public ClientDataQueryable length() {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.left = String.Format("length({0})",this.expression.left);
			return this;
		}

		public ClientDataQueryable trim() {
			Args.NotNull(this.expression.left,"Left operand");
			this.expression.left = String.Format("trim({0})",this.expression.left);
			return this;
		}

		public ClientDataQueryable concat(params string[] arg) {
			Args.NotNull(this.expression.left,"Left operand");
			var s =  "concat(" + this.expression.left;
			foreach (var item in arg) {
				s += "," + ClientDataQueryable.escape (item);
			}
			this.expression.left = s + ")";
			return this;
		}

		internal static string escape(object value) {
			return ClientDataQueryable.escape (value, false);
		}

		internal static string escape(object value, bool unquoted) {
			return System.Text.RegularExpressions.Regex.Replace (JsonConvert.SerializeObject (value), "^\"|\"$", unquoted ? "" : "'");
		}

	}
}

