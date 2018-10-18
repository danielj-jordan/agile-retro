using System;
using System.Collections.Generic;
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

        public List<DomainModel.Team> GetUserTeams(string email)
        {

         
            _logger.LogDebug("looking for {0}", email);
            var teams = database.Teams.GetUserTeams(email);
            
            _logger.LogDebug("db returning {0} teams for the user", teams.Count);

            return (_mapper.Map<List<DBModel.Team>,List<DomainModel.Team> >(teams));


        }

    }
}
