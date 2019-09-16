using System.Collections.Generic;
using DomainModel = Retrospective.Domain.Model;

namespace Retrospective.Domain
{
  public interface ITeamManager: IBaseManager
  {
    List<DomainModel.User> GetTeamMembers(string activeUser, string teamId);
    List<DomainModel.Team> GetUserTeams(string activeUser, string userId);
    List<DomainModel.Team> GetUserInvitedTeams(string activeUser, string email);
    DomainModel.Team AcceptInvitation(string activeUser, string teamId, string email);
    DomainModel.Team GetTeam(string activeUser, string teamId);
    DomainModel.Team SaveTeam(string activeUser, DomainModel.Team team);
    DomainModel.Team NewTeam(string activeUser);
    int OwnedTeamCount(string activeUser);

  }
}