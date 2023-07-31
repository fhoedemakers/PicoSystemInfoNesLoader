using Newtonsoft.Json.Linq;
using PicoSystemInfoNesLoader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace PicoNesLoader
{
    /// <summary>
    /// Class for accessing the GitHub API
    /// </summary>
    public class GitHub :GitHubBase
    {    
        #region properties
        public string Owner { get; }
        public string Repo { get; }  
        #endregion

        #region methods
        public async Task<JObject> GetLatestReleaseAsync()
        {
            var request = $"/repos/{Owner}/{Repo}/releases/latest";
            var str = await Client.GetStringAsync(request);
            return JObject.Parse(str);    
        }
        #endregion

        public GitHub(string owner, string repo)
        {
            Owner = owner;
            Repo = repo;
        }
    }
}
