
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

using app.Model;
using Retrospective.Domain;
using DomainModel=Retrospective.Domain.Model;
using app.ModelExtensions;

namespace app.Controllers
{
    [Route("api/[controller]")]
    public class MeetingController: Controller
    {
        private readonly ILogger<MeetingController>  _logger;
        private readonly IMeetingManager manager;

        public MeetingController(ILogger<MeetingController> logger, 
             IMeetingManager manager)
        {
            _logger=logger;
            this.manager=manager;
        }
        
        private string GetActiveUserId(){
            return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        
        /// <summary>
        /// returns the retrospective meetings for a team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("[action]/{id}")]
        public ActionResult<IEnumerable<Meeting>> Meetings(string id)
        {
            if(string.IsNullOrEmpty(id)){
                _logger.LogWarning("no team id supplied");
                return new BadRequestResult();
            }

            var meetings= manager.GetMeetingsForTeam(GetActiveUserId(), id);  
            return meetings.Select(m => m.ToViewModel()).ToList();

        }

        /// <summary>
        /// returns the retrospective meetings for this id
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("[action]/{id}")]
        public ActionResult<Meeting> Meeting(string id)
        {
            if(string.IsNullOrEmpty(id)){
                _logger.LogWarning("no meeting id supplied");
                return new BadRequestResult();
            }

            var meeting = manager.GetMeeting(GetActiveUserId(), id);
            return meeting.ToViewModel();
        }



        /// <summary>
        /// saves the retrospective meeting
        /// </summary>
        /// <param name="meeting"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("[action]")]
        public ActionResult<Meeting> Meeting([FromBody] Meeting meeting)
        {
            if(string.IsNullOrEmpty(meeting.TeamId)){
                _logger.LogWarning("no team id supplied for meeting");
                return new BadRequestResult();
            }

            _logger.LogDebug($"saving meeting id: {meeting.Id}");
            var saved = manager.SaveMeeting(GetActiveUserId(),meeting.ToDomainModel());
            
            return saved.ToViewModel();

        }

    }
}