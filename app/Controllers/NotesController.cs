using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        

        [HttpGet("[action]")]
        public IEnumerable<Comment> Notes()
        {
            var comments = new List<Comment>();
            comments.Add(new Comment{CommentId=1, CategoryId=2, Text="this sucks", UpdateUser="Snooper"});
            comments.Add(new Comment{CommentId=2, CategoryId=2, Text="No it does not", UpdateUser="Marcie"});

            return comments;
        }

        [HttpGet("[action]")]
        public IEnumerable<Category> Categories()
        {
            var categories= new List<Category>();
            categories.Add(new Category { CategoryId=1, Name ="this is a test"});
            categories.Add(new Category {CategoryId=2, Name ="waiting for another category"});

            return categories;

        }

        
        public class Comment
        {
            public int CommentId {get;set;}
            public int CategoryId { get; set; }
            public string Text { get; set; }
            public string UpdateUser { get; set; }
            
        }


        public class Category
        {
            public int CategoryId {get;set;}
            public string Name {get;set;}

        }
    }
}
