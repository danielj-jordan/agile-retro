using System;
using System.Linq;
using MongoDB.Bson;
using DomainModel = Retrospective.Domain.Model;
using DBModel = Retrospective.Data.Model;

namespace Retrospective.Domain.ModelExtensions
{
    public static class MeetingExtensions
    {
         public static DBModel.Meeting ToDBModel(
         this Retrospective.Domain.Model.Meeting meeting)
         {
             DBModel.Meeting dbMeeting = new DBModel.Meeting
             {
                 Id = !string.IsNullOrWhiteSpace(meeting.Id)? ObjectId.Parse(meeting.Id):(ObjectId?) null,
                 TeamId= ObjectId.Parse(meeting.TeamId),
                 Name = meeting.Name,
                 Categories = meeting.Categories.Select(c => new DBModel.Category
                 {
                     CategoryNumber = c.CategoryNum,
                     Name = c.Name,
                     SortOrder= c.SortOrder
                 }).ToArray()
             };

             return dbMeeting;
         }

         public static DomainModel.Meeting ToDomainModel(
            this DBModel.Meeting meeting)
         {
             DomainModel.Meeting domainMeeting = new DomainModel.Meeting
             {
                 Id = meeting.Id?.ToString(),
                 TeamId= meeting.TeamId.ToString(),
                 Name = meeting.Name,
                 Categories = meeting.Categories.Select(c => new DomainModel.Category
                 {
                     CategoryNum=c.CategoryNumber,
                     Name = c.Name,
                     SortOrder=c.SortOrder
                 }).ToArray()
             };
             return domainMeeting;
         }
    }
}