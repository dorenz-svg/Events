using Events.Models.Query;
using Events.Models.Repositories.Abstract;
using Events.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Events.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository repository;
        public EventsController(IEventRepository repo) => repository = repo;
        [HttpGet]
        public async Task<ActionResult<EventsResponse>> GetEvents()
        {
            return Ok(await repository.GetListEvents(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
        }
        [HttpPost("create")]
        public async Task<ActionResult<string>> CreateEvent(string name)
        {
            if (name is null)
                return BadRequest("The Name field is required.");
            return Ok(await repository.CreateEvent(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, name));
        }
        [HttpPost("addDates")]
        public async Task<ActionResult> AddDates([FromBody]AddDatesQuery query)
        {
            await repository.AddVisitorAndDates((long)query.IdEvent, User.FindFirst(ClaimTypes.NameIdentifier)?.Value, query.Dates);
            return Ok();
        }
        [HttpGet("getDate")]
        public async Task<ActionResult<object>> GetTime(long idEvent)
        {
            if (idEvent == 0)
                return BadRequest("The IdEvent field is required.");
            var temp = await repository.KnowTimeEvent(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, idEvent);
            return Ok(new { DateBegin = temp.Item1, DateEnd = temp.Item2 });
        }
    }
}
