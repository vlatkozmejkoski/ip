﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Quickstart.UI;
using IdentityServer4.Saml;
using IdentityServer4.Saml.Models;
using IdentityServer4.Test;

namespace IdSrv
{
    public static class Config
    {
        public static List<TestUser> GetUsers()
        {
            return TestUsers.Users;
        }


        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("role", "Role", new List<string> {JwtClaimTypes.Role, ClaimTypes.Role })
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> {
                new ApiResource("sensorsapi", JwtClaimTypes.Role)
                
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientName = "Sensor Dashboard",
                    ClientId = "sensorclient",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    RedirectUris =
                    {
                        "http://localhost:33117/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:33117/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "sensorsapi",
                        "role"
                    },
                    IdentityProviderRestrictions = new List<string>() { }
                    ,RequireConsent = false
                    ,EnableLocalLogin = true,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                },
                new Client {
                      ClientId = "http://test1.localhost:49585/saml",
                      ClientName = "tenant Client",
                      ProtocolType = IdentityServerConstants.ProtocolTypes.Saml2p,
                      AllowedScopes = { "openid", "profile", "role"}
                },
                new Client
                {
                    ClientId = "tenantApp",
                    ClientName = "tenant Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    RedirectUris = { "http://test1.localhost:49585/signin-oidc", "http://test2.localhost:49585/signin-oidc" },
                    PostLogoutRedirectUris = { "http://test1.localhost:49585/signout-callback-oidc", "http://test2.localhost:49585/signout-callback-oidc" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "sensorsapi",
                        "role"
                    },
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                   // ProtocolType = IdentityServerConstants.ProtocolTypes.Saml2p,
                }
            };
        }

        public static List<ServiceProvider> GetServiceProviders()
        {
            return new List<ServiceProvider>
            {
                new ServiceProvider {
                      EntityId = "http://test1.localhost:49585/saml",
                      SigningCertificates = {new X509Certificate2("TestClient.cer")},
                      AssertionConsumerServices = { new Service(SamlConstants.BindingTypes.HttpPost, "http://test1.localhost:49585/signin-saml") },
  
                }
        };
        }
    }
}