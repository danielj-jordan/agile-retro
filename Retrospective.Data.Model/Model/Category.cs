using System;
using MongoDB.Bson;

namespace Retrospective.Data.Model
{
    public class Category
    {

        public Category(){
        }
        

        public Category(int number, string name){
            this.CategoryNumber=number;
            this.Name=name;
            this.SortOrder=number;
        }
        
        public int CategoryNumber {get;set;}

        public string Name {get; set;}

        public int SortOrder {get;set;}
  

    }
}
