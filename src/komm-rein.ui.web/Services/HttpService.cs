
using kommrein.ui.web.Config;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace kommrein.ui.web.Services
{
    
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        
        public HttpService(IHttpClientFactory clientFactory
        )
        {
            _httpClient = clientFactory.CreateClient(ApiConfig.API_NAME);
        }

        public async Task<T> Get<T>(string path)
        {
            var result = await _httpClient.GetFromJsonAsync<T>(path);
            return result;
        }

        public async Task<T> Post<T>(string path, T value)
        {
            var response = await _httpClient.PostAsJsonAsync(path, value);
            T result = await response.Content.ReadFromJsonAsync<T>();
            return result;
        }

        public async Task<TR> Post<TR>(string path, object value)
        {
            var response = await _httpClient.PostAsJsonAsync(path, value);
            TR result = await response.Content.ReadFromJsonAsync<TR>();
            return result;
        }

        public async Task<T> Put<T>(string path, T value)
        {
            var response = await _httpClient.PutAsJsonAsync(path, value);
            T result = await response.Content.ReadFromJsonAsync<T>();
            return result;
        }
    }
}