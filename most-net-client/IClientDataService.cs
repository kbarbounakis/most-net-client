using System;

namespace Most.Client
{
	public interface IClientDataService 
	{
		IClientDataService authenticate(string cookie);
		IClientDataService authenticate(string username, string password);
		T execute<T>(ServiceExecuteOptions options);
		Object execute(ServiceExecuteOptions options);
	}
}

