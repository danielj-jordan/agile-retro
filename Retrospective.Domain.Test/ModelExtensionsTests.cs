using System;
using System.Collections.Generic;
using DBModel = Retrospective.Data.Model;
using DomainModel = Retrospective.Domain.Model;
using FluentAssertions;
using MongoDB.Bson;
using Retrospective.Domain.ModelExtensions;
using Xunit;

namespace Retrospective.Domain.Test
{
  public class ModelExtensionsTests
  {
    [Fact]
    public void Mapping_CommentToDBModelAndReverse()
    {
      DomainModel.Comment comment = new DomainModel.Comment
      {
        CommentId = ObjectId.GenerateNewId().ToString(),
        MeetingId = ObjectId.GenerateNewId().ToString(),
        CategoryNumber = 1,
        Text = "This is a test",
        LastUpdateUserId = ObjectId.GenerateNewId().ToString(),
        LastUpdateDate = System.DateTime.UtcNow,
        VotedUp = new List<string>{
                    ObjectId.GenerateNewId().ToString(),
                    ObjectId.GenerateNewId().ToString(),
                }
      };

      var dbComment = comment.ToDBModel();
      var domainComment = dbComment.ToDomainModel();

      comment.Should().BeEquivalentTo(domainComment);
    }


    [Fact]
    public void Mapping_MeetingToDBModelAndReverse()
    {
      DomainModel.Meeting meeting = new DomainModel.Meeting
      {
        Id = ObjectId.GenerateNewId().ToString(),
        TeamId = ObjectId.GenerateNewId().ToString(),
        Name = "meeting name",
        Categories = new DomainModel.Category[]{
                    new DomainModel.Category{
                        CategoryNum=1,
                        Name ="test 1",
                        SortOrder=1,
                    },
                    new DomainModel.Category{
                        CategoryNum=2,
                        Name ="test 2",
                        SortOrder=2,
                    }
                }
      };

      var dbMeeting = meeting.ToDBModel();
      var domainMeeting = dbMeeting.ToDomainModel();

      meeting.Should().BeEquivalentTo(domainMeeting);
    }


    [Fact]
    public void Mapping_TeamToDBModelAndReverse()
    {
      DomainModel.Team team = new DomainModel.Team
      {
        TeamId = ObjectId.GenerateNewId().ToString(),
        Name = "team name",
        Members = new DomainModel.TeamMember[]
        {
            new DomainModel.TeamMember
            {
                UserId = ObjectId.GenerateNewId().ToString(),
                RemoveDate=DateTime.UtcNow,
                StartDate=DateTime.UtcNow,
                Role = DomainModel.TeamRole.Owner
            },
            new DomainModel.TeamMember
            {
                UserId = ObjectId.GenerateNewId().ToString(),
                RemoveDate=DateTime.UtcNow,
                StartDate=DateTime.UtcNow,
                Role = DomainModel.TeamRole.Member
            }
        },
        Invited = new DomainModel.Invitation[]
        {
            new DomainModel.Invitation
            {
                Name = "Joe Cool",
                Email ="nobody@home.com",
                InviteDate=DateTime.UtcNow,
                Role = DomainModel.TeamRole.Stakeholder
            }
        }
      };

      var dbTeam = team.ToDBModel();
      var domainTeam = dbTeam.ToDomainModel();

      team.Should().BeEquivalentTo(domainTeam);
    }

    [Fact]
    public void Mapping_UserToDBModelAndReverse()
    {
      DomainModel.User user = new DomainModel.User
      {
          UserId= ObjectId.GenerateNewId().ToString(),
          Name ="Joe Cool",
          Email="nobody@home",
          IsDemoUser=false,
          LastLoggedIn=DateTime.UtcNow,
          AuthenticationSource="test source",
          AuthenticationID="123"
      };

      var dbUser = user.ToDBModel();
      var domainUser = dbUser.ToDomainModel();

      user.Should().BeEquivalentTo(domainUser);
    }
  }
}