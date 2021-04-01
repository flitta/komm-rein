using komm_rein.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace komm_rein.api.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{searchstring}")]
        public ActionResult<List<Facility>> Get(string searchstring)
        {
            return new List<Facility> { new Facility() {Name = "Test 1" }, new Facility() { Name = "Test 2" }, new Facility() { Name = "Test 3" } };
        }

    }
}
