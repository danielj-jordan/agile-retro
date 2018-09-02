
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
    public class SessionController: Controller
    {
        

        private readonly ILogger<SessionController>  _logger;
        private readonly IMapper _mapper;
        
        private readonly Database database;

        public SessionController(ILogger<SessionController> logger, IMapper mapper,Database database)
        {
            _logger=logger;
            _mapper=mapper;
            this.database=database;
        }
        
        
        /// <summary>
        /// returns the retrospective sessions for a team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        public ActionResult<IEnumerable<Session>> Meetings(string id)
        {
            if(string.IsNullOrEmpty(id)){
                _logger.LogWarning("no team id supplied");
                return new BadRequestResult();
            }

            _logger.LogDebug("looking for {0}", id);
            var meetings = database.Sessions.GetTeamRetrospectiveSessions(id);
            
            _logger.LogDebug("db returning {0} teams for the user", meetings.Count);

            return (_mapper.Map<List<DBModel.RetrospectiveSession>,List<app.Model.Session> >(meetings)).ToList();

        }
    }
}