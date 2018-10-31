using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using AutoMapper;
using DomainModel = Retrospective.Domain.Model;
using Retrospective.Data;
using DBModel= Retrospective.Data.Model;

namespace Retrospective.Domain
{
    public class TeamManager
    {
        private readonly ILogger<TeamManager>  _logger;
        private readonly IMapper _mapper;
        
        private readonly Database database;

        public TeamManager(ILogger<TeamManager> logger, IMapper mapper,Database database)
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

            return false;

        }


        public List<DomainModel.User> GetTeamMembers(string activeUser, string teamId)
        {
            //will check for access here
            var team = this.GetTeam(activeUser,teamId);

            var users =  database.Users.GetTeamUsers(teamId);

            return _mapper.Map<List<DBModel.User>,List<DomainModel.User> >(users);
        }

        public List<DomainModel.Team> GetUserTeams(string activeUser, string user)
        {

            _logger.LogDebug("looking for {0}", user);
            var teams = database.Teams.GetUserTeams(user);

            var domainTeams= _mapper.Map<List<DBModel.Team>,List<DomainModel.Team> >(teams);

            foreach(var team in domainTeams)
            {
                if(!ReadOnlyAccessAllowed(activeUser, team))
                domainTeams.Remove(team);
            }
            
            _logger.LogDebug("db returning {0} teams for the user", teams.Count);

            return (domainTeams);


        }

        /// <summary>
        /// returns the team for this team id, along with users belonging to it
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public DomainModel.Team GetTeam(string activeUser, string teamId){
             _logger.LogDebug("looking for {0}", teamId);

            var dbteam = database.Teams.Get(teamId);

            var team=_mapper.Map<DBModel.Team,DomainModel.Team> (dbteam);

            //confirm that this user is a member of the team
            if (this.ReadOnlyAccessAllowed(activeUser, team)==false)
            {
                throw new Exception.AccessDenied();
            }
                
            return team;
        }


        public DomainModel.Team SaveTeam(string activeUser, DomainModel.Team team)
        {

            _logger.LogDebug("saving team  {0}", team.TeamId);


            if(!String.IsNullOrEmpty(team.TeamId)   ){
                //will check for access here
                var workingTeam = this.GetTeam(activeUser,team.TeamId);

                if(!WriteAccessAllowed(activeUser,workingTeam)){
                    throw new Exception.AccessDenied();
                }
            }
            else{
                team.Owner=activeUser;
            }

            var dbTeamSaved = database.Teams.Save(_mapper.Map<DomainModel.Team, DBModel.Team>(team));
            return _mapper.Map<DBModel.Team,DomainModel.Team> (dbTeamSaved);

            
        }

    

    }
}
