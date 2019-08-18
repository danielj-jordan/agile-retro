using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using DBModel = Retrospective.Data.Model;
using DomainModel = Retrospective.Domain.Model;
using Retrospective.Data;
using Retrospective.Domain.ModelExtensions;

namespace Retrospective.Domain {
    public class BaseManager {
        private readonly ILogger<BaseManager> logger;
        private readonly IDatabase database;

        protected BaseManager (ILogger<BaseManager> logger, IDatabase database) {
            this.logger = logger;
            this.database = database;
        }


        protected DomainModel.TeamRole GetTeamRole(DomainModel.Team team, string userId)
        {
            var teamMember = team.Members.Where(tm => tm.UserId == userId).First();
            return teamMember.Role;
        }

        protected bool IsTeamMember (string activeUserId, DomainModel.Team team) 
        {
            if(team.Members==null)return false;
         
            //confirm that this user is a member of the team
            System.Console.WriteLine("activeUser:" + activeUserId + " " + team.Members.ToList().Count);

            var found= from member in team.Members 
                 where  member.UserId==activeUserId
                select member;

            if (found.ToList().Count>0) return true;

            return false;

        }

        protected bool IsTeamOwner (string activeUserId, DomainModel.Team team) {
            //confirm that this user is a member of the team
            if(team.Members==null)return false;

            //confirm that this user is a member of the team
            if (DomainModel.TeamRole.Owner == this.GetTeamRole(team, activeUserId)) 
            {
                return true;
            }

            return false;
        }

        protected bool isScrumMaster (string activeUserId, DomainModel.Team team) {
            //confirm that this user is a member of the team
            if(team.Members==null)return false;

            //confirm that this user is a member of the team
            if (DomainModel.TeamRole.ScrumMaster == this.GetTeamRole(team, activeUserId)) 
            {
                return true;
            }
            return false;
        }

        protected bool isStakeHolder (string activeUserId, DomainModel.Team team) {
            //confirm that this user is a member of the team
            if(team.Members==null)return false;

            //confirm that this user is a member of the team
            if (DomainModel.TeamRole.Stakeholder == this.GetTeamRole(team, activeUserId)) 
            {
                return true;
            }
            return false;
        }

        protected DomainModel.Team GetTeam (string teamId) {

            if (String.IsNullOrEmpty (teamId)) {
                throw new System.ArgumentException ();
            }
            logger.LogDebug ("looking for {0}", teamId);

            var dbteam = database.Teams.Get(teamId);
            return dbteam.ToDomainModel();
        }

        protected DomainModel.Meeting GetMeeting (string meetingId) {
            logger.LogDebug ("looking for meeting {0}", meetingId);

            var dbMeeting = database.Meetings.Get (meetingId);

            return dbMeeting.ToDomainModel();
        }

        protected DomainModel.Comment GetComment (string commentId) {
            logger.LogDebug ("looking for meeting {0}", commentId);

            var comment = database.Comments.GetComment (new ObjectId (commentId));
            return comment.ToDomainModel();
        }

        public DomainModel.User GetUser (string userId) {
            logger.LogDebug ("looking for userid {0}", userId);

            var user = database.Users.Get(new ObjectId (userId));
            return user.ToDomainModel();
        }

    }
}