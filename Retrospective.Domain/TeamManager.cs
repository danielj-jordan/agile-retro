using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using DomainModel = Retrospective.Domain.Model;
using Retrospective.Data;
using DBModel = Retrospective.Data.Model;
using Retrospective.Domain.ModelExtensions;

namespace Retrospective.Domain {
    public class TeamManager : BaseManager {
        private readonly ILogger<TeamManager> logger;
        private readonly IDatabase database;

        public TeamManager (ILogger<TeamManager> logger,  IDatabase database) : base (logger,  database) {
            this.logger = logger;
            this.database = database;
        }

        public List<DomainModel.User> GetTeamMembers (string activeUser, string teamId) {
            //will check for access here
            var team = GetTeam (activeUser, teamId);

            List<DBModel.User> teamUsers =  new List<DBModel.User>();
            foreach(var teamMember in team.Members)
            {
                var user = this.database.Users.Get(teamMember.UserId);
                teamUsers.Add(user);
            }
            return teamUsers.Select(u => u.ToDomainModel()).ToList<DomainModel.User>();
        }

        public List<DomainModel.Team> GetUserTeams (string activeUser, string userId) {

            logger.LogDebug ("looking for {0}", userId);
            var teams = database.Teams.GetUserTeams (userId);

            logger.LogDebug("{0} possible teams for {1}", teams.Count, userId);

            var domainTeams = teams.Select(t=> t.ToDomainModel()).ToList();

            foreach (var team in domainTeams) {
                if (!IsTeamMember (activeUser, team))
                    domainTeams.Remove (team);
            }

            logger.LogDebug ("db returning {0} teams for the user", teams.Count);

            return (domainTeams);
        }

        /// <summary>
        /// returns the team for this team id, along with users belonging to it
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public DomainModel.Team GetTeam (string activeUser, string teamId) {
            logger.LogDebug ("looking for {0}", teamId);

            var dbteam = database.Teams.Get (teamId);

            var team = dbteam.ToDomainModel();

            //confirm that this user is a member of the team
            if (!IsTeamMember (activeUser, team)) {
                throw new Exception.AccessDenied ();
            }

            return team;
        }

        public DomainModel.Team SaveTeam (string activeUser, DomainModel.Team team) {

            logger.LogDebug ("saving team  {0}", team.TeamId);

            if (!String.IsNullOrEmpty (team.TeamId)) {
                //will check for access here
                var workingTeam = this.GetTeam (activeUser, team.TeamId);

                if (!IsTeamOwner (activeUser, workingTeam)) {
                    throw new Exception.AccessDenied ();
                }
            } 

            var dbTeamSaved = database.Teams.Save (team.ToDBModel());
            return dbTeamSaved.ToDomainModel();
        }
    }
}