using Clinic.Common.Options;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;
using Microsoft.Extensions.Options;

namespace Clinic.Identity
{
    public static class SD
    {
        public static IConfiguration StaticConfig { get;  set; }
        public const string ADMIN = "admin";
        public const string PATIENT = "patient";
        public const string MANAGER = "manager";
        public const string DOCTOR = "doctor";
        
        public static IEnumerable<IdentityResource> IdentityResource =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new ProfileWithRoleIdentityResource(),
            };
        public static IEnumerable<ApiScope> ApiScopes =>
             new List<ApiScope>
            {
                new ApiScope(name: "Clinic", displayName: "Clinic Server",new[] { JwtClaimTypes.Role}),
                new ApiScope(name: "read", displayName: "Read your data"),
                new ApiScope(name: "write", displayName: "Write your data"),
                new ApiScope(name: "delete", displayName: "Delete your data")
            };

        public static IEnumerable<Client> Clients =>
             new List<Client>
            {
                new Client
                {
                    ClientId = "Clinic",
                    AllowedGrantTypes = GrantTypes.Code,
					RequirePkce = true,
			        RequireClientSecret = false,
					AllowedScopes = { "openid", "profile", "email", "Clinic" },
                    RedirectUris = { StaticConfig.GetSection("ApplicationSettings")["RedirectUris"] },
					PostLogoutRedirectUris = { StaticConfig.GetSection("ApplicationSettings")["PostLogout"] },
					Enabled = true
				}
        };
        public static bool ContainsCaseInsensitive(this string source, string substring)
        {
            return source?.IndexOf(substring, StringComparison.OrdinalIgnoreCase) > -1;
        }
    }
}
