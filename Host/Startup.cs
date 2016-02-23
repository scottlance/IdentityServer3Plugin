using Host.Config;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Host.Config;
using Owin;
using Serilog;
using IdSrvPlugin.Configuration;

namespace Host
{
    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // setup serilog to use diagnostics trace
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Trace(outputTemplate: "{Timestamp} [{Level}] ({Name}){NewLine} {Message}{NewLine}{Exception}")
                .CreateLogger();

            app.Map("/core", coreApp =>
            {
                var factory = new IdentityServerServiceFactory()
                    .UseInMemoryUsers(Users.Get())
                    .UseInMemoryClients(Clients.Get())
                    .UseInMemoryScopes(Scopes.Get());

                var options = new IdentityServerOptions
                {
                    SiteName = "IdentityServer3 with Plugin",

                    SigningCertificate = Certificate.Get(),
                    Factory = factory,
                    PluginConfiguration = ConfigurePlugins,
                };

                coreApp.UseIdentityServer(options);
            });
        }

        private void ConfigurePlugins(IAppBuilder pluginApp, IdentityServerOptions options)
        {
            var pluginOptions = new PluginOptions(options)
            {
                CompanyId = "CompanyId",
                CreateTokenUrl = "http://token.org/createtoken/url", 
                SharedSecret = "SharedSecret",
                TokenLoginUrl = "http://token.org/login/url"
            };
           
            pluginApp.UsePlugin(pluginOptions);
        }
    }
}