using System;
using System.Linq;
using Abp.AspNetCore;
using Abp.AspNetZeroCore.Web.Authentication.JwtBearer;
using Abp.Castle.Logging.Log4Net;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Hangfire;
using Abp.Timing;
using Castle.Facilities.Logging;
using Castle.Services.Logging.SerilogIntegration;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IIASA.FloodCitiSense.Authorization;
using IIASA.FloodCitiSense.Authorization.Roles;
using IIASA.FloodCitiSense.Authorization.Users;
using IIASA.FloodCitiSense.Configuration;
using IIASA.FloodCitiSense.EntityFrameworkCore;
using IIASA.FloodCitiSense.Identity;
using IIASA.FloodCitiSense.Install;
using IIASA.FloodCitiSense.MultiTenancy;
using IIASA.FloodCitiSense.Web.Authentication.JwtBearer;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using Swashbuckle.AspNetCore.Swagger;
using IIASA.FloodCitiSense.Web.IdentityServer;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Exceptions;
using IIASA.FloodCitiSense.MobilePushNotification;

#if FEATURE_SIGNALR
using Abp.Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using Owin.Security.AesDataProtectorProvider;
using Abp.Web.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Abp.AspNetZeroCore.Web.Owin;
#endif

namespace IIASA.FloodCitiSense.Web.Startup
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        private const string DefaultCorsPolicyName = "localhost";

        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            IdentityModelEventSource.ShowPII = true; //To show detail of error and see the problem

            //MVC
            services
                .AddMvc(options => options.Filters.Add(new CorsAuthorizationFilterFactory(DefaultCorsPolicyName)))
                .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            //Configure CORS for angular2 UI
            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    //App:CorsOrigins in appsettings.json can contain more than one address with splitted by comma.
                    builder
                        //.WithOrigins(_appConfiguration["App:CorsOrigins"].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(o => o.RemovePostFix("/")).ToArray())
                        .AllowAnyOrigin() //TODO: Will be replaced by above when Microsoft releases microsoft.aspnetcore.cors 2.0 - https://github.com/aspnet/CORS/pull/94
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

            //Identity server
            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                IdentityServerRegistrar.Register(services, _appConfiguration, _env);

                services.Configure<IdentityServerTokenGeneratorFacadeSettings>(
                    _appConfiguration.GetSection(nameof(IdentityServerTokenGeneratorFacadeSettings)));
                services.AddScoped<IdentityServerTokenGeneratorFacade>();
            }
          
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            //Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "FloodCitiSense API", Version = version.ToString(),  });
                options.DocInclusionPredicate((docName, description) => true);
            });

            //Recaptcha
            services.AddRecaptcha(new RecaptchaOptions
            {
                SiteKey = _appConfiguration["Recaptcha:SiteKey"],
                SecretKey = _appConfiguration["Recaptcha:SecretKey"]
            });

            //MobilePushNotification
            services.Configure<MobilePushNotificationConfig>(
                _appConfiguration.GetSection(nameof(MobilePushNotificationConfig)));

            //Hangfire (Enable to use Hangfire instead of default job manager)
            //services.AddHangfire(config =>
            //{
            //    config.UseSqlServerStorage(_appConfiguration.GetConnectionString("Default"));
            //});

            //Configure Abp and Dependency Injection
            return services.AddAbp<FloodCitiSenseWebHostModule>(options =>
            {
                var config = new LoggerConfiguration() //Configure Serilog here!
                    .MinimumLevel.Debug()
                    .Enrich.WithMachineName()
                    .Enrich.WithExceptionDetails()
                    .WriteTo.Seq(_appConfiguration["Seq:Url"],apiKey: _appConfiguration["Seq:apiKey"])
                    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message} {Properties:j}{NewLine}{Exception}")
                    .WriteTo.RollingFile("App_Data\\Logs\\log-{Date}.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message} {Properties:j}{NewLine}{Exception}")
                    .CreateLogger();
                //Configure Log4Net logging
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.LogUsing(new SerilogFactory(config))
                );
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //Initializes ABP framework.
            app.UseAbp(options =>
            {
                options.UseAbpRequestLocalization = false; //used below: UseAbpRequestLocalization
            });

            app.UseCors(DefaultCorsPolicyName); //Enable CORS!

            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                //app.UseJwtTokenMiddleware("IdentityBearer");
                app.UseIdentityServer();
            }

            app.UseStaticFiles();

            if (DatabaseCheckHelper.Exist(_appConfiguration["ConnectionStrings:Default"]))
            {
                app.UseAbpRequestLocalization();
            }

#if FEATURE_SIGNALR
            //Integrate to OWIN
            app.UseAppBuilder(ConfigureOwinServices);
#endif

            //Hangfire dashboard & server (Enable to use Hangfire instead of default job manager)
            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //{
            //    Authorization = new[] { new AbpHangfireAuthorizationFilter(AppPermissions.Pages_Administration_HangfireDashboard)  }
            //});
            //app.UseHangfireServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "FloodCitiSense API V1.2");
            }); //URL: /swagger

        }

#if FEATURE_SIGNALR
        private static void ConfigureOwinServices(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.Register(typeof(IAssemblyLocator), () => new SignalRAssemblyLocator());
            app.Properties["host.AppName"] = "FloodCitiSense";

            app.UseAbp();
            app.UseAesDataProtectorProvider();

            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);

                var hubConfiguration = new HubConfiguration
                {
                    EnableJSONP = true
                };

                map.RunSignalR(hubConfiguration);
            });
        }
#endif
    }
}
