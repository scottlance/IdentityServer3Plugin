# IdentityServer3Plugin
Plugin Template for IdP-Initiated SSO with IdentityServer 3

This is a prototype template for an Identity Server 3 Plugin. This template is based completely off of the WS-Federation plugin for Identity Server 3.  This plugin focuses on IdP-Initiated SSO.  The idea is that from a dashboard a user can click a link that will initiate SSO with a 3rd party external application.  This plugin was based around a provider that used two requests to sign on:

The first request sent a request for a token using a CompanyId, SharedSecret and a UserId, in which it responded with a token that was used in a login redirect to redirect the client to the external application.

Please note that this plugin code is for example only and does not actually connect to a external service provider.

Setup for the plugin is simple, create the ConfigurePlugins method, and then setup the PluginOptions object and add the plugin to the pipeline:

```
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
```
