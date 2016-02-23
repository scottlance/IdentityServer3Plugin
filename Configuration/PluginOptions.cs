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

using IdentityServer3.Core.Configuration;
using System;

namespace IdSrvPlugin.Configuration
{
    public class PluginOptions
    {            
        public string CreateTokenUrl { get; set; }
        public string CompanyId { get; set; }
        public string SharedSecret { get; set; }
        public string TokenLoginUrl { get; set; }

        /// <summary>
        /// Gets or sets the identity server options.
        /// </summary>
        /// <value>
        /// The identity server options.
        /// </value>
        public IdentityServerOptions IdentityServerOptions { get; set; }

        /// <summary>
        /// Gets or sets the WS-Federation service factory.
        /// </summary>
        /// <value>
        /// The factory.
        /// </value>
        public PluginServiceFactory Factory { get; set; }   

        /// <summary>
        /// Gets the data protector.wH
        /// </summary>
        /// <value>
        /// The data protector.
        /// </value>
        public IDataProtector DataProtector
        {
            get
            {
                return IdentityServerOptions.DataProtector;
            }
        }

        /// <summary>
        /// Gets or sets the map path.
        /// </summary>
        /// <value>
        /// The map path.
        /// </value>
        public string MapPath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginOptions"/> class.
        /// </summary>
        public PluginOptions()
        {
            MapPath = "/plugin";            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginOptions"/> class.
        /// Assigns the IdentityServerOptions and the Factory from the IdentityServerOptions.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <exception cref="System.ArgumentNullException">options</exception>
        public PluginOptions(IdentityServerOptions options)
            : this()
        {
            if (options == null) throw new ArgumentNullException("options");
            
            this.IdentityServerOptions = options;
            this.Factory = new PluginServiceFactory(options.Factory);
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// Factory not configured
        /// or
        /// DataProtector not configured
        /// or
        /// Options not configured
        /// </exception>
        public void Validate()
        {        
            if(string.IsNullOrWhiteSpace(CreateTokenUrl))
            {
                throw new ArgumentNullException(nameof(CreateTokenUrl));
            }

            if(string.IsNullOrWhiteSpace(CompanyId))
            {
                throw new ArgumentNullException(nameof(CompanyId));
            }

            if(string.IsNullOrWhiteSpace(SharedSecret))
            {
                throw new ArgumentNullException(nameof(SharedSecret));
            }

            if(string.IsNullOrWhiteSpace(TokenLoginUrl))
            {
                throw new ArgumentNullException(nameof(TokenLoginUrl));
            }

            if (Factory == null)
            {
                throw new ArgumentNullException("Factory not configured");
            }

            if (DataProtector == null)
            {
                throw new ArgumentNullException("DataProtector not configured");
            }

            if (IdentityServerOptions == null)
            {
                throw new ArgumentNullException("Options not configured");
            }
        }
    }
}