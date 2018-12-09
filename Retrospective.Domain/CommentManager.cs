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
    public class CommentManager : BaseManager {
        private readonly ILogger<CommentManager> logger;
        private readonly IMapper mapper;
        private readonly Database database;

        public CommentManager (ILogger<CommentManager> logger, IMapper mapper, Database database) : base (logger, mapper, database) {
            this.logger = logger;
            this.mapper = mapper;
            this.database = database;
        }

        public List<DomainModel.Comment> GetComments (string activeUser, string meetingId) {
            var meeting = this.GetMeeting (meetingId);
            var team = this.GetTeam (meeting.TeamId);
            if (!IsTeamMember (activeUser, team)) {
                throw new Exception.AccessDenied ();
            }

            return mapper.Map<List<DBModel.Comment>, List<DomainModel.Comment>> (database.Comments.GetComments (meetingId));
        }

        public List<DomainModel.Category> GetCategories (string activeUser, string meetingId) {
            var meeting = this.GetMeeting (meetingId);
            var team = this.GetTeam (meeting.TeamId);
            if (!IsTeamMember (activeUser, team)) {
                throw new Exception.AccessDenied ();
            }

            return meeting.Categories.ToList ();
        }

        public void DeleteComment (string activeUser, string commentId) {
            var comment = this.GetComment (commentId);
            var meeting = this.GetMeeting (comment.MeetingId);
            var team = this.GetTeam (meeting.TeamId);
            if (!IsTeamMember (activeUser, team)) {
                throw new Exception.AccessDenied ();
            }

            database.Comments.Delete (new ObjectId (commentId));
        }

        public DomainModel.Comment SaveComment (string activeUser, DomainModel.Comment comment) {
            var meeting = this.GetMeeting (comment.MeetingId);
            var team = this.GetTeam (meeting.TeamId);
            Console.WriteLine (team.Owner + team.Name);
            if (!IsTeamMember (activeUser, team)) {
                throw new Exception.AccessDenied ();
            }

            return mapper.Map<DBModel.Comment, DomainModel.Comment> (
                database.Comments.SaveComment (
                    mapper.Map<DomainModel.Comment, DBModel.Comment> (comment)
                )
            );
        }

    }
}