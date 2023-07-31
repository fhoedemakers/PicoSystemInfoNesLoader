using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PicoSystemInfoNesLoader
{
    public class GitHubBase
    {
        private string ApiToken = "";
        private static HttpClient _httpClient = null;
        protected HttpClient Client
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = new HttpClient()
                    {
                        BaseAddress = new Uri("https://api.github.com")
                    };
                    _httpClient.DefaultRequestHeaders.Accept.Clear();
                    _httpClient.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
                    _httpClient.DefaultRequestHeaders.Add("User-Agent", "PicoSystem_InfoNes");
                    if (!string.IsNullOrEmpty(ApiToken))
                    {
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", ApiToken);

                    }
                } 
                return _httpClient;
            }
        }
    }
}
