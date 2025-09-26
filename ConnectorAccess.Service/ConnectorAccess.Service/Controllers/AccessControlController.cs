using ConnectorAccess.Service.models.dtos;
using ConnectorAccess.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConnectorAccess.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccessControlController : ControllerBase
    {

        private readonly AccessControlService accessControlService;

        public AccessControlController(AccessControlService accessControlService)
        {
            this.accessControlService = accessControlService;
        }

        [HttpPost("add")]
        public IActionResult addAccessControl([FromBody] AccessControlDTO accessControlDTO)
        {
            var accessControl = accessControlService.AddAccessControl(
                accessControlDTO.Direction,
                accessControlDTO.ProductId,
                accessControlDTO.AccessedOn,
                accessControlDTO.Status
            );

            return Ok(accessControl);
        }

        [HttpGet("getLastAccess/{productId}")]
        public IActionResult GetLastAccessControlByProductId(int productId)
        {
            var lastAccess = accessControlService.GetLastAccessControlByProductId(productId);

            if (lastAccess == null)
            {
                return NotFound("Nenhum registro de acesso encontrado para o produto!");
            }

            return Ok(lastAccess);
        }

    }
}
