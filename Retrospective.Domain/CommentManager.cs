using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MongoDB.Bson;

using Microsoft.Extensions.Logging;
using DBModel=Retrospective.Data.Model;
using DomainModel=Retrospective.Domain.Model;
using Retrospective.Data;

namespace Retrospective.Domain
{
    public class CommentManager
    {
         private readonly ILogger<CommentManager>  _logger;
        private readonly IMapper _mapper;
        private readonly Database database;

        public CommentManager(ILogger<CommentManager> logger, IMapper mapper,Database database)
        {
            _logger=logger;
            _mapper=mapper;
            this.database=database;
        }



        private bool ReadOnlyAccessAllowed(string activeUser, DomainModel.Team team){
            //confirm that this user is a member of the team
            if(team.Owner.Equals(activeUser, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            var found = Array.Exists(team.TeamMembers, 
                member => member.Equals(activeUser, StringComparison.OrdinalIgnoreCase));

            if(found) return true;

            return false;

        }

        private bool WriteAccessAllowed(string activeUser, DomainModel.Team team){
            //confirm that this user is a member of the team
            if(team.Owner.Equals(activeUser, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            var found = Array.Exists(team.TeamMembers, 
                member => member.Equals(activeUser, StringComparison.OrdinalIgnoreCase));

            if(found) return true;

            return false;

        }

        private DomainModel.Team GetTeam(string teamId){

            if(String.IsNullOrEmpty(teamId)){
                throw new System.ArgumentException();
            }
             _logger.LogDebug("looking for {0}", teamId);

            var dbteam = database.Teams.Get(teamId);
            var team=_mapper.Map<DBModel.Team,DomainModel.Team> (dbteam);
            return team;
        }

        private DomainModel.Meeting GetMeeting (string meetingId)
        {
            _logger.LogDebug("looking for meeting {0}", meetingId);

            var dbMeeting = database.Meetings.Get(meetingId);

            var meeting=_mapper.Map<DBModel.Meeting,DomainModel.Meeting> (dbMeeting);
        
            return meeting;
        }

        private DomainModel.Comment GetComment (string commentId)
        {
            _logger.LogDebug("looking for meeting {0}", commentId);

            var comment = database.Comments.GetComment(new ObjectId(commentId));

            return _mapper.Map<DBModel.Comment,DomainModel.Comment> (comment);
        }

        public List<DomainModel.Comment> GetComments (string activeUser, string meetingId)
        {
            var meeting = this.GetMeeting( meetingId);
            var team =this.GetTeam(meeting.TeamId);
            if (!AccessRules.IsTeamMember(activeUser, team))
            {
                throw new Exception.AccessDenied();
            }

            return _mapper.Map<List<DBModel.Comment>, List<DomainModel.Comment>>(database.Comments.GetComments(meetingId));
        }

        public List<DomainModel.Category> GetCategories(string activeUser, string meetingId)
        {
            var meeting = this.GetMeeting( meetingId);
            var team =this.GetTeam(meeting.TeamId);
            if (!AccessRules.IsTeamMember(activeUser, team))
            {
                throw new Exception.AccessDenied();
            }

            return meeting.Categories.ToList();
        }

        public void DeleteComment  (string activeUser, string commentId)
        {
            var comment = this.GetComment(commentId);
            var meeting = this.GetMeeting( comment.MeetingId);
            var team =this.GetTeam(meeting.TeamId);
            if (!AccessRules.IsTeamMember(activeUser, team))
            {
                throw new Exception.AccessDenied();
            }

            database.Comments.Delete(new ObjectId(commentId));
        }

        public DomainModel.Comment SaveComment(string activeUser, DomainModel.Comment comment)
        {
            var meeting = this.GetMeeting( comment.MeetingId);
            var team = this.GetTeam(meeting.TeamId);
            Console.WriteLine(team.Owner + team.Name);
             if (!AccessRules.IsTeamMember(activeUser, team))
            {
                throw new Exception.AccessDenied();
            }

            return _mapper.Map<DBModel.Comment, DomainModel.Comment>(
                database.Comments.SaveComment(
                    _mapper.Map<DomainModel.Comment, DBModel.Comment>(comment)
                )
            );
        }

    }
}