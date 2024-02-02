using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts.Infrastructure;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Domain;

namespace Infrastructure
{
    public class TokenService : ITokenService
    {
        private IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string BuildToken(ApplicationUser user, bool rememberme)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Key"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("email", user.Email));
            claimsForToken.Add(new Claim("userid", user.Id));
            claimsForToken.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claimsForToken.Add(new Claim(JwtRegisteredClaimNames.NameId, user.UserName));
            claimsForToken.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            var jwtSecurityToke = new JwtSecurityToken(
                        expires: rememberme ? DateTime.Now.AddDays(30) : DateTime.Now.AddHours(10),
                        claims: claimsForToken,
                        signingCredentials: signingCredentials,
                        audience: _configuration["JWT:Audience"],
                        issuer: _configuration["JWT:Issuer"]

           );
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claimsForToken),
                Expires = rememberme ? DateTime.Now.AddDays(30) : DateTime.Now.AddHours(10),
                SigningCredentials = signingCredentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            //.WriteToken(jwtSecurityToke);
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

            //return tokenToReturn.ToString();
        }

        public bool IsTokenValid(string key, string issuer, string token)
        {
            throw new NotImplementedException();
        }
    }
}
