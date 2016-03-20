using System;
using System.Net.Http;

namespace Most.Client
{
	public class ClientDataModel
	{
		private string name;
		private IClientDataService service;
		public ClientDataModel (string name, IClientDataService service)
		{
			Args.NotEmpty(name,"Model name");
			this.name = name;
			Args.NotNull(service,"Model service");
			this.service = service;
		}

		public string GetName() {
			return this.name;
		}

		public IClientDataService GetService() {
			return this.service;
		}

		public object Schema() {
			var options = new ServiceExecuteOptions();
			options.Url = String.Format ("/{0}/schema.json", this.GetName ());
			return this.GetService().Execute(options);
		}

		public ClientDataQueryable Where(string name) {
			var result = new ClientDataQueryable(this.name, this.service);
			return result.Where (name);
		}

		public ClientDataQueryable Select(params string[] args) {
			var result = new ClientDataQueryable(this.name, this.service);
			return result.Select (args);
		}

		public ClientDataQueryable GroupBy(params string[] args) {
			var result = new ClientDataQueryable(this.name, this.service);
			return result.GroupBy (args);
		}

		public ClientDataQueryable Expand(params string[] args) {
			var result = new ClientDataQueryable(this.name, this.service);
			return result.Expand (args);
		}

		public ClientDataQueryable OrderBy(string arg) {
			var result = new ClientDataQueryable(this.name, this.service);
			return result.OrderBy (arg);
		}

		public ClientDataQueryable OrderByDescending(string arg) {
			var result = new ClientDataQueryable(this.name, this.service);
			return result.OrderByDescending (arg);
		}

		public object GetItems() {
			var result = new ClientDataQueryable(this.name, this.service);
			return result.GetItems();
		}

		public object GetList() {
			var result = new ClientDataQueryable(this.name, this.service);
			return result.GetList();
		}

		public object Save(object data) {
			var options = new ServiceExecuteOptions () {
				Url = String.Format ("/{0}/index.json", this.GetName ()),
				Method = HttpMethod.Post,
				Data = data
			};
			return this.GetService ().Execute (options);
		}

		public object Remove(object data) {
			var options = new ServiceExecuteOptions () {
				Url = String.Format ("/{0}/index.json", this.GetName ()),
				Method = HttpMethod.Delete,
				Data = data
			};
			return this.GetService ().Execute (options);
		}

	}
}

