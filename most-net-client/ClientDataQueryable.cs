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
				this.Left = left;
			}

			public string Left {
				get;
				set;
			}
			public string Operator {
				get;
				set;
			}
			public object Right {
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

		public string GetModel() {
			return this.model;
		}

		public IClientDataService GetService() {
			return this.service;
		}

		public string GetUrl() {
			return this.url;
		}

		public ClientDataQueryable SetUrl(string value) {
			Args.NotEmpty(value,"Model URL");
			this.url = value;
			return this;
		}

		public ClientDataQueryable Where(string name) {
			Args.NotNull(name,"Name");
			this.expression = new QueryExpression (name);
			return this;
		}

		public ClientDataQueryable And(string name) {
			Args.NotNull(name,"Name");
			this.lop = "and";
			this.expression = new QueryExpression (name);
			return this;
		}

		public ClientDataQueryable AndAlso(string name) {
			Args.NotNull(name,"Name");
			if (this.options.filter != null) {
				this.options.filter = "(" + this.options.filter + ")";
			}
			this.lop = "and";
			this.expression = new QueryExpression (name);
			return this;
		}

		public ClientDataQueryable OrElse(string name) {
			Args.NotNull(name,"Name");
			if (this.options.filter != null) {
				this.options.filter = "(" + this.options.filter + ")";
			}
			this.lop = "or";
			this.expression = new QueryExpression (name);
			return this;
		}

		public ClientDataQueryable Or(string name) {
			Args.NotNull(name,"Name");
			this.lop = "or";
			this.expression = new QueryExpression (name);
			return this;
		}

		private ClientDataQueryable Append() {
			Args.NotNull (this.expression, "Query Expression");
			var expr = "";
			if (this.expression.Left != null) {
				if (this.expression.Operator == "in") {
					var exprs = new List<String> ();
					foreach (var right in (Object[])this.expression.Right) {
						exprs.Add (this.expression.Left + " eq " + ClientDataQueryable.Escape (right));
					}
					expr = '(' + String.Join (" or ", exprs) + ')';
				} else if (this.expression.Operator == "nin") {
					var exprs = new List<String> ();
					foreach (var right in (Object[])this.expression.Right) {
						exprs.Add (this.expression.Left + " ne " + ClientDataQueryable.Escape (right));
					}
					expr = '(' + String.Join (" and ", exprs) + ')';
				} else {
					expr = this.expression.Left + " " + this.expression.Operator + " " + ClientDataQueryable.Escape (this.expression.Right);
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

		public ClientDataQueryable Filter(string filter) {
			this.options.filter = filter;
			return this;
		}

		public ClientDataQueryable Equal(object value) {
			Args.NotNull(this.expression.Left,"Left operand");
			this.expression.Operator = "eq";
			this.expression.Right = value;
			return this.Append();
		}



		public ClientDataQueryable NotEqual(object value) {
			Args.NotNull(this.expression.Left,"Left operand");
			this.expression.Operator = "ne";
			this.expression.Right = value;
			return this.Append();
		}

		public ClientDataQueryable GreaterThan(object value) {
			Args.NotNull(this.expression.Left,"Left operand");
			this.expression.Operator = "gt";
			this.expression.Right = value;
			return this.Append();
		}

		public ClientDataQueryable GreaterOrEqual(object value) {
			Args.NotNull(this.expression.Left,"Left operand");
			this.expression.Operator = "ge";
			this.expression.Right = value;
			return this.Append();
		}

		public ClientDataQueryable LowerThan(object value) {
			Args.NotNull(this.expression.Left,"Left operand");
			this.expression.Operator = "lt";
			this.expression.Right = value;
			return this.Append();
		}

		public ClientDataQueryable LowerOrEqual(object value) {
			Args.NotNull(this.expression.Left,"Left operand");
			this.expression.Operator = "le";
			this.expression.Right = value;
			return this.Append();
		}

		public ClientDataQueryable In(object[] values) {
			Args.NotNull(this.expression.Left,"Left operand");
			this.expression.Operator = "in";
			this.expression.Right = values;
			return this.Append();
		}

		public ClientDataQueryable NotIn(object[] values) {
			Args.NotNull(this.expression.Left,"Left operand");
			this.expression.Operator = "nin";
			this.expression.Right = values;
			return this.Append();
		}

		public ClientDataQueryable Select(params string[] args) {
			if (args.Length > 0)
				this.options.select = String.Join (",", args);
			else
				this.options.select = null;
			return this;
		}

		public ClientDataQueryable Expand(params string[] args) {
			if (args.Length > 0)
				this.options.expand = String.Join (",", args);
			else
				this.options.expand = null;
			return this;
		}

		public ClientDataQueryable GroupBy(params string[] args) {
			if (args.Length > 0)
				this.options.group = String.Join (",", args);
			else
				this.options.group = null;
			return this;
		}

		public ClientDataQueryable OrderBy(string arg) {
			Args.NotEmpty(arg,"Order operand");
			this.options.order = arg;
			return this;
		}

		public ClientDataQueryable ThenBy(string arg) {
			Args.NotEmpty(arg,"Order operand");
			if (this.options.order != null) {
				this.options.order += "," + arg;
			} else {
				return this.OrderBy (arg);
			}
			return this;
		}

		public ClientDataQueryable OrderByDescending(string arg) {
			Args.NotEmpty(arg,"Order operand");
			this.options.order = arg + " desc";
			return this;
		}

		public ClientDataQueryable ThenByDescending(string arg) {
			Args.NotEmpty(arg,"Order operand");
			if (this.options.order != null) {
				this.options.order += "," + arg + " desc";
			} else {
				return this.OrderByDescending (arg);
			}
			return this;
		}

		public ClientDataQueryable Take(int num) {
			Args.NotNegative(num,"Page size");
			this.options.first = null;
			this.options.top = num;
			return this;
		}

		public ClientDataQueryable All() {
			this.options.top = -1;
			this.options.skip = null;
			this.options.inlinecount = null;
			this.options.first = null;
			return this;
		}

		public ClientDataQueryable Skip(int num) {
			Args.NotNegative(num,"Skip records");
			this.options.first = null;
			this.options.skip = num;
			return this;
		}

		public ClientDataQueryable First() {
			this.options.top = null;
			this.options.skip = null;
			this.options.inlinecount = null;
			this.options.first = true;
			return this;
		}

		public object GetItem() {
			var options = new ServiceExecuteOptions () {
				Url = this.GetUrl (),
				Query = this.options.ToNameValueCollection ()
			};
			return this.First ().GetService ().Execute (options);
		}

		public object GetItems() {
			this.options.inlinecount = null;
			var options = new ServiceExecuteOptions () {
				Url = this.GetUrl (),
				Query = this.options.ToNameValueCollection ()
			};
			return this.GetService ().Execute (options);
		}

		public object GetList() {
			this.options.inlinecount = true;
			var options = new ServiceExecuteOptions () {
				Url = this.GetUrl (),
				Query = this.options.ToNameValueCollection ()
			};
			return this.GetService ().Execute (options);
		}

		internal static string Escape(object value) {
			return ClientDataQueryable.Escape (value, false);
		}

		internal static string Escape(object value, bool unquoted) {
			return System.Text.RegularExpressions.Regex.Replace (JsonConvert.SerializeObject (value), "^\"|\"$", unquoted ? "" : "'");
		}

	}
}

