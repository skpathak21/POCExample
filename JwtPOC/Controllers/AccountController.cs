using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtPOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        //private readonly IConfiguration _configuration;

        //public AccountController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}
        private readonly IJwtAuth jwtAuth;

        private readonly List<Member> lstMember = new List<Member>()
        {
            new Member{Id=1, Name="Kirtesh" },
            new Member {Id=2, Name="Nitya" },
            new Member{Id=3, Name="pankaj"}
        };
        public AccountController(IJwtAuth jwtAuth)
        {
            this.jwtAuth = jwtAuth;
        }
        // GET: api/<MembersController>
        [HttpGet]
        public IEnumerable<Member> AllMembers()
        {
            return lstMember;
        }

        // GET api/<MembersController>/5
        [HttpGet("{id}")]
        public Member MemberByid(int id)
        {
            return lstMember.Find(x => x.Id == id);
        }

        [AllowAnonymous]
        // POST api/<MembersController>
        [HttpPost("authentication")]
        public IActionResult Authentication([FromBody] UserCredential userCredential)
        {
            var token = jwtAuth.Authentication(userCredential.UserName, userCredential.Password);
            if (token == null)
                return Unauthorized();
            return Ok(token);
        }
        [Authorize]
        // POST api/<MembersController>
        [HttpGet("getdata")]
        public IActionResult getdata()
        {           
            return Ok("Hello");
        }
    }
}
