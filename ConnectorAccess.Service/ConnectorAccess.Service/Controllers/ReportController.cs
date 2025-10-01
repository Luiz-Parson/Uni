using ConnectorAccess.Service.models.dtos;
using ConnectorAccess.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Linq;

namespace ConnectorAccess.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ReportService reportService;

        public ReportController(ReportService reportService)
        {
            this.reportService = reportService;
        }

        [HttpPost("getReport")]
        public IActionResult GetProductReport([FromBody] ReportDTO reportDTO)
        {
            DataTable report = reportService.GetProductReport(
                reportDTO.Description,
                reportDTO.Epc,
                reportDTO.Sku,
                reportDTO.InitialDate,
                reportDTO.EndDate
            );

            string result = JsonConvert.SerializeObject(report);
            return Ok(result);
        }

        [HttpPost("getEntryAndExit")]
        public IActionResult GetAllEntrysAndExits([FromBody] ReportDTO reportDTO)
        {
            DataTable entrysAndExits = reportService.GetAllEntrysAndExits(
                reportDTO.Description,
                reportDTO.Epc,
                reportDTO.Sku,
                reportDTO.InitialDate,
                reportDTO.EndDate
            );

            string result = JsonConvert.SerializeObject(entrysAndExits);
            return Ok(result);
        }

        [HttpPost("getStock")]
        public IActionResult GetAllStock([FromBody] StockDTO stockDTO)
        {
            DataTable stock = reportService.GetAllStock(
                stockDTO.Description,
                stockDTO.Sku,
                stockDTO.InitialDate,
                stockDTO.EndDate
            );

            string result = JsonConvert.SerializeObject(stock);
            return Ok(result);
        }

        [HttpGet("getSequence")]
        public IActionResult GetSequence()
        {
            long sequence = reportService.GetSequence();
            return Ok(sequence);
        }

        [HttpPost("getValidityReport")]
        public IActionResult GetValidityReport([FromBody] ValidityReportDTO validityReportDTO)
        {
            DataTable validityReport = reportService.GetValidityReport(
                validityReportDTO.Description,
                validityReportDTO.Epc,
                validityReportDTO.Sku
            );

            string result = JsonConvert.SerializeObject(validityReport);
            return Ok(result);
        }

        [HttpPost("getLiveReport")]
        public IActionResult GetLiveReport([FromBody] ReportDTO reportDTO)
        {
            DataTable liveReport = reportService.GetLiveReport(
                reportDTO.Description,
                reportDTO.Epc,
                reportDTO.Sku,
                reportDTO.InitialDate,
                reportDTO.EndDate
            );

            string result = JsonConvert.SerializeObject(liveReport);
            return Ok(result);
        }
    }
}
