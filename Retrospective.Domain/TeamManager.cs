using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using DomainModel = Retrospective.Domain.Model;
using Retrospective.Data;
using DBModel = Retrospective.Data.Model;

namespace Retrospective.Domain {
    public class TeamManager : BaseManager {
        private readonly ILogger<TeamManager> logger;
        private readonly IMapper mapper;
        private readonly Database database;

        public TeamManager (ILogger<TeamManager> logger, IMapper mapper, Database database) : base (logger, mapper, database) {
            this.logger = logger;
            this.mapper = mapper;
            this.database = database;
        }

        public List<DomainModel.User> GetTeamMembers (string activeUser, string teamId) {
            //will check for access here
            var team = GetTeam (activeUser, teamId);

            var users = database.Users.GetTeamUsers (teamId);

            return mapper.Map<List<DBModel.User>, List<DomainModel.User>> (users);
        }

        public List<DomainModel.Team> GetUserTeams (string activeUser, string user) {

            logger.LogDebug ("looking for {0}", user);
            var teams = database.Teams.GetUserTeams (user);

            var domainTeams = mapper.Map<List<DBModel.Team>, List<DomainModel.Team>> (teams);

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

            var team = mapper.Map<DBModel.Team, DomainModel.Team> (dbteam);

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
            } else {
                team.Owner = activeUser;
            }

            var dbTeamSaved = database.Teams.Save (mapper.Map<DomainModel.Team, DBModel.Team> (team));
            return mapper.Map<DBModel.Team, DomainModel.Team> (dbTeamSaved);
        }
    }
}