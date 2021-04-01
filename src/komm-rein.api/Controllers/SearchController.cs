using komm_rein.api.Services;
using komm_rein.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<SearchController> _logger;
        private readonly ISearchService<Facility> _service;

        public SearchController(
            ISearchService<Facility> service,
            ILogger<SearchController> logger
            )
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{searchstring}")]
        public async Task<ActionResult<List<Facility>>> Get(string searchstring)
        {
            return await _service.Search(searchstring);
        }

    }
}
