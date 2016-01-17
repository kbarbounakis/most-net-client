using System;

namespace Most.Client
{
	public class ClientDataQueryable
	{
		private string model;

		public ClientDataQueryable (string model)
		{
			if (String.IsNullOrEmpty (model)) {
				throw new ArgumentNullException ("model", "The target model cannot be null or empty.");
			}
		}
	}
}

