using System;
using Xunit;
using FluentAssertions;
using DomainModel = Retrospective.Domain.Model;
using app.ModelExtensions;

namespace apptest
{
  public class ModelExtensionTest
  {
    [Fact]
    public void Mapping_CommentToDomainModelAndReverse()
    {
      app.Model.Comment comment = new app.Model.Comment
      {
        CommentId = "123",
        MeetingId = "456",
        CategoryNum = 1,
        Text = "This is a test",
        UpdateUserId = "12323444",
        ThisUserVoted = true,
        VoteCount = 1
      };

      var domainComment = comment.ToDomainModel();
      var appComment = domainComment.ToViewModel("abc");

      comment.Should().BeEquivalentTo(appComment, options =>
        options.Excluding(o => o.ThisUserVoted)
        .Excluding(o => o.VoteCount));
    }



    [Fact]
    public void Mapping_TeamToDomainModelAndReverse()
    {
      app.Model.Team team = new app.Model.Team
      {
        TeamId = "456",
        Name = "meeting name",
        Members = new app.Model.TeamMember[]{
                        new app.Model.TeamMember{
                            UserId="456",
                            UserName ="test 1",
                            RemoveDate=DateTime.UtcNow,
                            StartDate=DateTime.UtcNow,
                            Role = "Member"
                        },
                        new app.Model.TeamMember{
                            UserId="456a",
                            UserName ="test 2",
                            RemoveDate=DateTime.UtcNow,
                            StartDate=DateTime.UtcNow,
                            Role = "Member"
                        }
                    },
        Invited = new app.Model.Invitation[]{
                new app.Model.Invitation{
                    Name ="Joe Bool",
                    Email ="jcool@gmail.com",
                    InviteDate = DateTime.UtcNow,
                    Role = "Member"
                }
            }
      };

      var domanModelTeam = team.ToDomainModel();
      var appModelTeam = domanModelTeam.ToViewModel();

      team.Should().BeEquivalentTo(appModelTeam, options =>
      options.Excluding(e => e.Members));

      team.Members.Should().BeEquivalentTo(appModelTeam.Members, o =>
       o.Excluding(e => e.UserName));
    }

    [Fact]
    public void Mapping_MeetingToDomainModelAndReverse()
    {
      app.Model.Meeting meeting = new app.Model.Meeting
      {
        Id = "abc",
        TeamId = "456",
        Name = "meeting name",
        Categories = new app.Model.Category[]{
            new app.Model.Category{
                CategoryNum=1,
                Name="Category 1",
                SortOrder=1
            },
            new app.Model.Category{
                CategoryNum=2,
                Name="Category 2",
                SortOrder=2
            }
        }
      };

      var domainModel = meeting.ToDomainModel();
      var appModel = domainModel.ToViewModel();

      meeting.Should().BeEquivalentTo(appModel);
    }

    [Fact]
    public void Mapping_UserToDomainModelAndReverse()
    {
      app.Model.User user = new app.Model.User
      {
        UserId = "abc",
        Name = "456",
        Email="444fds",
        IsDemoUser=false,
        AuthenticationID="googleid",
        AuthenticationSource="Google",
        LastLoggedIn=DateTime.UtcNow
      };

      var domainModel = user.ToDomainModel();
      var appModel = domainModel.ToViewModel();

      user.Should().BeEquivalentTo(appModel);
    }

  }
}