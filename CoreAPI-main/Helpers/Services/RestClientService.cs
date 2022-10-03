using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApi.Helpers.Common;
using WebApi.Models;

namespace WebApi.Helpers.Utils
{
    public class RestClientService
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(RestClientService));
        public static async Task<JObject> CallRestAPI(string url, string method, object requestBody, RestHeaderRequest header = null)
        {
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(GlobalVariable.TimeoutCallFASTService)
            };
            if (header == null) header = new RestHeaderRequest();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(header.ContenType));
            if (!string.IsNullOrEmpty(header.Authentication))
            {
                string[] arrAuth = header.Authentication.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (arrAuth.Length == 2)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(arrAuth[0], arrAuth[1]);
                }
                else
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(header.Authentication);
                }
            }

            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json")
            };
            httpRequestMessage.Method = (method.ToUpper()) switch
            {
                "GET" => HttpMethod.Get,
                "PUT" => HttpMethod.Put,
                "DELETE" => HttpMethod.Delete,
                _ => HttpMethod.Post
            };
            var response = client.SendAsync(httpRequestMessage).Result;
            string reponseJsonString = await response.Content.ReadAsStringAsync();
            _logger.Information(
                    "Logging Request and response to FAST\r\nRequest with method: " + httpRequestMessage.Method +
                    " and URL: " + url +
                    "\r\nRequest Header: " + client.DefaultRequestHeaders.ToString() +
                    "\r\nRequest Body: " + ((url.Contains("token") || url.Contains("login") || url.Contains("auth"))? "Login request is secured." : JsonConvert.SerializeObject(requestBody)) +
                    "\r\nResponse with status: " + response.StatusCode +
                    "\r\nResponse Header: " + response.Headers.ToString() + "\r\nResponse Body: " + reponseJsonString);
            if (response.IsSuccessStatusCode)
            {
                var objReponse = JObject.Parse(reponseJsonString);
                return objReponse;
            }
            else
            {
                throw new HttpRequestException("Reponse error with code "+ response.StatusCode + " " + reponseJsonString);
            }          
        }
    }
}
