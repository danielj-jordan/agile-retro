using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel = Retrospective.Domain.Model;

namespace app.ModelExtensions
{
    public static class TeamExtension
    {

        public static app.Model.Team ToViewModel(
         this DomainModel.Team team)
         {
             var viewModelTeam = new app.Model.Team
             {
                TeamId= team.TeamId,
                Name= team.Name,
                Members= team.Members?.Select(m=> new app.Model.TeamMember
                {
                    UserId= m.UserId,
                    RemoveDate=m.RemoveDate,
                    StartDate=m.StartDate,
                    Role=m.Role.ToString()
                    
                }).ToArray(),
                Invited = team.Invited?.Select(i => new app.Model.Invitation
                {
                    Name = i.Name,
                    Email=i.Email,
                    InviteDate=i.InviteDate,
                    Role = i.Role.ToString()

                }).ToArray()
             };
             return viewModelTeam;
         }

         public static DomainModel.Team ToDomainModel(
         this app.Model.Team team)
         {
             var viewModelTeam = new DomainModel.Team
             {
                TeamId= team.TeamId,
                Name= team.Name,
                Members= team.Members?.Select(m=> new DomainModel.TeamMember
                {
                    UserId= m.UserId,
                    RemoveDate=m.RemoveDate,
                    StartDate=m.StartDate,
                    Role=(DomainModel.TeamRole)Enum.Parse(typeof(DomainModel.TeamRole),m.Role)
                    
                }).ToArray(),
                Invited = team.Invited?.Select(i => new DomainModel.Invitation
                {
                    Name = i.Name,
                    Email=i.Email,
                    InviteDate=i.InviteDate,
                    Role = (DomainModel.TeamRole)Enum.Parse(typeof(DomainModel.TeamRole),i.Role)

                }).ToArray()
             };
             return viewModelTeam;
         }




    }
}