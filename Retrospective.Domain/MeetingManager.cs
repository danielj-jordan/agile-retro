using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

using Microsoft.Extensions.Logging;
using DBModel=Retrospective.Data.Model;
using DomainModel=Retrospective.Domain.Model;
using Retrospective.Data;
using Retrospective.Domain.ModelExtensions;

namespace Retrospective.Domain
{
    public class MeetingManager:BaseManager, IMeetingManager
    {
         private readonly ILogger<MeetingManager>  _logger;
        private readonly IDatabase database;

        public MeetingManager(ILogger<MeetingManager> logger,  IDatabase database)
        :base(logger, database){
            _logger=logger;
            this.database=database;
        }


         public List<DomainModel.Meeting> GetMeetingsForTeam(string activeUser, string teamId)
        {
            var team =this.GetTeam(teamId);
            if (!IsTeamMember(activeUser, team))
            {
                throw new Exception.AccessDenied();
            }

            _logger.LogDebug("looking for {0}", teamId);
            var meetings = database.Meetings.GetMeetings(teamId);
            return meetings.Select(m => m.ToDomainModel()).ToList();
        }


        public DomainModel.Meeting GetMeeting(string activeUser, string meetingId)
        {
             _logger.LogDebug("looking for meetingId {0}", meetingId);
            var meeting = database.Meetings.Get(meetingId);

            var team =this.GetTeam(meeting.TeamId.ToString());
            if (!IsTeamMember(activeUser, team))
            {
                throw new Exception.AccessDenied();
            }

            return meeting.ToDomainModel();
        }

        public DomainModel.Meeting SaveMeeting(string activeUser, DomainModel.Meeting meeting)
        {
            //is this an existing meeting?
            if(!string.IsNullOrEmpty(meeting.Id))
            {
                var currentMeeting = database.Meetings.Get(meeting.Id);

                var team =this.GetTeam(currentMeeting.TeamId.ToString());
                if (!IsTeamOwner(activeUser, team))
                {
                    throw new Exception.AccessDenied();
                }

                //can't change the team that change this meeting
                if(meeting.TeamId!=currentMeeting.TeamId.ToString())
                {
                     throw new Exception.AccessDenied();
                }
            }
            else{
                //active user must be owner of the team
                var team =this.GetTeam(meeting.TeamId);
                if (!IsTeamOwner(activeUser, team))
                {
                    throw new Exception.AccessDenied();
                }
            }

            DBModel.Meeting dbMeeting= meeting.ToDBModel();
            var dbMeetingSaved = database.Meetings.Save(dbMeeting);
            return dbMeetingSaved.ToDomainModel();
        
        }
        
    }
}