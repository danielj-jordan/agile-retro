using AutoMapper;
using DBModel=Retrospective.Data.Model;
using DomainModel=Retrospective.Domain.Model;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;


namespace Retrospective.Domain
{
    public class DomainProfile: Profile
    {
        ILogger _logger;

        public DomainProfile(ILogger<Mapper> logger)
        {
            _logger=logger;
            this.CreateMap();

               
        }

        public DomainProfile()
        {
            this.CreateMap();
        }


        private void CreateMap()
        {

            //mapping for Comment
            CreateMap<DBModel.Comment, DomainModel.Comment>()
                .ForMember(dest=>dest.CommentId, opt=>opt.MapFrom(src=>src.Id))
                .ForMember(dest=>dest.SessionId, opt=>opt.MapFrom(src=>src.RetrospectiveId))
                .ForMember(dest=>dest.CategoryNum, opt=>opt.MapFrom(src=>src.CategoryNumber))
                .ForMember(dest=>dest.UpdateUser, opt=>opt.MapFrom(src=>src.LastUpdateUser))
                .ReverseMap();
               
            //mapping for Category
            CreateMap<DBModel.Category, DomainModel.Category>()
                .ForMember(dest=>dest.CategoryNum, opt=>opt.MapFrom(src=>src.CategoryNumber))
                .ForMember(dest=>dest.Name, opt=>opt.MapFrom(src=>src.Name))
                .ReverseMap();          

            //mapping for Meeting
            CreateMap<DBModel.Meeting, DomainModel.Meeting>()
                .ForMember(dest=>dest.Id, opt=>opt.MapFrom(src=>src.Id))
                .ForMember(dest=>dest.Name, opt=>opt.MapFrom(src=>src.Name))
                .ForMember (dest=>dest.TeamId, opt=>opt.MapFrom(src=>src.TeamId)); 

            CreateMap< DomainModel.Meeting, DBModel.Meeting>()
                .ForMember(dest=>dest.Id, opt=>opt.MapFrom(src=>ObjectId.Parse(src.Id)))
                .ForMember(dest=>dest.Name, opt=>opt.MapFrom(src=>src.Name))
                .ForMember (dest=>dest.TeamId, opt=>opt.MapFrom(src=>ObjectId.Parse(src.TeamId))); 

            //mapping for Team
            CreateMap<DBModel.Team, DomainModel.Team>()
                .ForMember(dest=>dest.TeamId, opt=>opt.MapFrom(src=>src.Id))
                .ForMember(dest=>dest.Name, opt=>opt.MapFrom(src=>src.Name))
                .ForMember(dest=>dest.Owner, opt=>opt.MapFrom(src=>src.Owner))
                .ForMember(dest=>dest.TeamMembers, opt=>opt.MapFrom(src=>src.TeamMembers));

            CreateMap<DomainModel.Team, DBModel.Team>()
                .ForMember(dest=>dest.Id, opt=> opt.MapFrom(src=>ObjectId.Parse(src.TeamId)))
                .ForMember(dest=>dest.Name, opt=>opt.MapFrom(src=>src.Name))
                .ForMember(dest=>dest.Owner, opt=>opt.MapFrom(src=>src.Owner))
                .ForMember(dest=>dest.TeamMembers, opt=>opt.MapFrom(src=>src.TeamMembers));


             
            CreateMap<DBModel.User, DomainModel.User>()
                .ForMember(dest=>dest.UserId, opt=>opt.MapFrom(src=>src.Id))
                .ForMember(dest=>dest.Name, opt=>opt.MapFrom(src=>src.Name))
                .ForMember(dest =>dest.Email , opt => opt.MapFrom(src=>src.Email))
                .ReverseMap();
            
        }






    }
       
    
}