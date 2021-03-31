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
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FacilityController : ControllerBase
    {
        private readonly ILogger<FacilityController> _logger;
        private readonly IFacilityService _service;

        public FacilityController(
            IFacilityService service,
            ILogger<FacilityController> logger
            )
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{name}")]
        [AllowAnonymous]
        public async Task<ActionResult<Facility>> Get(string name)
        {
            try
            {
                return await _service.GetByName(name);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Bad request in GET Facility: {name}");
                return BadRequest();
            }
        }
                
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Facility>>> Get()
        {
            try
            {
                return await _service.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Bad request in GET Facilities");
                return BadRequest();
            }
        }

        [HttpGet("{id}/slots")]
        [AllowAnonymous]
        public async Task<ActionResult<Slot[]>> GetSlots(Guid id, DateTime day, [FromBody] Visit visit)
        {
            try
            {
                return await _service.GetSlotsForVisit(id, day, visit);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Bad request in GET Facility/Slots, id: {id}");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Facility>> Post([FromBody] Facility value)
        {
            try
            {
                return await _service.Create(value, User.Sid());
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex, "Bad Request in POST Facility");
                return BadRequest();
            }
        }

        [HttpPut("{id}/settings")]
        public async Task<ActionResult<FacilitySettings>> Settings(Guid id, [FromBody] FacilitySettings value)
        {
            try
            {
               return  await _service.SetSettings(id, value, User.Sid());
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Bad request in GET Facilities");
                return BadRequest();
            }
        }

        [HttpGet("{id}/settings")]
        public async Task<ActionResult<FacilitySettings>> Settings(Guid id)
        {
            try
            {
                return await _service.GetSettings(id, User.Sid());
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Bad request in GET Facility/Settings, id: {id}");
                return BadRequest();
            }
        }


        [HttpGet("{id}/openinghours")]
        [AllowAnonymous]
        public async Task<ActionResult<OpeningHours[]>> OpeningHours(Guid id)
        {
            try
            {
                return (await _service.GetOpeningHours(id)).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Bad request in GET Facility/Openinghours, id: {id}");
                return BadRequest();
            }
        }

        [HttpPut("{id}/openinghours")]
        public async Task<ActionResult<OpeningHours[]>> OpeningHours([FromBody] OpeningHours[] value, Guid id)
        {
            try
            {
                return (await _service.SetOpeningHours(id, value, User.Sid())).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Bad request in PUT Facility/Openinghours, id: {id}");
                return BadRequest();
            }
        }
    }
}
