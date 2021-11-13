using Events.Models.Request;
using Events.Models.Repositories.Abstract;
using Events.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        private readonly IEventRepository _repository;
        public EventsController(IEventRepository repository) => _repository = repository;

        [HttpGet]
        public async Task<ActionResult<EventsResponse>> GetEvents()
        {
            return Ok(await _repository.GetListEvents(UserId));
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateEvent([Required] string nameEvent)
        {
            return Ok(await _repository.CreateEvent(UserId, nameEvent));
        }

        [HttpPost("{idEvent:long}/addUser")]
        public async Task<ActionResult> AddUser([FromRoute][Range(1, long.MaxValue)] long eventId, AddDatesRequest query)
        {
            if (query is null)
                return BadRequest();
            await _repository.AddVisitorAndDates(eventId, UserId, query.Dates);
            return Ok();
        }

        [HttpGet("getEventDate")]
        public async Task<ActionResult<object>> GetEventDate([Range(1, long.MaxValue)] long eventId)
        {
            var EventDate = await _repository.GetEventDate(UserId, eventId);
            return Ok(new { DateBegin = EventDate.DateBegin, DateEnd = EventDate.DateBegin });
        }
    }
}
