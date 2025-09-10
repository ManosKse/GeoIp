using FakeItEasy;
using FluentAssertions;
using GeoIpProject.Api.Services.Interfaces;
using GeoIpProject.Api.Services.Interfaces.Models;
using GeoIpProject.Features.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GeoIpProject.Api.Controllers.Tests
{
    internal class BatchControllerTests
    {
        private IGeoIpProjectService _geoIpService = null!;
        private LinkGenerator _linkGenerator = null!;
        private ILogger<BatchesController> _logger = null!;
        private BatchesController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _geoIpService = A.Fake<IGeoIpProjectService>();
            _linkGenerator = A.Fake<LinkGenerator>();
            _logger = A.Fake<ILogger<BatchesController>>();
            _controller = new BatchesController(_geoIpService, _linkGenerator, _logger);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Test]
        public async Task CreateBatch_ShouldReturnBadRequest_WhenNoIps()
        {
            // Arrange
            var req = new CreateBatchRequest { Ips = new List<string>() };

            // Act
            var result = await _controller.CreateBatch(req, CancellationToken.None);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task GetBatchesProcessingStatus_ShouldReturnBadRequest_WhenInvalidGuid()
        {
            // Arrange Act
            var result = await _controller.GetBatchesProcessingStatus("not-a-guid", CancellationToken.None);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task GetBatchesProcessingStatus_ShouldReturnNotFound_WhenServiceReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            GetBatchProcessingStatusResponse response = null;

            A.CallTo(() => _geoIpService.GetBatchProcessingStatusAsync(id, CancellationToken.None))
                .Returns(response);

            // Act
            var result = await _controller.GetBatchesProcessingStatus(id, CancellationToken.None);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task GetBatchesProcessingStatus_ShouldReturnOk_WhenServiceReturnsStatus()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var status = new GetBatchProcessingStatusResponse();

            A.CallTo(() => _geoIpService.GetBatchProcessingStatusAsync(id, CancellationToken.None))
                .Returns(status);

            // Act
            var result = await _controller.GetBatchesProcessingStatus(id, CancellationToken.None);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(status);
        }
    }
}
