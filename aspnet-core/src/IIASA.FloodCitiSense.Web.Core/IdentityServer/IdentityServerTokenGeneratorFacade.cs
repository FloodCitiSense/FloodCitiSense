using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using IIASA.FloodCitiSense.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IIASA.FloodCitiSense.Web.IdentityServer
{
    public class IdentityServerTokenGeneratorFacade
    {
        private readonly IIntrospectionRequestValidator _introspectionRequestValidator;

        public IdentityServerTokenGeneratorFacade(IOptions<IdentityServerTokenGeneratorFacadeSettings> settings, IdentityServerOptions options,
            IClientStore clientStore,
            IIntrospectionRequestValidator introspectionRequestValidator,
            IResourceStore resourceStore, IUserClaimsPrincipalFactory<User> principalFactory,
            ITokenService tokenService, IRefreshTokenService refreshTokenService)
        {
            _introspectionRequestValidator = introspectionRequestValidator;
            ClientId = settings.Value.ClientId;

            Options = options;
            ClientStore = clientStore;
            ResourceStore = resourceStore;
            PrincipalFactory = principalFactory;
            TokenService = tokenService;
            RefreshTokenService = refreshTokenService;
        }

        protected IClientStore ClientStore { get; set; }

        protected IResourceStore ResourceStore { get; set; }

        protected IdentityServerOptions Options { get; set; }

        protected IUserClaimsPrincipalFactory<User> PrincipalFactory { get; set; }

        protected ITokenService TokenService { get; set; }

        protected IRefreshTokenService RefreshTokenService { get; set; }

        public string ClientId { get; protected set; }

        // Based on https://stackoverflow.com/a/44322425/1249506
        public async Task<IdentityServerToken> GetIdentityServerTokenForUserAsync(User user)
        {
            var request = new TokenCreationRequest();
            var identityPricipal = await PrincipalFactory.CreateAsync(user);

            var identityUser =
                new IdentityServerUser(user.Id.ToString())
                {
                    AdditionalClaims = identityPricipal.Claims.ToArray(),
                    DisplayName = user.UserName,
                    AuthenticationTime = DateTime.UtcNow,
                    IdentityProvider = IdentityServerConstants.LocalIdentityProvider
                };

            request.Subject = identityUser.CreatePrincipal();
            request.IncludeAllIdentityClaims = true;
            request.ValidatedRequest = new ValidatedRequest
            {
                Subject = request.Subject,
            };

            var client = await ClientStore.FindClientByIdAsync(ClientId);
            request.ValidatedRequest.SetClient(client);
            request.Resources =
                new Resources(await ResourceStore.FindEnabledIdentityResourcesByScopeAsync(client.AllowedScopes),
                    await ResourceStore.FindApiResourcesByScopeAsync(client.AllowedScopes))
                {
                    OfflineAccess = client.AllowOfflineAccess
                };

            request.ValidatedRequest.Options = Options;
            request.ValidatedRequest.ClientClaims = identityUser.AdditionalClaims;

            var token = await TokenService.CreateAccessTokenAsync(request);
            var accessToken = await TokenService.CreateSecurityTokenAsync(token);
            var refreshToken = await RefreshTokenService.CreateRefreshTokenAsync(request.Subject, token, client);

            return new IdentityServerToken(token, accessToken, refreshToken);
        }
    }
}