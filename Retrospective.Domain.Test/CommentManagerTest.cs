using System;
using System.Collections.Generic;
using AutoMapper;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace Retrospective.Domain.Test
{

  [Collection("Domain Test collection")]
  public class CommentManagerTest
  {
    TestFixture fixture;

    public CommentManagerTest(TestFixture fixture)
    {
      this.fixture = fixture;
    }


    [Fact]
    public void GetComments()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, fixture.Database);

      List<Retrospective.Domain.Model.Comment> comments = manager.GetComments(fixture.SampleUser.Id.ToString(), fixture.SessionId.ToString());

      Assert.True(comments.Count > 0);

    }

    [Fact]
    public void GetCommentsFail_NoAccess()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, fixture.Database);

      Assert.Throws<Exception.AccessDenied>(
          () =>
          {
            manager.GetComments("", fixture.SessionId.ToString());
          }
      );
    }

    [Fact]
    public void GetCategories()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, fixture.Database);

      List<Retrospective.Domain.Model.Category> categories = manager.GetCategories(fixture.SampleUser.Id.ToString(), fixture.SessionId.ToString());

      Assert.True(categories.Count > 0);

    }

    [Fact]
    public void GetCategoriesFail_NoAccess()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, fixture.Database);

      Assert.Throws<Exception.AccessDenied>(
          () =>
          {
            manager.GetCategories("", fixture.SessionId.ToString());
          }
      );
    }

    [Fact]
    public void DeleteComment()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, fixture.Database);

      manager.DeleteComment(fixture.OwnerUser.Id.ToString(), fixture.DeleteNote.ToString());
    }

    [Fact]
    public void DeleteCommentFail_NoAccess()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, fixture.Database);

      Assert.Throws<Exception.AccessDenied>(
          () =>
          {
            manager.DeleteComment("", fixture.UpdateNote.ToString());
          }
      );

    }

    [Fact]
    public void SaveComment()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, fixture.Database);

      Domain.Model.Comment comment = new Model.Comment();
      comment.CategoryNumber = 1;
      comment.MeetingId = fixture.SessionId.ToString();
      comment.Text = "test";
      comment.LastUpdateUserId = this.fixture.SampleUser.Id.ToString();

      var saved = manager.SaveComment(fixture.OwnerUser.Id.ToString(), comment);
      Assert.NotNull(saved.CommentId);

      saved.Text += " again";

      var savedAgain = manager.SaveComment(fixture.OwnerUser.Id.ToString(), saved);
      Assert.Equal(savedAgain.CommentId, saved.CommentId);
    }

    [Fact]
    public void SaveCommentFail_NoAccess()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, fixture.Database);

      Domain.Model.Comment comment = new Model.Comment();
      comment.CategoryNumber = 1;
      comment.MeetingId = fixture.SessionId.ToString();
      comment.Text = "test";

      Assert.Throws<Exception.AccessDenied>(
          () =>
          {
            manager.SaveComment("", comment);
          }
      );
    }

    [Fact]
    public void VoteDown_WithAlreadyVotedUser_ShouldRemoveFromList()
    {
      //arrange
      string userA = "1a191fc44dec80f5829e6c1a";
      string userB = "2b191fc44dec80f5829e6c2b";
      var dataMock = new Mock<Retrospective.Data.IDatabase>();
      var dataCommentMock = new Mock<Retrospective.Data.IDataComment>();


      dataMock.Setup(m => m.Comments.GetComment(It.IsAny<ObjectId>())).Returns(
          new Retrospective.Data.Model.Comment
          {
            VotedUp = new ObjectId[] { new ObjectId(userA), new ObjectId(userB) }
          }
      );

      var teamId = ObjectId.GenerateNewId();
      dataMock.Setup(m => m.Teams.Get(It.IsAny<string>())).Returns(
        new Data.Model.Team
        {
          Id = teamId,
          Members = new Data.Model.TeamMember[]
          {
            new Data.Model.TeamMember
            {
              UserId= new ObjectId(userA),
              Role =  Data.Model.TeamRole.Owner
            }
          }
        }
      );

      var meetingid = ObjectId.GenerateNewId();
      dataMock.Setup(m => m.Meetings.Get(It.IsAny<string>())).Returns(
        new Retrospective.Data.Model.Meeting
        {
          Id = meetingid,
          TeamId = teamId,
          Categories = new Data.Model.Category[0]
        }
      );

      dataMock.Setup(m => m.Comments.SaveComment(It.IsAny<Data.Model.Comment>())).Returns(
        new Data.Model.Comment
        {
          CategoryNumber = 2,
          Id = ObjectId.Parse("507f1f77bcf86cd799439011"),
          MeetingId = meetingid
        }
       );


      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, dataMock.Object);

      //act
      var resultComment = manager.VoteDown(userA, "507f1f77bcf86cd799439011");

      //assert

      //check the value passed to SaveComment
      dataMock.Verify(s => s.Comments.SaveComment(It.Is<Retrospective.Data.Model.Comment>(
        c => c.VotedUp.Length == 1)
          ), Times.Once);

    }


    [Fact]
    public void VoteDown_WithAlreadyUnVotedUser_ShouldNotRemoveFromList()
    {
      //arrange
      string userA = "1a191fc44dec80f5829e6c1a";
      string userB = "2b191fc44dec80f5829e6c2b";
      string userC = "3c191fc44dec80f5829e6c3c";

      var dataMock = new Mock<Retrospective.Data.IDatabase>();
      var dataCommentMock = new Mock<Retrospective.Data.IDataComment>();


      dataMock.Setup(m => m.Comments.GetComment(It.IsAny<ObjectId>())).Returns(
          new Retrospective.Data.Model.Comment
          {
            VotedUp = new ObjectId[] { new ObjectId(userA), new ObjectId(userB) }
          }
      );

      var teamId = ObjectId.GenerateNewId();
      dataMock.Setup(m => m.Teams.Get(It.IsAny<string>())).Returns(
        new Data.Model.Team
        {
          Id = teamId,
          Members = new Data.Model.TeamMember[]
          {
            new Data.Model.TeamMember
            {
              UserId= new ObjectId(userC),
              Role =  Data.Model.TeamRole.Owner
            }
          }
        }
      );

      var meetingid = ObjectId.GenerateNewId();
      dataMock.Setup(m => m.Meetings.Get(It.IsAny<string>())).Returns(
        new Retrospective.Data.Model.Meeting
        {
          Id = meetingid,
          TeamId = teamId,
          Categories = new Data.Model.Category[0]
        }
      );

      dataMock.Setup(m => m.Comments.SaveComment(It.IsAny<Data.Model.Comment>())).Returns(
        new Data.Model.Comment
        {
          CategoryNumber = 2,
          Id = ObjectId.Parse("507f1f77bcf86cd799439011"),
          MeetingId = meetingid
        }
       );

      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, dataMock.Object);

      //act
      var resultComment = manager.VoteDown(userC, "507f1f77bcf86cd799439011");

      //assert

      //check the value passed to SaveComment
      dataMock.Verify(s => s.Comments.SaveComment(It.Is<Retrospective.Data.Model.Comment>(
        c => c.VotedUp.Length == 2)
      ), Times.Once);
    }


    [Fact]
    public void VoteUp_WithUnVotedUser_ShouldAddToList()
    {
      //arrange
      string userA = "1a191fc44dec80f5829e6c1a";
      string userB = "2b191fc44dec80f5829e6c2b";
      string userC = "3c191fc44dec80f5829e6c3c";
      var dataMock = new Mock<Retrospective.Data.IDatabase>();
      var dataCommentMock = new Mock<Retrospective.Data.IDataComment>();

      dataMock.Setup(m => m.Comments.GetComment(It.IsAny<ObjectId>())).Returns(
          new Retrospective.Data.Model.Comment
          {
            VotedUp = new ObjectId[] { new ObjectId(userA), new ObjectId(userB) }
          }
      );


      var teamId = ObjectId.GenerateNewId();
      dataMock.Setup(m => m.Teams.Get(It.IsAny<string>())).Returns(
        new Data.Model.Team
        {
          Id = teamId,
          Members = new Data.Model.TeamMember[]
          {
            new Data.Model.TeamMember
            {
              UserId= new ObjectId(userC),
              Role =  Data.Model.TeamRole.Owner
            }
          }
        }
      );

      var meetingid = ObjectId.GenerateNewId();
      dataMock.Setup(m => m.Meetings.Get(It.IsAny<string>())).Returns(
        new Retrospective.Data.Model.Meeting
        {
          Id = meetingid,
          TeamId = teamId,
          Categories = new Data.Model.Category[0]
        }
      );

      dataMock.Setup(m => m.Comments.SaveComment(It.IsAny<Data.Model.Comment>())).Returns(
        new Data.Model.Comment
        {
          CategoryNumber = 2,
          Id = ObjectId.Parse("507f1f77bcf86cd799439011"),
          MeetingId = meetingid
        }
       );

      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, dataMock.Object);

      //act
      var resultComment = manager.VoteUp(userC, "507f1f77bcf86cd799439011");

      //assert

      //check the value passed to SaveComment
      dataMock.Verify(s => s.Comments.SaveComment(It.Is<Retrospective.Data.Model.Comment>(
        c => c.VotedUp.Length == 3)
      ), Times.Once);
    }

    [Fact]
    public void VoteUp_AlreadyVotedUser_ShouldNotAddToList()
    {

      //arrange
      string userA = "1a191fc44dec80f5829e6c1a";
      string userB = "2b191fc44dec80f5829e6c2b";
      var dataMock = new Mock<Retrospective.Data.IDatabase>();
      var dataCommentMock = new Mock<Retrospective.Data.IDataComment>();


      dataMock.Setup(m => m.Comments.GetComment(It.IsAny<ObjectId>())).Returns(
          new Retrospective.Data.Model.Comment
          {
            VotedUp = new ObjectId[] { new ObjectId(userA), new ObjectId(userB) }
          }
      );

      var teamId = ObjectId.GenerateNewId();
      dataMock.Setup(m => m.Teams.Get(It.IsAny<string>())).Returns(
        new Data.Model.Team
        {
          Id = teamId,
          Members = new Data.Model.TeamMember[]
          {
            new Data.Model.TeamMember
            {
              UserId= new ObjectId(userA),
              Role =  Data.Model.TeamRole.Owner
            }
          }
        }
      );

      var meetingid = ObjectId.GenerateNewId();
      dataMock.Setup(m => m.Meetings.Get(It.IsAny<string>())).Returns(
        new Retrospective.Data.Model.Meeting
        {
          Id = meetingid,
          TeamId = teamId,
          Categories = new Data.Model.Category[0]
        }
      );

      dataMock.Setup(m => m.Comments.SaveComment(It.IsAny<Data.Model.Comment>())).Returns(
        new Data.Model.Comment
        {
          CategoryNumber = 2,
          Id = ObjectId.Parse("507f1f77bcf86cd799439011"),
          MeetingId = meetingid
        }
       );

      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, dataMock.Object);

      //act
      var resultComment = manager.VoteUp(userA, "507f1f77bcf86cd799439011");

      //assert

      //check the value passed to SaveComment
      dataMock.Verify(s => s.Comments.SaveComment(It.Is<Retrospective.Data.Model.Comment>(
        c => c.VotedUp.Length == 2)
      ), Times.Once);
    }

    [Fact]
    public void UpdateComment_WithDifferentCategoryNumber_UpdatesCategoryNum()
    {
      //arrange

      string userId = "5d191fc44dec80f5829e6ca9";
      var dataMock = new Mock<Retrospective.Data.IDatabase>();
      var dataCommentMock = new Mock<Retrospective.Data.IDataComment>();

      dataMock.Setup(m => m.Comments.GetComment(It.IsAny<ObjectId>())).Returns(
          new Retrospective.Data.Model.Comment
          {
            MeetingId = ObjectId.GenerateNewId(),
            CategoryNumber = 1
          }
      );

      dataMock.Setup(m => m.Meetings.Get(It.IsAny<string>())).Returns(
        new Retrospective.Data.Model.Meeting
        {
          Id = ObjectId.GenerateNewId(),
          TeamId = ObjectId.GenerateNewId(),
          Categories = new Data.Model.Category[0]
        }
      );


      var stubTeam = new Data.Model.Team
      {
        Members = new Data.Model.TeamMember[]
          {
            new Data.Model.TeamMember
            {
              UserId= new ObjectId(userId),
              Role =  Data.Model.TeamRole.Owner
            }
          }
      };

      dataMock.Setup(m => m.Teams.Get(It.IsAny<string>())).Returns(stubTeam);


      dataMock.Setup(m => m.Comments.SaveComment(It.IsAny<Data.Model.Comment>())).Returns(
        new Data.Model.Comment
        {
          CategoryNumber = 2,
          Id = ObjectId.GenerateNewId(),
          MeetingId = ObjectId.GenerateNewId()
        }
       );


      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, dataMock.Object);

      //act
      Retrospective.Domain.Model.Comment comment = new Model.Comment
      {
        CategoryNumber = 2,
        CommentId = "5c9d9bbb34e1434ab9b93ed3",
        MeetingId = ObjectId.GenerateNewId().ToString()
      };

      manager.UpdateCommentText(userId, comment);

      //assert
      //check the value passed to SaveComment
      dataMock.Verify(s => s.Comments.SaveComment(It.Is<Retrospective.Data.Model.Comment>(
        c => c.CategoryNumber == 2)
        ), Times.Once);
    }
  }
}