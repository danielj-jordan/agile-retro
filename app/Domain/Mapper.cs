using AutoMapper;
using ViewModel =app.Model;
using DBModel=Retrospective.Data.Model;
using DomainModel=Retrospective.Domain.Model;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;


namespace app.Domain
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

            //mapping from Domain.Models
             CreateMap<ViewModel.Team, DomainModel.Team>().ReverseMap();

             CreateMap<ViewModel.User, DomainModel.User>().ReverseMap();

             CreateMap<ViewModel.Meeting, DomainModel.Meeting>().ReverseMap();

            CreateMap<ViewModel.TeamMember, DomainModel.TeamMember>().ReverseMap();

            //mapping for Comment
            CreateMap<ViewModel.Comment, DomainModel.Comment>()
                .ForMember(dest=>dest.CommentId, opt=>opt.MapFrom(src=>src.CommentId))
                .ForMember(dest=>dest.MeetingId, opt=>opt.MapFrom(src=>src.SessionId))
                .ForMember(dest=>dest.CategoryNumber, opt=>opt.MapFrom(src=>src.CategoryNum))
                .ForMember(dest=>dest.LastUpdateUser, opt=>opt.MapFrom(src=>src.UpdateUser))
                .ForMember(dest=>dest.LastUpdateDate, opt=>opt.Ignore())
                .ForMember(dest=>dest.VotedUp, opt=>opt.Ignore());


            CreateMap<DomainModel.Comment, ViewModel.Comment>()
                .ForMember(dest=>dest.CommentId, opt=>opt.MapFrom(src=>src.CommentId))
                .ForMember(dest=>dest.SessionId, opt=>opt.MapFrom(src=>src.MeetingId))
                .ForMember(dest=>dest.CategoryNum, opt=>opt.MapFrom(src=>src.CategoryNumber))
                .ForMember(dest=>dest.UpdateUser, opt=>opt.MapFrom(src=>src.LastUpdateUser));


               




        }





    }
       
    
}