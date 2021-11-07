using Events.Models.Query;
using Events.Models.Repositories.Abstract;
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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository repository;
        public EventsController(IEventRepository repo) => repository = repo;
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetEvents()
        {
            return Ok(await repository.GetListEvents(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
        }
        [HttpPost("createEvent")]
        public async Task<ActionResult<string>> CreateEvent(string name)
        {
            return Ok(await repository.CreateEvent(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, name));
        }
        [HttpPost("addDates")]
        public async Task<ActionResult> AddDates([FromBody]AddDatesQuery query)
        {
            await repository.AddVisitorAndDates(query.IdEvent, User.FindFirst(ClaimTypes.NameIdentifier)?.Value, query.Dates);
            return Ok();
        }
        [HttpGet("getDate")]
        public async Task<ActionResult<object>> GetTime(long idEvent)
        {
            var temp = await repository.KnowTimeEvent(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, idEvent);
            return Ok(new { DateBegin = temp.Item1, DateEnd = temp.Item2 });
        }
    }
}
