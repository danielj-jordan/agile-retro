using System.Collections.Generic;
using MongoDB.Bson;
using Retrospective.Data.Model;

namespace Retrospective.Data
{
  public interface IDataMeeting
  {

    Meeting Save(Meeting meeting);

    List<Meeting> GetMeetings(string teamObjectId);

    List<Meeting> GetMeetings(ObjectId teamObjectId);

    Meeting Get(string meetingId);

    Meeting Get(ObjectId retroId);
  }
}