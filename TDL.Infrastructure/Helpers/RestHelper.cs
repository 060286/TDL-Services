using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Cache;
using System.Text;
using TDL.Infrastructure.Exceptions;
using TDL.Infrastructure.JsonConverters;
using TDL.Infrastructure.Utilities;

namespace TDL.Infrastructure.Helpers
{
    public static class RestHelper
    {
        public static IRestResponse PostAsForm(string uri, string resource, Dictionary<string, string> headers, Dictionary<string, string> formBody)
        {
            var client = new RestClient(uri)
            {
                CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Revalidate)
            };

            var request = new RestRequest(resource, Method.POST);

            Guard.DoByCondition(headers != null, () => request.AddHeaders(headers));

            Guard.ThrowIfNull<NotFoundException>(formBody, "Form body cannot be null!");

            foreach(var kvp in formBody)
            {
                request.AddParameter(kvp.Key, kvp.Value);
            }

            request.AlwaysMultipartFormData = true;

            return client.Execute(request);
        }

        public static IRestResponse Post(string uri, string resource, Dictionary<string, string> headers,
            object body, Dictionary<KeyValuePair<string, string>, ParameterType> parameters = null)
        {
            var client = new RestClient(uri)
            {
                CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Revalidate)
            };

            string serializedBody = JsonConvert.SerializeObject(body, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new StringConverter()
                }
            });

            var request = new RestRequest(resource, Method.POST);

            Guard.DoByCondition(headers != null, () => request.AddHeaders(headers));
            request.AddJsonBody(serializedBody);
            Guard.DoByCondition(parameters != null,
                () =>
                {
                    foreach (var (key, value) in parameters)
                    {
                        request.Parameters.Add(new Parameter(key.Key, key.Value, value));
                    };
                });

            return client.Execute(request);
        }

        public static IRestResponse Get(string uri, string resource, Dictionary<string, string> header,
            Dictionary<string, string> queryParams)
        {
            var client = new RestClient(uri)
            {
                CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Revalidate)
            };

            var request = new RestRequest(resource, Method.GET);
            Guard.DoByCondition(header != null, () => request.AddHeaders(header));

            foreach(var kvp in queryParams)
            {
                request.AddQueryParameter(kvp.Key, kvp.Value);
            }

            return client.Execute(request);
        }
    }
}
