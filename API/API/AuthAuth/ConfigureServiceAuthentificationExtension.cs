﻿// based on https://blog.devgenius.io/security-in-react-and-webapi-in-asp-net-core-c-with-authentification-and-authorization-by-keycloak-f890d340d093
// Needs: Microsoft.AspNetCore.Authentication.Jwt
// Needs: Newtonsoft.Json

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;

namespace API.AuthAuth
{
    public static class ConfigureServiceAuthentificationExtension
    {
        /// <summary>
        /// Used to get the role within the claims structure used by keycloak, then it adds the role(s) in the ClaimsItentity of ClaimsPrincipal.Identity
        /// </summary>
        public class ClaimsTransformer : IClaimsTransformation
        {
            public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
            {
                ClaimsIdentity claimsIdentity = (ClaimsIdentity)principal.Identity;

                // flatten resource_access because Microsoft identity model doesn't support nested claims
                // by map it to Microsoft identity model, because automatic JWT bearer token mapping already processed here
                if (claimsIdentity.IsAuthenticated && claimsIdentity.HasClaim((claim) => claim.Type == "resource_access"))
                {
                    var userRole = claimsIdentity.FindFirst((claim) => claim.Type == "resource_access");

                    var content = Newtonsoft.Json.Linq.JObject.Parse(userRole.Value);
                    if (content[System.Environment.GetEnvironmentVariable("AuthClient")] != null)
                    {
                        foreach (var role in content[System.Environment.GetEnvironmentVariable("AuthClient")]["roles"])
                        {
                            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
                        }
                    }
                }

                return Task.FromResult(principal);
            }
        }

        private static RsaSecurityKey BuildRSAKey(string publicKeyJWT)
        {
            RSA rsa = RSA.Create();

            rsa.ImportSubjectPublicKeyInfo(

                source: Convert.FromBase64String(publicKeyJWT),
                bytesRead: out _
            );

            var IssuerSigningKey = new RsaSecurityKey(rsa);

            return IssuerSigningKey;
        }

        public static void ConfigureJWT(this IServiceCollection services, bool IsDevelopment, string publicKeyJWT)
        {
            services.AddTransient<IClaimsTransformation, ClaimsTransformer>();

            var AuthenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            AuthenticationBuilder.AddJwtBearer(o =>
            {

                #region == JWT Token Validation ===

                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    //ValidIssuers = new[] { "https://auth.a.ucnit.eu/auth/realms/xOIDCx" },
                    ValidIssuers = new[] { System.Environment.GetEnvironmentVariable("AuthURL") },

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = BuildRSAKey(publicKeyJWT),
                    ValidateLifetime = true
                };

                #endregion

                #region === Event Authentification Handlers ===

                o.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = c =>
                    {
                        Console.WriteLine("User successfully authenticated");
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();

                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";

                        if (IsDevelopment)
                        {
                            return c.Response.WriteAsync(c.Exception.ToString());
                        }
                        return c.Response.WriteAsync("An error occured processing your authentication.");
                    }
                };

                #endregion

            });
        }
    }
}
