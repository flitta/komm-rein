
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
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;

namespace kommrein.ui.web.Services
{
    
    public class HttpService : IHttpService
    {
        protected readonly IHttpClientFactory _clientFactory;
        protected HttpClient _httpClient;

        public HttpService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public void Init(string apiName)
        {
            _httpClient = _clientFactory.CreateClient(apiName);
        }
        
        public async Task<T> Get<T>(string path)
        {
            T result = default(T);
     
            try
            {
                result = await _httpClient.GetFromJsonAsync<T>(path);

            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
            }

            return result;
        }

        public async Task<T> Post<T>(string path, T value)
        {
            T result = default(T);

            try
            {
                var response = await _httpClient.PostAsJsonAsync(path, value);
                result = await response.Content.ReadFromJsonAsync<T>();

            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
            }

            return result;
        }

        public async Task<TR> Post<TR>(string path, object value)
        {
            TR result = default(TR);

            try
            {
                var response = await _httpClient.PostAsJsonAsync(path, value);
                result = await response.Content.ReadFromJsonAsync<TR>();

            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
            }
         
            return result;
        }

        public async Task<T> Put<T>(string path, T value)
        {
            T result = default(T);

            try
            {
                var response = await _httpClient.PutAsJsonAsync(path, value);
                result = await response.Content.ReadFromJsonAsync<T>();

            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
            }
                       
            return result;
        }
    }
}