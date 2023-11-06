using System;
using System.Collections.Generic;
using System.Text;

namespace Itequia.SpeedCode.Security.Models
{
    public class AuthenticationRequest
    {
        public string Token { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }
        public List<string> Roles { get; set; }
        public string ClientId { get; set; }
        public Dictionary<string, string> CustomClaims { get; set; }
    }
}
