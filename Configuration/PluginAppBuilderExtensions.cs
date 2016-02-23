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

using Owin;
using System;
using IdSrvPlugin.Configuration.Hosting;

namespace IdSrvPlugin.Configuration
{
    public static class PluginAppBuilderExtensions
    {
        /// <summary>
        /// Add the plugin to the IdentityServer pipeline.
        /// </summary>
        /// <param name="app">The appBuilder.</param>
        /// <param name="options">The options.</param>
        /// <returns>The appBuilder</returns>
        /// <exception cref="System.ArgumentNullException">options</exception>
        public static IAppBuilder UsePlugin(this IAppBuilder app, PluginOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            
            app.Map(options.MapPath, pluginApp =>
            {               
                pluginApp.Use<AutofacContainerMiddleware>(AutofacConfig.Configure(options));               
                pluginApp.UseWebApi(WebApiConfig.Configure(options));
            });

            return app;
        }
    }
}