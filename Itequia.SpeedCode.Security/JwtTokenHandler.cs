using Itequia.SpeedCode.Security.Models;
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

namespace Itequia.SpeedCode.Security
{
    public class JwtTokenHandler
    {
        public const string JWT_SECURITY_KEY = "AbCDEfGhYJkLmAbCDEfGhYJkLm123456";
        private const int JWT_TOKEN_VALIDITY_MINS = 20;

        public async Task<AuthenticationResponse>? GenerateJwtToken(AuthenticationRequest authenticationRequest)
        {
           
            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);


            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Name, authenticationRequest.UserName));

            if (authenticationRequest.Roles.Any())
            {
                authenticationRequest.Roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
            }

            if (authenticationRequest.CustomClaims != null && authenticationRequest.CustomClaims.Any())
            {
                authenticationRequest.CustomClaims.ToList().ForEach(cc => claims.Add(new Claim(cc.Key, cc.Value)));
            }


            var claimIdentity = new ClaimsIdentity(claims);

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new AuthenticationResponse
            {
                UserName = authenticationRequest.UserName,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds,
                JwtToken = token
            };
        }
    }
}
