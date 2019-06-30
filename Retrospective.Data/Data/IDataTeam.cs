using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Retrospective.Data.Model;

namespace Retrospective.Data
{
  public interface IDataTeam
  {
    Team Save(Team team);

    Team Get(string teamId);

    List<Team> GetUserTeams(ObjectId userId);

    List<Team> GetUserTeams(string userId);

    List<Team> GetOwnedTeams(ObjectId userId);
  }
}