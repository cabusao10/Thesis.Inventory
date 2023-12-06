using AspNetCoreHero.Results;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.MobileApp.Utility;

namespace Thesis.Inventory.MobileApp.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<Result<T>> PostAsync<T>(this HttpClient client, string endpoint, object data)
        {
            var settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };
            using StringContent jsonContent = new(
                     JsonConvert.SerializeObject(data, settings),
                Encoding.UTF8,
                "application/json");


            var test = JsonConvert.SerializeObject(data, settings);
            var result = await client.PostAsync(endpoint, jsonContent);
            var content = await result.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<Result<T>>(content);

            return (Result<T>)Convert.ChangeType(response, typeof(Result<T>));
        }
        public static async Task<Result<T>> PatchAsync<T>(this HttpClient client, string endpoint, object data)
        {
            var settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };
            using StringContent jsonContent = new(
                     JsonConvert.SerializeObject(data, settings),
                Encoding.UTF8,
                "application/json");


            var test = JsonConvert.SerializeObject(data, settings);
            var result = await client.PatchAsync(endpoint, jsonContent);
            var content = await result.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<Result<T>>(content);

            return (Result<T>)Convert.ChangeType(response, typeof(Result<T>));
        }
        public static async Task<Result<T>> DeleteAsync<T>(this HttpClient client, string endpoint, object data)
        {
            var result = await client.DeleteAsync(endpoint);
            var content = await result.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<Result<T>>(content);

            return (Result<T>)Convert.ChangeType(response, typeof(Result<T>));
        }

        public static async Task<Result<T>> GetAsync<T>(this HttpClient client, string endpoint)
        {
            var result = await client.GetAsync(endpoint);
            var content = await result.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<Result<T>>(content);

            return (Result<T>)Convert.ChangeType(response, typeof(Result<T>));
        }
    }
}
