using System;

namespace Most.Client
{
	public interface IClientDataService 
	{
		IClientDataService Authenticate(string cookie);
		IClientDataService Authenticate(string username, string password);
		Object Execute(ServiceExecuteOptions options);
	}
}

