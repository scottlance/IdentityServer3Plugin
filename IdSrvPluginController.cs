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
 
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web.Http;
using IdentityServer3.Core;
using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Logging;
using IdSrvPlugin.Configuration;
using IdSrvPlugin.Configuration.Hosting;
using IdSrvPlugin.Requests;
using IdSrvPlugin.Services;

namespace IdSrvPlugin
{
    [HostAuthentication(Constants.PrimaryAuthenticationType)]
    [RoutePrefix("")]
    [NoCache]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [SecurityHeaders(EnableCsp = false)]
    public class IdSrvPluginController : ApiController
    {       
        private static readonly ILog logger = LogProvider.GetCurrentClassLogger();
        private readonly IHttpClientService httpClientService;
        private readonly PluginOptions options;  

        public IdSrvPluginController(IHttpClientService httpClientService, PluginOptions torchOptions)
        {
            this.httpClientService = httpClientService;
            this.options = torchOptions;
        }

        [Route("")]
        [SecurityHeaders(EnableCsp = false, EnableXfo = false)]
        public async Task<IHttpActionResult> Get()
        {
            logger.Info("Processing Torch sign in request");

            var subject = User.GetSubjectId();

            if (subject == null) throw new NullReferenceException(nameof(subject));

            var tokenRequest = new TokenRequest(options, httpClientService);
            var token = await tokenRequest.GetToken(subject);

            return new SignInRequest(token.Token, options);
        }
    }
}
