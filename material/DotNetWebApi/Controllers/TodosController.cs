using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNetWebApi.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class TodosController : Controller
    {
        private readonly ILogger<TodosController> _logger;

        public TodosController(ILogger<TodosController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
            => Ok(new object[] { 1, "2", DateTime.Now });

        [HttpPost]
        public IActionResult Post([FromBody] string input)
        {
            return Created(nameof(Get), input);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] object input)
        {
            return Ok();
        }
    }
}