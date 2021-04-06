﻿using System;
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
    public class VisitController : ControllerBase
    {
        private readonly ILogger<VisitController> _logger;
        private readonly IVisitService _service;

        public VisitController(
            IVisitService service,
            ILogger<VisitController> logger
            )
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Visit>>> Get()
        {
            try
            {
                return await _service.GetAll(User.Sid());
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Bad request in GET Visits");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Visit>> Get(Guid id)
        {
            try
            {
                return await _service.GetById(id, User.Sid());
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Bad request in GET Visits, id: {id}");
                return BadRequest();
            }
        }

        [HttpPut("cancel/{id}")]
        public async Task<ActionResult<Visit>> Cancel(Guid id)
        {
            try
            {
                await _service.Cancel(id, User.Sid());

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Bad request in PUT Visits/Cancel, id: {id}");
                return BadRequest();
            }
        }

        [HttpPost("{name}/{from}/{to}/{pax}/{kids}")]
        public async Task<ActionResult<Signed<Visit>>> Post(string name, DateTime from, DateTime to, int pax, int kids)
        {
            try
            {
                return await _service.BookVisit(name, from, to, pax, kids, User.Sid());
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Bad request in POST Visit");
                return BadRequest();
            }
        }
    }
}
