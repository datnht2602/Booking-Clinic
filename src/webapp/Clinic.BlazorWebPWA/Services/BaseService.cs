using System.Net.Http.Headers;
using System.Text;
using Clinic.BlazorWebPWA.Extensions;
using Clinic.BlazorWebPWA.Services.IService;
using Clinic.DTO.Models.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Newtonsoft.Json;

namespace Clinic.BlazorWebPWA.Services;

 public class BaseService : ComponentBase,IBaseService
    {
        
        public ResponseDto responseModel { get; set; }
        private readonly IHttpClientFactory httpClient;
  
        
        protected BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new ResponseDto();
            this.httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient(name:"ApiGateway");
                HttpRequestMessage message = new (apiRequest.ApiType.ConvertMethod(),requestUri: apiRequest.Url);
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8,"application/json");
                }

                if (!string.IsNullOrEmpty(apiRequest.AccessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);
                }
                
                HttpResponseMessage apiResponse = null;

                apiResponse = await client.SendAsync(message);
        
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
                return apiResponseDto;

            }
            catch(Exception e)
            {
                var dto = new ResponseDto
                {
                    DisplayMessage = "Error",
                    ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var apiResponseDto = JsonConvert.DeserializeObject<T>(res);
                return apiResponseDto;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }