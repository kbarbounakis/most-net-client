using System;

namespace Most.Client
{
	public interface IClientDataService 
	{
		IClientDataService authenticate(string cookie);
		IClientDataService authenticate(string username, string password);
		Object execute(ServiceExecuteOptions options);
	}
}

