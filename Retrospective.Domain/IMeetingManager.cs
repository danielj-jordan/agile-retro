using System;
using System.Collections.Generic;
using DomainModel=Retrospective.Domain.Model;
using Retrospective.Data;
using Retrospective.Domain.ModelExtensions;


namespace Retrospective.Domain
{
    public interface IMeetingManager: IBaseManager
    {
        List<DomainModel.Meeting> GetMeetingsForTeam(string activeUser, string teamId);
        DomainModel.Meeting GetMeeting(string activeUser, string meetingId);
        DomainModel.Meeting SaveMeeting(string activeUser, DomainModel.Meeting meeting);

         
    }
}