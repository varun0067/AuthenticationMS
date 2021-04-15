using AuthenticationServiceAPI.DTO;
using AuthenticationServiceAPI.Interface;
using AuthenticationServiceAPI.Models;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthenticationServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private IMemberService _service;
        private readonly ILog _log4net = LogManager.GetLogger(typeof(AuthorizationController));

        public AuthorizationController(IMemberService service)
        {
            _service=service;
        }


        [HttpPost("Login")]
        public ActionResult Login([FromBody] MemberLoginDTO member)
        {
            _log4net.Info("HttpPost request : " + nameof(Login));
            if (member == null)
            {
                return BadRequest("Member details cannot be empty");
            }
            
            try
            {
                Member AuthenticatedUser = _service.AuthenticateUser(member);
                if(AuthenticatedUser != null)
                {
                    TokenUserDTO token=_service.CreateJwt(AuthenticatedUser);
                    return Ok(token);
                }
                else
                    return BadRequest("Invalid Credentials!!!");
            }
            catch(Exception e)
            {
                _log4net.Error("Exception Occured : " + e.Message + " from " + nameof(Login));
                return BadRequest("Exception Occured");
            }
        }
    }
}
