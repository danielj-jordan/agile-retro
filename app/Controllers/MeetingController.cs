
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

using app.Model;
using Retrospective.Domain;
using DomainModel=Retrospective.Domain.Model;

namespace app.Controllers
{
    [Route("api/[controller]")]
    public class MeetingController: Controller
    {
        private readonly ILogger<MeetingController>  _logger;
        private readonly IMapper _mapper;
        private readonly MeetingManager manager;

        public MeetingController(ILogger<MeetingController> logger, IMapper mapper,
             MeetingManager manager)
        {
            _logger=logger;
            _mapper=mapper;
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
            return (_mapper.Map<List<DomainModel.Meeting>,List<app.Model.Meeting> >(meetings)).ToList();

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
            return (_mapper.Map<DomainModel.Meeting,app.Model.Meeting> (meeting));

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
            var saved = manager.SaveMeeting(GetActiveUserId(),_mapper.Map<app.Model.Meeting, DomainModel.Meeting>(meeting));
            
            
            return (_mapper.Map<DomainModel.Meeting,app.Model.Meeting> (saved));

        }

    }
}