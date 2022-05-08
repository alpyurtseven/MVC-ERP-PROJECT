using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Service
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfiguration = new HttpConfiguration();
            ConfigureOAuth(app);
            WebApiConfig.Register(httpConfiguration);
            app.UseWebApi(httpConfiguration);
        }

        void ConfigureOAuth(IAppBuilder app)
        {
          
            OAuthAuthorizationServerOptions oAuthAuthorizationServerOptions = new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1), 
                AllowInsecureHttp = true, 
                Provider = new Authorization() 
            };

           
            app.UseOAuthAuthorizationServer(oAuthAuthorizationServerOptions);

          
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

           
        }
    }
}