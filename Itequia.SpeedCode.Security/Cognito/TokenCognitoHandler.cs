using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Itequia.SpeedCode.Security.Cognito
{
    public class TokenCognitoHandler: ITokenCognitoHandler
    {
        public async Task<bool> Validate(string idToken, string region, string userPoolId, string clientId)
        {
            // 1. Validate JWT structure
            if (
                string.IsNullOrEmpty(idToken) ||
                idToken.Split('.').Count() != 3
            )
                throw new Exception();// BusinessException(HttpStatusCode.BadRequest, CognitoErrors.InvalidJwtStructure);


            var token = idToken.Split('.')[0].Replace('_', '/').Replace('-', '+');
            switch (token.Length % 4)
            {
                case 2: token += "=="; break;
                case 3: token += "="; break;
            }
            var decoded = Convert.FromBase64String(token);
            var decodedToken = System.Text.Encoding.Default.GetString(decoded);

            // 2. Validate JWT signature
            JObject header = JObject.Parse(decodedToken);
            if (header != null)
            {
                var headerValue = header.GetValue("kid");
                string secret = headerValue != null ? headerValue.ToString() : "";
                ClaimsPrincipal claims = await ValidateJwt(idToken, secret, region, userPoolId, clientId);

                // 3. Validate claims
                if (claims != null && claims.Claims != null)
                {
                    Claim? useClaim = claims.Claims.FirstOrDefault(x => x.Type == "token_use");
                    if (useClaim == null || useClaim.Value != "access")
                        throw new Exception();// BusinessException(HttpStatusCode.BadRequest, CognitoErrors.IdTokenUse);

                    Claim? userClaim = claims.Claims.FirstOrDefault(x => x.Type == "username");
                    return (userClaim != null) ? true : false;
                }
            }
            return false;

        }
       
        private async Task<ClaimsPrincipal> ValidateJwt(string jwt, string secret, string region, string userPoolId, string clientId)
        {
            string jwkUrl = JsonWebKeySetUrl(region, userPoolId);

            JObject publicKeys = JObject.Parse(await new HttpClient().GetStringAsync(jwkUrl));
            JArray jwkSecrets = new JArray(publicKeys.GetValue("keys").Children());

            if (!jwkSecrets.Any(x => ((JObject)x).GetValue("kid").ToString() == secret))
                throw new Exception();// BusinessException(HttpStatusCode.BadRequest, CognitoErrors.InvalidJwtSignature);

            var keys = new List<SecurityKey>();
            foreach (JObject webKey in jwkSecrets)
            {
                var e = Base64UrlDecode(webKey.GetValue("e").ToString());
                var n = Base64UrlDecode(webKey.GetValue("n").ToString());

                var key = new RsaSecurityKey(new RSAParameters { Exponent = e, Modulus = n })
                {
                    KeyId = webKey.GetValue("kid").ToString()
                };

                keys.Add(key);
            }

            var parameters = new TokenValidationParameters
            {
                ValidIssuer = Issuer(region, userPoolId),
                ValidateAudience = false,
                IssuerSigningKeys = keys,

                NameClaimType = "name",
                RoleClaimType = "role",

                RequireSignedTokens = true
            };

            var handler = new JwtSecurityTokenHandler();
            handler.InboundClaimTypeMap.Clear();

            var user = handler.ValidateToken(jwt, parameters, out var _);
            return user;
        }

        private byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 1: output += "==="; break; // Three pad chars
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                default: throw new System.Exception("Illegal base64url string!");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }

        private string Issuer(string region, string userPoolId) => $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";
        private string JsonWebKeySetUrl(string region, string userPoolId) => $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}/.well-known/jwks.json";
    }
}
