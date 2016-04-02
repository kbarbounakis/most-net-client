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

		public IClientDataService getService() {
			return this.service;
		}

		public object schema() {
			var options = new ServiceExecuteOptions();
			options.Url = String.Format ("/{0}/schema.json", this.GetName ());
			return this.getService().execute(options);
		}

		public ClientDataQueryable where(string name) {
			var result = new ClientDataQueryable(this.name, this.service);
			return result.where (name);
		}

		public ClientDataQueryable select(params string[] args) {
			var result = new ClientDataQueryable(this.name, this.service);
			return result.select (args);
		}

		public ClientDataQueryable groupBy(params string[] args) {
			var result = new ClientDataQueryable(this.name, this.service);
			return result.groupBy (args);
		}

		public ClientDataQueryable expand(params string[] args) {
			var result = new ClientDataQueryable(this.name, this.service);
			return result.expand (args);
		}

		public ClientDataQueryable orderBy(string arg) {
			var result = new ClientDataQueryable(this.name, this.service);
			return result.orderBy (arg);
		}

		public ClientDataQueryable skip(int num) {
			var result = new ClientDataQueryable(this.name, this.service);
			return result.skip (num);
		}

		public ClientDataQueryable take(int num) {
			var result = new ClientDataQueryable(this.name, this.service);
			return result.take (num);
		}

		public ClientDataQueryable orderByDescending(string arg) {
			var result = new ClientDataQueryable(this.name, this.service);
			return result.orderByDescending (arg);
		}

		public object getItems() {
			var result = new ClientDataQueryable(this.name, this.service);
			return result.getItems();
		}

		public object getList() {
			var result = new ClientDataQueryable(this.name, this.service);
			return result.getList();
		}

		public object save(object data) {
			var options = new ServiceExecuteOptions () {
				Url = String.Format ("/{0}/index.json", this.GetName ()),
				Method = HttpMethod.Post,
				Data = data
			};
			return this.getService ().execute (options);
		}

		public object remove(object data) {
			var options = new ServiceExecuteOptions () {
				Url = String.Format ("/{0}/index.json", this.GetName ()),
				Method = HttpMethod.Delete,
				Data = data
			};
			return this.getService ().execute (options);
		}

	}
}

