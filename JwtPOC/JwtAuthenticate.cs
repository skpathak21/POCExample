using JwtPOC.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JwtPOC
{

    public interface IJwtAuth
    {
        string Authentication(string username, string password);
    }
    public class Auth : IJwtAuth
    {
        private readonly string username = "kirtesh";
        private readonly string password = "Demo1";
        private readonly string key;
        public Auth(string key)
        {
            this.key = key;
        }
        public string Authentication(string username, string password)
        {
            if (!(username.Equals(username) || password.Equals(password)))
            {
                return null;
            }

            // 1. Create Security Token Handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // 2. Create Private Key to Encrypted
            var tokenKey = Encoding.ASCII.GetBytes(key);

            //3. Create JETdescriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, username)
                    }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            //4. Create Token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 5. Return Token from method
            return tokenHandler.WriteToken(token);
        }
    }

    public class UserCredential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    //public class RevokeTokenRequest
    //{
    //    public string Token { get; set; }
    //}
    //public interface IUserService
    //{
    //    AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
    //    AuthenticateResponse RefreshToken(string token, string ipAddress);
    //    bool RevokeToken(string token, string ipAddress);
    //    IEnumerable<User> GetAll();
    //    User GetById(int id);
    //}

    //public class UserService : IUserService
    //{
    //    private DataContext _context;
    //    private readonly AppSettings _appSettings;

    //    public UserService(
    //        DataContext context,
    //        IOptions<AppSettings> appSettings)
    //    {
    //        _context = context;
    //        _appSettings = appSettings.Value;
    //    }

    //    public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
    //    {
    //        var user = _context.Users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

    //        // return null if user not found
    //        if (user == null) return null;

    //        // authentication successful so generate jwt and refresh tokens
    //        var jwtToken = generateJwtToken(user);
    //        var refreshToken = generateRefreshToken(ipAddress);

    //        // save refresh token
    //        user.RefreshTokens.Add(refreshToken);
    //        _context.Update(user);
    //        _context.SaveChanges();

    //        return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
    //    }

    //    public AuthenticateResponse RefreshToken(string token, string ipAddress)
    //    {
    //        var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

    //        // return null if no user found with token
    //        if (user == null) return null;

    //        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

    //        // return null if token is no longer active
    //        if (!refreshToken.IsActive) return null;

    //        // replace old refresh token with a new one and save
    //        var newRefreshToken = generateRefreshToken(ipAddress);
    //        refreshToken.Revoked = DateTime.UtcNow;
    //        refreshToken.RevokedByIp = ipAddress;
    //        refreshToken.ReplacedByToken = newRefreshToken.Token;
    //        user.RefreshTokens.Add(newRefreshToken);
    //        _context.Update(user);
    //        _context.SaveChanges();

    //        // generate new jwt
    //        var jwtToken = generateJwtToken(user);

    //        return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
    //    }

    //    public bool RevokeToken(string token, string ipAddress)
    //    {
    //        var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

    //        // return false if no user found with token
    //        if (user == null) return false;

    //        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

    //        // return false if token is not active
    //        if (!refreshToken.IsActive) return false;

    //        // revoke token and save
    //        refreshToken.Revoked = DateTime.UtcNow;
    //        refreshToken.RevokedByIp = ipAddress;
    //        _context.Update(user);
    //        _context.SaveChanges();

    //        return true;
    //    }

    //    public IEnumerable<User> GetAll()
    //    {
    //        return _context.Users;
    //    }

    //    public User GetById(int id)
    //    {
    //        return _context.Users.Find(id);
    //    }

    //    // helper methods

    //    private string generateJwtToken(User user)
    //    {
    //        var tokenHandler = new JwtSecurityTokenHandler();
    //        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
    //        var tokenDescriptor = new SecurityTokenDescriptor
    //        {
    //            Subject = new ClaimsIdentity(new Claim[]
    //            {
    //                new Claim(ClaimTypes.Name, user.Id.ToString())
    //            }),
    //            Expires = DateTime.UtcNow.AddMinutes(15),
    //            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    //        };
    //        var token = tokenHandler.CreateToken(tokenDescriptor);
    //        return tokenHandler.WriteToken(token);
    //    }

    //    private RefreshToken generateRefreshToken(string ipAddress)
    //    {
    //        using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
    //        {
    //            var randomBytes = new byte[64];
    //            rngCryptoServiceProvider.GetBytes(randomBytes);
    //            return new RefreshToken
    //            {
    //                Token = Convert.ToBase64String(randomBytes),
    //                Expires = DateTime.UtcNow.AddDays(7),
    //                Created = DateTime.UtcNow,
    //                CreatedByIp = ipAddress
    //            };
    //        }
    //    }
    //}
}
