using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace TDL.Infrastructure.Helpers
{
    public static class JwtHelper
    {
        public static bool IsExprired(string token)
        {
            var jwtToken = new JwtSecurityToken(token);

            var expClaim = jwtToken.Claims.First(x => x.Type == "exp").Value;
            var tokenExpiryTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(expClaim)).DateTime;

            return tokenExpiryTime < DateTime.UtcNow;
        }
    }
}
