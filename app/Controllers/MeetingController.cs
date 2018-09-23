
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using app.Model;
using DBModel=Retrospective.Data.Model;
using AutoMapper;
using Retrospective.Data;

namespace app.Controllers
{
    [Route("api/[controller]")]
    public class MeetingController: Controller
    {
        

        private readonly ILogger<MeetingController>  _logger;
        private readonly IMapper _mapper;
        
        private readonly Database database;

        public MeetingController(ILogger<MeetingController> logger, IMapper mapper,Database database)
        {
            _logger=logger;
            _mapper=mapper;
            this.database=database;
        }
        
        
        /// <summary>
        /// returns the retrospective meetings for a team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        public ActionResult<IEnumerable<Meeting>> Meetings(string id)
        {
            if(string.IsNullOrEmpty(id)){
                _logger.LogWarning("no team id supplied");
                return new BadRequestResult();
            }

            _logger.LogDebug("looking for {0}", id);
            var meetings = database.Meetings.GetMeetings(id);
            
            _logger.LogDebug("db returning {0} teams for the user", meetings.Count);

            return (_mapper.Map<List<DBModel.Meeting>,List<app.Model.Meeting> >(meetings)).ToList();

        }

        /// <summary>
        /// returns the retrospective meetings for this id
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        public ActionResult<Meeting> Meeting(string id)
        {
            if(string.IsNullOrEmpty(id)){
                _logger.LogWarning("no meeting id supplied");
                return new BadRequestResult();
            }

            _logger.LogDebug("looking for {0}", id);
            var meetings = database.Meetings.Get(id);
            
            return (_mapper.Map<DBModel.Meeting,app.Model.Meeting> (meetings));

        }



        /// <summary>
        /// saves the retrospective meeting
        /// </summary>
        /// <param name="meeting"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ActionResult<Meeting> Meeting([FromBody] Meeting meeting)
        {
            if(string.IsNullOrEmpty(meeting.TeamId)){
                _logger.LogWarning("no team id supplied for meeting");
                return new BadRequestResult();
            }

            _logger.LogDebug($"saving meeting id: {meeting.Id}");
            var saved = database.Meetings.Save(_mapper.Map<app.Model.Meeting, DBModel.Meeting>(meeting));
            
            
            return (_mapper.Map<DBModel.Meeting,app.Model.Meeting> (saved));

        }

    }
}