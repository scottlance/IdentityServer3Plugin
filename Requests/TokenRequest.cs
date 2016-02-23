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
using System.Threading.Tasks;
using IdentityServer3.Core.Logging;
using IdSrvPlugin.Configuration;
using IdSrvPlugin.Models;
using IdSrvPlugin.Services;

namespace IdSrvPlugin.Requests
{
    public class TokenRequest
    {
        private readonly static ILog logger = LogProvider.GetCurrentClassLogger();        
        private readonly IHttpClientService httpClientService;
        private readonly PluginOptions options;  

        public TokenRequest(PluginOptions options, IHttpClientService httpClientService)
        {
            this.options = options;
            this.httpClientService = httpClientService;
        }

        public async Task<TokenResult> GetToken(string subject)
        {
            logger.Info("Getting user token");
           
            var requestData = new SignInData
            {
                CompanyId = options.CompanyId,
                SharedSecret = options.SharedSecret,
                UserId = subject
            };

            return await httpClientService.PostDeserializedAsync<SignInData, TokenResult>(new Uri(options.CreateTokenUrl), requestData);            
        }  
    }
}