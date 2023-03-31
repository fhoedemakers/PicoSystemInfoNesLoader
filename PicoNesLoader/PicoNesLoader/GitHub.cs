﻿using Newtonsoft.Json.Linq;
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
    public class GitHub
    {
        static HttpClient _httpClient = null;
        private string ApiToken = "";
        private HttpClient Client
        {
            get { 
                if ( _httpClient == null)
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
                        _httpClient.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("token", ApiToken);
                    
                    }
                }   
                
                return _httpClient; 
            }

        }

        public async Task<JObject> GetLatestReleaseAsync()
        {
            var request = $"/repos/{Owner}/{Repo}/releases/latest";
            var str = await Client.GetStringAsync(request);
            return JObject.Parse(str);    
        }

       
        public string Owner { get; }
        public string Repo { get; }

        public GitHub(string owner, string repo)
        {
            Owner = owner;
            Repo = repo;
        }
    }
}