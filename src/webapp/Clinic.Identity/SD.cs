﻿using Duende.IdentityServer.Models;

namespace Clinic.Identity
{
    public static class SD
    {
        public const string ADMIN = "admin";
        public const string PATIENT = "patient";
        public const string MANAGER = "manager";
        public const string DOCTOR = "doctor";

        public static IEnumerable<IdentityResource> IdentityResource =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };
        public static IEnumerable<ApiScope> ApiScopes =>
             new List<ApiScope>
            {
                new ApiScope(name: "magic", displayName: "Magic Server"),
                new ApiScope(name: "read", displayName: "Read your data"),
                new ApiScope(name: "write", displayName: "Write your data"),
                new ApiScope(name: "delete", displayName: "Delete your data")
            };

        public static IEnumerable<Client> Clients =>
             new List<Client>
            {
                new Client
                {
                    ClientId = "service.client",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api1", "api2.read_only"}
                },
                new Client
                {
                    ClientId = "magic",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = { "magic"},
                    RedirectUris = { "https://localhost:44379/signin-oidic" },
                    PostLogoutRedirectUris = { "https://localhost:44379/signout-callback-oidc" }
                }
        };
    }
}
