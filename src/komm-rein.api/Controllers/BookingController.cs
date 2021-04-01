using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using komm_rein.model;
using komm_rein.api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace komm_rein.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IFacilityService _service;

        public BookingController(
            IFacilityService service,
            ILogger<BookingController> logger
            )
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost("{facilityId}/{day}")]
        public async Task<ActionResult<Slot[]>> Get(Guid facilityId, DateTime day,[FromBody] Visit visitRequest)
        {
            try
            {
                return await _service.GetSlotsForVisit(facilityId, day, visitRequest);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Bad request in GET Visits");
                return BadRequest();
            }
        }
    }
}
