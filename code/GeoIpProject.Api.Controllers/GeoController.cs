using GeoIpProject.Api.Controllers;
using GeoIpProject.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace GeoIpProject.Features.Controllers
{
    /// <summary>
    /// Geo Controller
    /// </summary>
    [ApiController]
    [Route(ControllerNames.GetIpInfo)]
    [Produces(MediaTypeNames.Application.Json)]
    public class GeoController : ControllerBase
    {
        private readonly IGeoIpProjectService _geoIpService;
        private readonly ILogger<GeoController> _logger;

        /// <summary>
        /// GeoController constructor
        /// </summary>
        /// <param name="geoIpService"></param>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public GeoController(IGeoIpProjectService geoIpService, ILogger<GeoController> logger)
        {
            _geoIpService = geoIpService ?? throw new ArgumentNullException(nameof(geoIpService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Returns the information regarding the given ip
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ActionNames.GetIpInfo)]
        [ProducesResponseType(typeof(GetIpInfoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> GetIpInfo(string ip, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Lookup request for {Ip} start", ip);

            var resp = await _geoIpService.GetIpInfoAsync(ip, cancellationToken);

            _logger.LogInformation("Lookup request finish");
            return Ok(resp);
        }
    }
}
