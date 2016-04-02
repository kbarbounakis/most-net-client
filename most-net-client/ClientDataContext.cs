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

		public static ClientDataContext create(string uri) 
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

		public ClientDataContext authenticate(string cookie) {
			this.service.authenticate(cookie);
			return this;
		}

		public ClientDataContext authenticate(string username, string password) {
			this.service.authenticate(username, password);
			return this;
		}

		public ClientDataModel model(string name) {
			return new ClientDataModel(name, this.service);
		}


	}
}

