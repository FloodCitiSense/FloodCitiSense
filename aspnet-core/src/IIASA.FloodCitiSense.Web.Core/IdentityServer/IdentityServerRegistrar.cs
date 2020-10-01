using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Abp.IdentityServer4;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IIASA.FloodCitiSense.Authorization.Users;
using IIASA.FloodCitiSense.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;

namespace IIASA.FloodCitiSense.Web.IdentityServer
{
    public static class IdentityServerRegistrar
    {
        public static void Register(IServiceCollection services, IConfigurationRoot configuration,
            IHostingEnvironment env)
        {
            var certPath = Path.Combine(env.ContentRootPath, "fcs.pfx");
            if (!File.Exists(certPath))
            {
                throw new Exception($"{env.ContentRootPath} not able to find fcs.pfx");
            }
            services.AddIdentityServer(x =>
                {
                    x.IssuerUri = configuration["App:ServerRootAddress"];
                    x.PublicOrigin = configuration["App:ServerRootAddress"];
                })
                .AddSigningCredential(new X509Certificate2(certPath, "floodcitisense"))
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                .AddInMemoryClients(IdentityServerConfig.GetClients(configuration))
                .AddAbpPersistedGrants<FloodCitiSenseDbContext>()
                .AddAbpIdentityServer<User>();
        }

        private static SigningCredentials CreateSigningCredential()
        {
            var credentials = new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.RsaSha256Signature);

            return credentials;
        }
        private static RSACryptoServiceProvider GetRsaCryptoServiceProvider()
        {
            return new RSACryptoServiceProvider(2048);
        }
        private static SecurityKey GetSecurityKey()
        {
            return new RsaSecurityKey(GetRsaCryptoServiceProvider());
        }
    }
}
