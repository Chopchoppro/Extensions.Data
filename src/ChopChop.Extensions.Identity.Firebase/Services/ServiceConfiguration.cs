using ChopChop.Extensions.Identity.Firebase.Services.Infostruct;

using FirebaseAdmin;

using Google.Apis.Auth.OAuth2;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChopChop.Extensions.Identity.Firebase.Services;

public static class ServiceConfiguration
{
    public static IServiceCollection RegisterFirebaseAuth(this IServiceCollection services, IConfiguration Configuration, bool isCreateFirebaseApp = false)
    {
        var firebaseOptions = Configuration.GetSection("FirebaseOptions")
       .Get<FirebaseOptions>();

        if (isCreateFirebaseApp)
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromJson(firebaseOptions.ToJson())
            });

        services.AddTransient<IFirebaseServices, FirebaseServices>(c => new FirebaseServices(new HttpClient() { BaseAddress = new Uri(firebaseOptions.Url) }, firebaseOptions.ApiKey));


        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
         .AddJwtBearer(options =>
         {
             options.Events = new JwtBearerEvents
             {
                 OnAuthenticationFailed = MyMethod(),

             };
             options.Authority = firebaseOptions.Validator;
             options.Audience = firebaseOptions.Audience;
             options.TokenValidationParameters.ValidIssuer = firebaseOptions.Validator;

         });
        services.AddAuthorization();
        return services;
    }

    private static Func<AuthenticationFailedContext, Task> MyMethod()
    {
        return ctx =>
        {
            if (ctx.HttpContext.Request != null)
            {
                //var logger = ctx.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
                //logger.LogError(0, ctx.Exception, "Token validation failed");
            }

            return Task.CompletedTask;
        };
    }
}
