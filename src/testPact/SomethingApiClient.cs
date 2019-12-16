using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;

namespace testPact
{
    public class SomethingApiClient
    {
        private readonly HttpClient _client;

        public SomethingApiClient(string baseUri = null)
        {
            _client = new HttpClient { BaseAddress = new Uri(baseUri ?? "http://my-api") };
        }

        public Something GetSomething(string id)
        {
            string reasonPhrase;

            var request = new HttpRequestMessage(HttpMethod.Get, "/somethings/" + id);
            request.Headers.Add("Accept", "application/json");

            var response = _client.SendAsync(request);

            var content = response.Result.Content.ReadAsStringAsync().Result;
            var status = response.Result.StatusCode;

            reasonPhrase = response.Result.ReasonPhrase; //NOTE: any Pact mock provider errors will be returned here and in the response body

            request.Dispose();
            response.Dispose();

            if (status == HttpStatusCode.OK)
            {
                return !String.IsNullOrEmpty(content) ?
                    JsonConvert.DeserializeObject<Something>(content)
                    : null;
            }

            throw new Exception(reasonPhrase);
        }
    }
}