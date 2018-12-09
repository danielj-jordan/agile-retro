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

        public CommentDataTest(TestFixture fixture)
        {
            database=fixture.database;
        }

        [Fact]
        public void SaveComment()
        {

            Comment comment = new Comment();
            comment.Text="this is a test too.";
            comment.RetrospectiveId=new ObjectId();
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

            ObjectId retrospectiveId= new ObjectId();

            //setup a record to find
            Comment comment = new Comment();
            comment.Id=null;
            comment.Text="this is a test too.";
            comment.RetrospectiveId =retrospectiveId;
            commentdata.SaveComment(comment);

            var createdId=(ObjectId)comment.Id;


            Assert.True(createdId!=null);


            var foundComment = commentdata.GetComment(createdId);

            Assert.True(createdId==foundComment.Id);

            var foundComments= commentdata.GetComments(retrospectiveId);
            
            Assert.Contains(retrospectiveId, from com in foundComments select com.RetrospectiveId);


        }
    }
}
