﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Core.Entities.Concrete;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using Core.Extensions;
using Core.Utilities.Security.Encription;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
 


namespace Core.Utilities.Security.Jwt
{
    public class JwtHelper:ITokenHelper
    {
        //konfigurasyon dosyası vasıtasıyla o bilgileri okuyacağız
        public IConfiguration Configuration { get; }
        private TokenOptions _tokenOptions;
        private DateTime _accessTokenExpiration;


        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
        }

        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
        {
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };
        }

        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials, List<OperationClaim> operationClaims)
        {
            var jwt = new JwtSecurityToken(
                    issuer:tokenOptions.Issuer,
                    audience:tokenOptions.Audience,
                    expires: _accessTokenExpiration,
                    notBefore:DateTime.Now,
                    claims:SetClaims(user,operationClaims),
                    signingCredentials:signingCredentials
                );

            return jwt;
        }

        public IEnumerable<Claim> SetClaims(User user,List<OperationClaim> operationClaims)
        {
            var claims = new List<Claim>();
            //claims.Add(new Claim("email",user.Email));
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddRoles(operationClaims.Select(c=>c.Name).ToArray());

            return claims;
        }
    }
}
