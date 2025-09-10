using GeoIpProject.Api.Controllers;
using GeoIpProject.Api.Services.Interfaces;
using GeoIpProject.Api.Services.Interfaces.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeoIpProject.Features.Controllers
{
    /// <summary>
    /// Batches Controller
    /// </summary>
    [ApiController]
    [Route(ControllerNames.Batches)]
    public class BatchesController : ControllerBase
    {
        private readonly IGeoIpProjectService _geoIpProjectService;
        private readonly LinkGenerator _links;
        private readonly ILogger<BatchesController> _logger;

        /// <summary>
        /// Batches controller constructor
        /// </summary>
        /// <param name="geoIpProjectService"></param>
        /// <param name="links"></param>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public BatchesController(IGeoIpProjectService geoIpProjectService, LinkGenerator links, ILogger<BatchesController> logger)
        {
            _geoIpProjectService = geoIpProjectService ?? throw new ArgumentNullException(nameof(geoIpProjectService));
            _links = links ?? throw new ArgumentNullException(nameof(links));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates a batch from the list of ip given
        /// </summary>
        /// <param name="req"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ActionNames.CreateBatch)]
        public async Task<IActionResult> CreateBatch([FromBody] CreateBatchRequest req, CancellationToken cancellationToken)
        {
            if (!req.Ips.Any()) return BadRequest(Problem("At least one IP is required"));

            var batch = await _geoIpProjectService.CreateIpBatchesAsync(req, cancellationToken);

            var statusUrl = _links.GetUriByAction(HttpContext, action: nameof(GetBatchesProcessingStatus), controller: "Batches", values: new { id = batch.BatchId })
                            ?? $"/api/batches/{batch.BatchId}";

            batch.StatusUrl = statusUrl;

            _logger.LogInformation("Created batch {BatchId}", batch.BatchId);

            return Ok(batch);
        }

        /// <summary>
        /// Returns the status of the processing of the given batch id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ActionNames.GetBatchesProcessingStatus)]
        public async Task<IActionResult> GetBatchesProcessingStatus(string id, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out _)) return BadRequest(Problem("This is not a guid"));

            var status = await _geoIpProjectService.GetBatchProcessingStatusAsync(id, cancellationToken);

            if (status == null) return NotFound();

            return Ok(status);
        }

        private static ProblemDetails Problem(string detail) => new ProblemDetails { Title = "Invalid request", Detail = detail, Status = StatusCodes.Status400BadRequest };
    }
}
