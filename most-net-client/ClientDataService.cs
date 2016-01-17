using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace Most.Client {
	
	public class ClientDataService
	{
		private string uri;
		private string cookie;

		public ClientDataService (string uri)
		{
			//ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

			if (String.IsNullOrEmpty (uri)) {
				throw new ArgumentNullException ("uri", "Uri parameter cannot be null or empty.");
			}
			if (!Uri.IsWellFormedUriString (uri, UriKind.Absolute)) {
				throw new InvalidCastException ("The specified uri string is not well formed. An absolute uri was expected.");
			}
			this.uri = uri;
		}

		private static bool RemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
			return true;
		}
		/// <summary>
		/// Prepares an authenticated request by adding a cookie value which is going to be used as HTTP request header.
		/// </summary>
		/// <param name="cookie">A string which represents the HTTP Cookie header</param>
		public ClientDataService authenticate(string cookie) {
			this.cookie = cookie;
			return this;
		}
		/// <summary>
		/// Prepares an authenticated request by passing remote user credentials.
		/// </summary>
		/// <param name="username">A string which represents the user name.</param>
		/// <param name="password">A string which represents the user password.</param>
		public ClientDataService authenticate(string username, string password) {
			try {
				//create request uri
				Uri requestUri;
				Uri.TryCreate( new Uri(this.uri), "/User/index.json?$filter=id eq me()", out requestUri);
				//init request
				HttpWebRequest req = HttpWebRequest.CreateHttp(requestUri.AbsoluteUri);
				req.KeepAlive = true;
				req.AllowAutoRedirect = true;
				req.Method = "GET";
				//init authorization header
				string a = String.Concat(username, ":", password);
				req.Headers.Add("Authorization","Basic " + Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(a)));
				HttpWebResponse response = (HttpWebResponse)req.GetResponse();
				//if response status code is 200
				if (response.StatusCode == HttpStatusCode.OK) {
					//get authentication cookie value
					this.cookie = response.Headers.Get("Set-Cookie");
					//close response
					response.Close();
				}
				//if response status code is 401 or 403
				else if ((response.StatusCode == HttpStatusCode.Unauthorized) ||
					(response.StatusCode == HttpStatusCode.Forbidden)) {
					throw new UnauthorizedAccessException();
				}
				else {
					throw new HttpListenerException((int)response.StatusCode, response.StatusDescription);
				}
				return this;
			} catch (Exception ex) {
				throw ex;
			}
		}
		/// <summary>
		/// Executes an HTTP GET request based on the given relative URI.
		/// </summary>
		/// <returns>An object which represents the response of the executed HTTP request.</returns>
		/// <param name="relativeUri">A string which represents a relative URI.</param>
		/// <param name="query">A collection of query parameters.</param>
		public Object get(string relativeUri, IDictionary<string,Object> query) {
			//create request uri
			Uri requestUri;
			Object result;
			Uri.TryCreate( new Uri(this.uri), relativeUri, out requestUri);
			if (requestUri == null) {
				throw new InvalidCastException("The given relative URI is not well formed.");
			}
			if ((query != null) && (query.Count>0)) {
				//todo::add query
			}
			//string s = string.Join("&", data.Select((x) => x.Key + "=" + x.Value));
			//init request
			HttpWebRequest req = HttpWebRequest.CreateHttp(requestUri.AbsoluteUri);
			req.KeepAlive = true;
			req.AllowAutoRedirect = true;
			req.Method = "GET";
			req.ContentType = "application/json";
			//set authentication cookie
			if (!String.IsNullOrEmpty (this.cookie)) {
				req.Headers.Add("Cookie",this.cookie);
			}
			HttpWebResponse response = (HttpWebResponse)req.GetResponse();
			//if response status code is 200
			if (response.StatusCode == HttpStatusCode.OK) {
				Newtonsoft.Json.JsonSerializer sr = new JsonSerializer ();
				StreamReader reader = new StreamReader (response.GetResponseStream ());
				using (var textReader = new JsonTextReader(reader))
				{
					result = sr.Deserialize (textReader);
				}
				//close response
				response.Close();
			}
			//if response status code is 401 or 403
			else if ((response.StatusCode == HttpStatusCode.Unauthorized) ||
				(response.StatusCode == HttpStatusCode.Forbidden)) {
				throw new UnauthorizedAccessException();
			}
			else {
				throw new HttpListenerException((int)response.StatusCode, response.StatusDescription);
			}
			return result;
		}

		public Object post(string relativeUri) {
			return null;
		}

		public ClientDataQueryable model(string model) {
			return new ClientDataQueryable (model);
		}

	}



}

