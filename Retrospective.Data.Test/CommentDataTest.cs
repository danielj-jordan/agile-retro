using System;
using System.Linq;
using Xunit;
using Retrospective.Data.Model;
using Retrospective.Data;
using MongoDB.Bson;

namespace Retrospective.Data.Test
{

  
    [Collection("Database collection")]
    public class CommentDataTest
    {
        private IDatabase database;
        private TestFixture fixture;

        public CommentDataTest(TestFixture fixture)
        {
            this.fixture=fixture;
            database=fixture.database;
        }

        [Fact]
        public void SaveComment()
        {

            Comment comment = new Comment();
            comment.Text="this is a test too.";
            comment.MeetingId=new ObjectId();
            comment.Id=null;
            
        
            
            DataComment commentdata = new DataComment(database);
            commentdata.SaveComment(comment);


            Console.WriteLine("created id:{0}", comment.Id);
            Assert.True(comment.Id!=null);

        }


        [Fact]
        public void RetreiveComment()
        {
            DataComment commentdata = new DataComment(database);

            //setup a record to find
            Comment comment = new Comment();
            comment.Id=null;
            comment.Text="this is a test too.";
            comment.MeetingId = (ObjectId)this.fixture.meeting.Id;
            commentdata.SaveComment(comment);

            var createdId=(ObjectId)comment.Id;


            Assert.True(createdId!=null);


            var foundComment = commentdata.GetComment(createdId);

            Assert.True(createdId==foundComment.Id);

            var foundComments= commentdata.GetComments((ObjectId)this.fixture.meeting.Id);
            
            Assert.Contains(this.fixture.meeting.Id.ToString(), from com in foundComments select com.MeetingId.ToString());


        }
    }
}
