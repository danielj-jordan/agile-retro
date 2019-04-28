using System;
using AutoMapper;
using Xunit;
using Moq;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Retrospective.Domain.Test
{

  [Collection("Domain Test collection")]
  public class CommentManagerTest
  {
    AutoMapper.Mapper mapper = null;
    TestFixture fixture;

    public CommentManagerTest(TestFixture fixture)
    {
      this.fixture = fixture;
      var config = new MapperConfiguration(c =>
      {
        c.AddProfile<Retrospective.Domain.DomainProfile>();
      });

      mapper = new Mapper(config);
    }

    [Fact]
    public void GetComments()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, mapper, fixture.Database);

      List<Retrospective.Domain.Model.Comment> comments = manager.GetComments(fixture.SampleUser, fixture.SessionId.ToString());

      Assert.True(comments.Count > 0);

    }

    [Fact]
    public void GetCommentsFail_NoAccess()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, mapper, fixture.Database);

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
      var manager = new Retrospective.Domain.CommentManager(logger, mapper, fixture.Database);

      List<Retrospective.Domain.Model.Category> categories = manager.GetCategories(fixture.SampleUser, fixture.SessionId.ToString());

      Assert.True(categories.Count > 0);

    }

    [Fact]
    public void GetCategoriesFail_NoAccess()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, mapper, fixture.Database);

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
      var manager = new Retrospective.Domain.CommentManager(logger, mapper, fixture.Database);

      manager.DeleteComment(fixture.Owner, fixture.DeleteNote.ToString());
    }

    [Fact]
    public void DeleteCommentFail_NoAccess()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, mapper, fixture.Database);

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
      var manager = new Retrospective.Domain.CommentManager(logger, mapper, fixture.Database);

      Domain.Model.Comment comment = new Model.Comment();
      comment.CategoryNumber = 1;
      comment.MeetingId = fixture.SessionId.ToString();
      comment.Text = "test";

      var saved = manager.SaveComment(fixture.Owner, comment);
      Assert.NotNull(saved.CommentId);

      saved.Text += " again";

      var savedAgain = manager.SaveComment(fixture.Owner, saved);
      Assert.Equal(savedAgain.CommentId, saved.CommentId);
    }

    [Fact]
    public void SaveCommentFail_NoAccess()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, mapper, fixture.Database);

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
      var dataMock = new Mock<Retrospective.Data.IDatabase>();
      var dataCommentMock = new Mock<Retrospective.Data.IDataComment>();


      dataMock.Setup(m => m.Comments.GetComment(It.IsAny<ObjectId>())).Returns(
          new Retrospective.Data.Model.Comment
          {
            VotedUp = new string[] { "testUserA", "testUserB" }
          }
      );

      dataMock.Setup(m => m.Meetings.Get(It.IsAny<string>())).Returns(
        new Retrospective.Data.Model.Meeting()
      );

      dataMock.Setup(m => m.Teams.Get(It.IsAny<string>())).Returns(
        new Data.Model.Team
        {
          Owner = "testUserA"
        }
      );

      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, mapper, dataMock.Object);

      //act
      var resultComment = manager.VoteDown("testUserA", "507f1f77bcf86cd799439011");

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
      var dataMock = new Mock<Retrospective.Data.IDatabase>();
      var dataCommentMock = new Mock<Retrospective.Data.IDataComment>();


      dataMock.Setup(m => m.Comments.GetComment(It.IsAny<ObjectId>())).Returns(
          new Retrospective.Data.Model.Comment
          {
            VotedUp = new string[] { "testUserA", "testUserB" }
          }
      );

      dataMock.Setup(m => m.Meetings.Get(It.IsAny<string>())).Returns(
        new Retrospective.Data.Model.Meeting()
      );


      dataMock.Setup(m => m.Teams.Get(It.IsAny<string>())).Returns(
        new Data.Model.Team
        {
          Owner = "testUserC"
        }
      );

      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, mapper, dataMock.Object);

      //act
      var resultComment = manager.VoteDown("testUserC", "507f1f77bcf86cd799439011");

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
      var dataMock = new Mock<Retrospective.Data.IDatabase>();
      var dataCommentMock = new Mock<Retrospective.Data.IDataComment>();


      dataMock.Setup(m => m.Comments.GetComment(It.IsAny<ObjectId>())).Returns(
          new Retrospective.Data.Model.Comment
          {
            VotedUp = new string[] { "testUserA", "testUserB" }
          }
      );

      dataMock.Setup(m => m.Meetings.Get(It.IsAny<string>())).Returns(
        new Retrospective.Data.Model.Meeting()
      );


      dataMock.Setup(m => m.Teams.Get(It.IsAny<string>())).Returns(
        new Data.Model.Team
        {
          Owner = "testUserC"
        }
      );

      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, mapper, dataMock.Object);

      //act
      var resultComment = manager.VoteUp("testUserC", "507f1f77bcf86cd799439011");

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
      var dataMock = new Mock<Retrospective.Data.IDatabase>();
      var dataCommentMock = new Mock<Retrospective.Data.IDataComment>();


      dataMock.Setup(m => m.Comments.GetComment(It.IsAny<ObjectId>())).Returns(
          new Retrospective.Data.Model.Comment
          {
            VotedUp = new string[] { "testUserA", "testUserB" }
          }
      );

      dataMock.Setup(m => m.Meetings.Get(It.IsAny<string>())).Returns(
        new Retrospective.Data.Model.Meeting()
      );


      dataMock.Setup(m => m.Teams.Get(It.IsAny<string>())).Returns(
        new Data.Model.Team
        {
          Owner = "testUserA"
        }
      );

      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, mapper, dataMock.Object);

      //act
      var resultComment = manager.VoteUp("testUserA", "507f1f77bcf86cd799439011");

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
      var dataMock = new Mock<Retrospective.Data.IDatabase>();
      var dataCommentMock = new Mock<Retrospective.Data.IDataComment>();

      dataMock.Setup(m => m.Comments.GetComment(It.IsAny<ObjectId>())).Returns(
          new Retrospective.Data.Model.Comment
          {
            CategoryNumber = 1
          }
      );

      dataMock.Setup(m => m.Meetings.Get(It.IsAny<string>())).Returns(
        new Retrospective.Data.Model.Meeting()
      );


      dataMock.Setup(m => m.Teams.Get(It.IsAny<string>())).Returns(
        new Data.Model.Team
        {
          Owner = "testUserA"
        }
      );

      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager>();
      var manager = new Retrospective.Domain.CommentManager(logger, mapper, dataMock.Object);

      //act
      Retrospective.Domain.Model.Comment comment = new Model.Comment
      {
        CategoryNumber = 2,
        CommentId = "5c9d9bbb34e1434ab9b93ed3"
      };

      manager.UpdateCommentText("testUserA", comment);

      //assert
      //check the value passed to SaveComment
      dataMock.Verify(s => s.Comments.SaveComment(It.Is<Retrospective.Data.Model.Comment>(
        c => c.CategoryNumber == 2)
        ), Times.Once);
    }
  }
}