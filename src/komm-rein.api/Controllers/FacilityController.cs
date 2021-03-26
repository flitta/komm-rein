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

        [HttpPost]
        public async Task<ActionResult<Facility>> Post([FromBody] Facility value)
        {
            try
            {
                await _service.Create(value, User.Sid());
                return new Facility() { ID = value.ID, Name = value.Name };
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("/{id}/settings")]
        public async Task<ActionResult<FacilitySettings>> Settings(Guid id, [FromBody] FacilitySettings value)
        {
            try
            {
               return  await _service.SetSettings(id, value, User.Sid());
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/settings
        [HttpGet("/{id}/settings")]
        public async Task<ActionResult<FacilitySettings>> Settings(Guid id)
        {
            try
            {
                return await _service.GetSettings(id, User.Sid());
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet("/{id}/openinghours")]
        public async Task<ActionResult<OpeningHours[]>> OpeningHours(Guid id)
        {
            try
            {
                return (await _service.GetOpeningHours(id)).ToArray();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("/{id}/openinghours")]
        public async Task<ActionResult<OpeningHours[]>> OpeningHours([FromBody] OpeningHours[] value, Guid id)
        {
            try
            {
                return (await _service.SetOpeningHours(id, value, User.Sid())).ToArray();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
