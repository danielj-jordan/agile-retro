using System;
using Xunit;
using Retrospective.Data.Model;
using Retrospective.Data;

namespace app_db_test
{
    public class UnitTest1
    {
        [Fact]
        public void SaveComment()
        {

            Comment comment = new Comment();
            comment.Text="this is a test too.";
        

            CommentData database = new CommentData();
            database.SaveComment(comment);


            Console.WriteLine("created id:{0}", comment.Id);
            Assert.True(comment.Id!=null);

        }
    }
}
