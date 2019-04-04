using System.Collections.Generic;
using Retrospective.Data.Model;

namespace Retrospective.Data
{
  public interface IDataTeam
  {
    Team Save(Team team);

    Team Get(string teamId);

    List<Team> GetUserTeams(string email);

    List<Team> GetOwnedTeams(string email);
  }
}