using Events.Models.Repositories.Abstract;
using Events.Models.Request;
using Events.Models.Response;
using Events.Response;
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

        [HttpPost("{eventId:long}/user")]
        public async Task<ActionResult> AddUser([FromRoute][Range(1, long.MaxValue)] long eventId, AddDatesRequest query)
        {
            await _repository.AddVisitorAndDates(eventId, UserId, query.Dates);
            return Ok();
        }

        [HttpGet("{eventId:long}/date")]
        public async Task<ActionResult<EventDateResponse>> GetEventDate([FromRoute][Range(1, long.MaxValue)] long eventId)
        {
            var date = await _repository.GetEventDate(UserId, eventId);
            return Ok(new EventDateResponse{ DateBegin = date.DateBegin, DateEnd = date.DateEnd });
        }
    }
}
