using ConnectorAccess.Service.models.dtos;
using ConnectorAccess.Service.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ConnectorAccess.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemUserController : ControllerBase
    {

        private readonly SystemUserService systemUserService;

        public SystemUserController(SystemUserService systemUserService)
        {
            this.systemUserService = systemUserService;
        }

        [HttpPost]
        public IActionResult addSystemUser([FromBody] SystemUserDTO systemUserDTO)
        {

            systemUserService.addSystemUser(
                systemUserDTO.Username,
                systemUserDTO.Password,
                systemUserDTO.IsAdmin,
                systemUserDTO.CreatedBy
            );

            return Ok("Usuário salvo com sucesso!");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            var user = systemUserService.Login(loginDTO.Username, loginDTO.Password);
            if (user == null)
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            return Ok(new
            {
                user.Id,
                user.Username,
                user.IsAdmin
            });
        }
    }
}
