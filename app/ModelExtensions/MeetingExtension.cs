using System;
using System.Collections.Generic;
using System.Linq;

namespace app.ModelExtensions
{
  public static class MeetingExtension
  {
    public static app.Model.Meeting ToViewModel(
     this Retrospective.Domain.Model.Meeting meeting)
    {
      var viewModelMeeting = new app.Model.Meeting
      {
        Id = meeting.Id,
        TeamId = meeting.TeamId,
        Name = meeting.Name,
        Categories = meeting.Categories.Select(c =>
        new app.Model.Category
        {
          CategoryNum = c.CategoryNum,
          Name = c.Name,
          SortOrder = c.SortOrder
        }).ToArray()
      };
      return viewModelMeeting;
    }

    public static Retrospective.Domain.Model.Meeting ToDomainModel(
        this app.Model.Meeting meeting)
    {
      var domainMeeting = new Retrospective.Domain.Model.Meeting
      {
        Id = meeting.Id,
        TeamId = meeting.TeamId,
        Name = meeting.Name,
        Categories = meeting.Categories.Select(c =>
        new Retrospective.Domain.Model.Category
        {
          CategoryNum = c.CategoryNum,
          Name = c.Name,
          SortOrder = c.SortOrder
        }).ToArray()
      };
      return domainMeeting;
    }
  }
}