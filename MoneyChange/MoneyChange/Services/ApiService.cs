using MoneyChange.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MoneyChange.Helpers;

namespace MoneyChange
{
    public class ApiService
    {
        public async Task<Response> CheckConnection()
        {
            if(!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSucces = false,
                    Message = Lenguages.SettingInternetConnection
                };
            }

            var response = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            if(!response)
            {
                return new Response
                {
                    IsSucces = false,
                    Message = Lenguages.CheckInternetConnection
                };
            }

            return new Response
            {
                IsSucces = true
            };
        }
        public async Task<Response> GetList<T>(string urlBase, string controller)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.GetAsync(controller);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSucces = false,
                        Message = result,
                    };
                }
                var list = JsonConvert.DeserializeObject<List<T>>(result);

                return new Response
                {
                    IsSucces = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSucces = false,
                    Message = ex.Message
                };
            }
        }
    }
}
