using System;

namespace Most.Client
{
	public class ClientDataContext
	{
		private IClientDataService service;
		public ClientDataContext (string uri)
		{
			this.service = new ClientDataService (uri);
		}

		public static ClientDataContext Create(string uri) 
		{
			return new ClientDataContext(uri);
		}

		public IClientDataService getService() {
			return this.service;
		}

		public ClientDataContext setService(IClientDataService service) {
			this.service = service;
			return this;
		}

		public ClientDataContext Authenticate(string cookie) {
			this.service.Authenticate(cookie);
			return this;
		}

		public ClientDataContext Authenticate(string username, string password) {
			this.service.Authenticate(username, password);
			return this;
		}

		public ClientDataModel Model(string name) {
			return new ClientDataModel(name, this.service);
		}


	}
}

