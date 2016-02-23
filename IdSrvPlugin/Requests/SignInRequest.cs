/*
 * Copyright 2015 Dominick Baier, Brock Allen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using IdSrvPlugin.Configuration;

namespace IdSrvPlugin.Requests
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class SignInRequest : IHttpActionResult
    {        
        private readonly PluginOptions options;  
        private readonly string token;

        public SignInRequest(string token, PluginOptions options)
        {
            this.options = options;
            this.token = token;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        HttpResponseMessage Execute()
        {                
            var responseData = $"<html><body><form id=\"selfsubmittingform\" method=\"POST\" action=\"{options.TokenLoginUrl}\"><input type=\"hidden\" name=\"TOKEN\" value=\"{token}\" /></form><script type='text/javascript'>document.getElementById('selfsubmittingform').submit();</script></body></html>";
            var response = new HttpResponseMessage();
            response.Content = new StringContent(responseData, Encoding.UTF8, "text/html");
            return response;
        }       
    }
}