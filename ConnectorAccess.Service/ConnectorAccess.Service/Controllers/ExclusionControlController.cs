using ConnectorAccess.Service.models.dtos;
using ConnectorAccess.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ConnectorAccess.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExclusionControlController : ControllerBase
    {

        private readonly ExclusionControlService exclusionControlService;

        public ExclusionControlController(ExclusionControlService exclusionControlService)
        {
            this.exclusionControlService = exclusionControlService;
        }

        [HttpPost("add")]
        public IActionResult addExclusionControl([FromBody] ExclusionControlDTO exclusionControlDTO)
        {
            var exclusionControl = exclusionControlService.AddExclusionControl(
                exclusionControlDTO.ProductId,
                exclusionControlDTO.Category,
                exclusionControlDTO.ExcludedOn
            );

            return Ok(exclusionControl);
        }

        [HttpPost("add-multiple")]
        public IActionResult AddMultipleExclusions([FromBody] List<ExclusionControlDTO> exclusions)
        {
            if (exclusions == null || exclusions.Count == 0)
                return BadRequest("A lista de exclusões está vazia.");

            var result = exclusionControlService.AddMultipleExclusions(exclusions);
            return Ok(result);
        }
    }
}
