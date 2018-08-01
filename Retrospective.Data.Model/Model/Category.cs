using System;
using MongoDB.Bson;

namespace Retrospective.Data.Model
{
    public class Category
    {
        public Category(int number, string name){
            this.CategoryNumber=number;
            this.Name=name;
        }
        
        public int CategoryNumber {get;set;}

        public string Name {get; set;}
  

    }
}
