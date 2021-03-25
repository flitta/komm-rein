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

namespace komm_rein.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacilityController : ControllerBase
    {
        private readonly ILogger<FacilityController> _logger;
        private readonly IFacilityService _service;
        
        public FacilityController(IFacilityService service,  ILogger<FacilityController> logger)
        {
            _logger = logger;
            _service = service;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<Facility> Get()
        {
            return new []{ new Facility { } };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public Facility Post([FromBody] Facility value)
        {
            var sid = User.Claims.First(x => x.Type == ClaimTypes.Sid);
            
            _service.Create(value, sid.Value);

            return value;
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Facility value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}
