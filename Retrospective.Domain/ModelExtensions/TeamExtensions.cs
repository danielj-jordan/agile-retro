using System;
using System.Linq;
using MongoDB.Bson;
using DomainModel = Retrospective.Domain.Model;
using DBModel = Retrospective.Data.Model;

namespace Retrospective.Domain.ModelExtensions
{
  public static class TeamExtensions
  {
    public static DBModel.Team ToDBModel(
    this Retrospective.Domain.Model.Team team)
    {
      DBModel.Team dbTeam = new DBModel.Team
      {
        Id =  !string.IsNullOrWhiteSpace(team.TeamId)? ObjectId.Parse(team.TeamId): (ObjectId?)null ,
        Name = team.Name,
        Members = team.Members?.Select(m => new DBModel.TeamMember
        {
          UserId = ObjectId.Parse(m.UserId),
          RemoveDate = m.RemoveDate,
          StartDate = m.StartDate,
          Role = (DBModel.TeamRole)Enum.Parse(typeof(DBModel.TeamRole), m.Role.ToString())
        }).ToArray(),
        Invited = team.Invited?.Select(i => new DBModel.Invitation
        {
          Name = i.Name,
          Email = i.Email,
          InviteDate = i.InviteDate,
          Role = (DBModel.TeamRole)Enum.Parse(typeof(DBModel.TeamRole), i.Role.ToString())
        }).ToArray()
      };

      return dbTeam;
    }


    public static DomainModel.Team ToDomainModel(
     this DBModel.Team team)
    {
      DomainModel.Team domainTeam = new DomainModel.Team
      {
        TeamId = team.Id.ToString(),
        Name = team.Name,
        Members = team.Members?.Select(m => new DomainModel.TeamMember
        {
          UserId = m.UserId.ToString(),
          RemoveDate = m.RemoveDate,
          StartDate = m.StartDate,
          Role = (DomainModel.TeamRole)Enum.Parse(typeof(DomainModel.TeamRole), m.Role.ToString())
        }).ToArray(),
        Invited = team.Invited?.Select(i => new DomainModel.Invitation
        {
          Name = i.Name,
          Email = i.Email,
          InviteDate = i.InviteDate,
          Role = (DomainModel.TeamRole)Enum.Parse(typeof(DomainModel.TeamRole), i.Role.ToString())
        }).ToArray()

      };

      return domainTeam;
    }
  }
}