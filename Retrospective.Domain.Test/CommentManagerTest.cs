using System;
using AutoMapper;
using Xunit;
using System.Collections.Generic;

namespace Retrospective.Domain.Test {

    [Collection ("Domain Test collection")]
    public class CommentManagerTest 
    {
        AutoMapper.Mapper mapper = null;
        TestFixture fixture;

        public CommentManagerTest (TestFixture fixture) {
            this.fixture = fixture;
            var config = new MapperConfiguration (c => {
                c.AddProfile<Retrospective.Domain.DomainProfile> ();
            });

            mapper = new Mapper (config);
        }

        [Fact]
        public void GetComments()
        {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager> ();
            var manager = new Retrospective.Domain.CommentManager (logger, mapper, fixture.Database);

            List<Retrospective.Domain.Model.Comment> comments = manager.GetComments(fixture.SampleUser, fixture.SessionId.ToString ());
            
            Assert.True(comments.Count>0);

        }

        [Fact]
        public void GetCommentsFail_NoAccess()
        {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager> ();
            var manager = new Retrospective.Domain.CommentManager (logger, mapper, fixture.Database);

            Assert.Throws<Exception.AccessDenied> (
                () => {
                    manager.GetComments ("", fixture.SessionId.ToString ());
                }
            );
        }

        [Fact]
        public void GetCategories()
        {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager> ();
            var manager = new Retrospective.Domain.CommentManager (logger, mapper, fixture.Database);

            List<Retrospective.Domain.Model.Category> categories = manager.GetCategories(fixture.SampleUser, fixture.SessionId.ToString ());
            
            Assert.True(categories.Count>0);

        }

        [Fact]
        public void GetCategoriesFail_NoAccess()
        {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager> ();
            var manager = new Retrospective.Domain.CommentManager (logger, mapper, fixture.Database);
           
            Assert.Throws<Exception.AccessDenied>(
                ()=>{
                    manager.GetCategories("", fixture.SessionId.ToString ());
                }
            );
        }

        [Fact]
        public void DeleteComment()
        {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager> ();
            var manager = new Retrospective.Domain.CommentManager (logger, mapper, fixture.Database);
          
            manager.DeleteComment(fixture.Owner, fixture.DeleteNote.ToString());
        }

        [Fact]
        public void DeleteCommentFail_NoAccess()
        {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager> ();
            var manager = new Retrospective.Domain.CommentManager (logger, mapper, fixture.Database);

            Assert.Throws<Exception.AccessDenied>(
                ()=>{
                    manager.DeleteComment("", fixture.UpdateNote.ToString());
                }
            );
  
        }

        [Fact]
        public void SaveComment()
        {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager> ();
            var manager = new Retrospective.Domain.CommentManager (logger, mapper, fixture.Database);
  
            Domain.Model.Comment comment = new Model.Comment();
            comment.CategoryNumber=1;
            comment.MeetingId=fixture.SessionId.ToString();
            comment.Text="test";

            var saved = manager.SaveComment(fixture.Owner, comment);
            Assert.NotNull(saved.CommentId);

            saved.Text += " again";

            var savedAgain= manager.SaveComment(fixture.Owner, saved);
            Assert.Equal(savedAgain.CommentId, saved.CommentId);
        }

        [Fact]
        public void SaveCommentFail_NoAccess()
        {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager> ();
            var manager = new Retrospective.Domain.CommentManager (logger, mapper, fixture.Database);

            Domain.Model.Comment comment = new Model.Comment();
            comment.CategoryNumber=1;
            comment.MeetingId=fixture.SessionId.ToString();
            comment.Text="test";

            Assert.Throws<Exception.AccessDenied>(
                ()=>{
                    manager.SaveComment("", comment);
                }
            );


        }

    }
}