using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel = Retrospective.Domain.Model;

namespace app.ModelExtensions
{
    public static class InvitationExtension
    {
        
         public static DomainModel.Invitation ToDomainModel(
         this app.Model.Invitation invitation)
         {
             var domainInvite = new DomainModel.Invitation
             {
                Name = invitation.Name,
                Email=invitation.Email,
                InviteDate=invitation.InviteDate,
                Role = (DomainModel.TeamRole)Enum.Parse(typeof(DomainModel.TeamRole),invitation.Role)
             };
             return domainInvite;
         }
    }
}