// The MIT License (MIT)
//
// Copyright (c) 2015 Scott Lance
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// The most recent version of this license can be found at: http://opensource.org/licenses/MIT

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IdSrvPlugin.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly ISerializationService serialization;        
        
        public HttpClientService(ISerializationService serialization)
        {
			ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;

			this.serialization = serialization;          
        }

        public async Task<HttpResponseMessage> PostAsync<T>(Uri requestUri, T payload) where T : class            
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
            var content = new StringContent(serialization.Serialize(payload));

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            requestMessage.Content = content;

            return await MakeRequestAsync(requestMessage);
        }

        public async Task<T2> PostDeserializedAsync<T1, T2>(Uri requestUri, T1 payload)
            where T1 : class
            where T2 : class
        {
            var result = await PostAsync<T1>(requestUri, payload);
            var content = await result.Content.ReadAsStringAsync();
           
            return serialization.Deserialize<T2>(content);
        }

        private async Task<HttpResponseMessage> MakeRequestAsync(HttpRequestMessage request)
        {
            using (HttpClient httpClient = new HttpClient())
            {                                
                var result = await httpClient.SendAsync(request).ConfigureAwait(false);

                if (result.IsSuccessStatusCode)
                    return result;

                return null;
            }
        }
    }
}