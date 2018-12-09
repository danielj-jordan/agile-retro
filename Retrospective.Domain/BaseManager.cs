using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using DBModel = Retrospective.Data.Model;
using DomainModel = Retrospective.Domain.Model;
using Retrospective.Data;

namespace Retrospective.Domain {
    public class BaseManager {
        private readonly ILogger<BaseManager> logger;
        private readonly IMapper mapper;
        private readonly Database database;

        protected BaseManager (ILogger<BaseManager> logger, IMapper mapper, Database database) {
            this.logger = logger;
            this.mapper = mapper;
            this.database = database;
        }

        protected static bool IsTeamMember (string activeUser, DomainModel.Team team) {
            //confirm that this user is a member of the team
            if (team.Owner.Equals (activeUser, StringComparison.OrdinalIgnoreCase)) {
                return true;
            }

            if(team.Members==null)return false;
           /*  var found = Array.Exists (team.Members,
                member => member.UserName.Equals (activeUser, StringComparison.OrdinalIgnoreCase)
                );
            */
            System.Console.WriteLine("activeUser:" + activeUser + " " + team.Members.ToList().Count);

            var found= from member in team.Members 
                 where  member.UserName==activeUser
                select member;

            if (found.ToList().Count>0) return true;

            return false;

        }

        protected static bool IsTeamOwner (string activeUser, DomainModel.Team team) {
            //confirm that this user is a member of the team
            if (team.Owner.Equals (activeUser, StringComparison.OrdinalIgnoreCase)) {
                return true;
            }

            return false;

        }

        protected DomainModel.Team GetTeam (string teamId) {

            if (String.IsNullOrEmpty (teamId)) {
                throw new System.ArgumentException ();
            }
            logger.LogDebug ("looking for {0}", teamId);

            var dbteam = database.Teams.Get (teamId);
            var team = mapper.Map<DBModel.Team, DomainModel.Team> (dbteam);
            return team;
        }

        protected DomainModel.Meeting GetMeeting (string meetingId) {
            logger.LogDebug ("looking for meeting {0}", meetingId);

            var dbMeeting = database.Meetings.Get (meetingId);

            var meeting = mapper.Map<DBModel.Meeting, DomainModel.Meeting> (dbMeeting);

            return meeting;
        }

        protected DomainModel.Comment GetComment (string commentId) {
            logger.LogDebug ("looking for meeting {0}", commentId);

            var comment = database.Comments.GetComment (new ObjectId (commentId));

            return mapper.Map<DBModel.Comment, DomainModel.Comment> (comment);
        }
    }
}