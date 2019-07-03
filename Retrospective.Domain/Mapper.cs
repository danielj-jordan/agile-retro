using AutoMapper;
using DBModel = Retrospective.Data.Model;
using DomainModel = Retrospective.Domain.Model;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Retrospective.Domain {
    public class DomainProfile : Profile {
        ILogger _logger;

        public DomainProfile (ILogger<Mapper> logger) {
            _logger = logger;
            this.CreateMap ();

        }

        public DomainProfile () {
            this.CreateMap ();
        }

        private void CreateMap () {

            //mapping for Comment
            CreateMap<DBModel.Comment, DomainModel.Comment> ()
                .ForMember (dest => dest.CommentId, opt => opt.MapFrom (src => src.Id))
                .ForMember (dest => dest.MeetingId, opt => opt.MapFrom (src => src.MeetingId))
                .ForMember (dest => dest.CategoryNumber, opt => opt.MapFrom (src => src.CategoryNumber))
                .ForMember (dest => dest.Text, opt => opt.MapFrom (src => src.Text))
                .ForMember (dest => dest.LastUpdateUserId, opt => opt.MapFrom (src => src.LastUpdateUserId));

            CreateMap<DomainModel.Comment, DBModel.Comment> ()
                .ForMember (dest => dest.Id, opt => opt.MapFrom (src => ObjectId.Parse (src.CommentId)))
                .ForMember (dest => dest.MeetingId, opt => opt.MapFrom (src => ObjectId.Parse (src.MeetingId)))
                .ForMember (dest => dest.CategoryNumber, opt => opt.MapFrom (src => src.CategoryNumber))
                .ForMember (dest => dest.Text, opt => opt.MapFrom (src => src.Text))
                .ForMember (dest => dest.LastUpdateUserId, opt => opt.MapFrom (src => src.LastUpdateUserId));

            //mapping for Category
            CreateMap<DBModel.Category, DomainModel.Category> ()
                .ForMember (dest => dest.CategoryNum, opt => opt.MapFrom (src => src.CategoryNumber))
                .ForMember (dest => dest.Name, opt => opt.MapFrom (src => src.Name))
                .ReverseMap ();

            //mapping for Meeting
            CreateMap<DBModel.Meeting, DomainModel.Meeting> ()
                .ForMember (dest => dest.Id, opt => opt.MapFrom (src => src.Id))
                .ForMember (dest => dest.Name, opt => opt.MapFrom (src => src.Name))
                .ForMember (dest => dest.TeamId, opt => opt.MapFrom (src => src.TeamId));

            CreateMap<DomainModel.Meeting, DBModel.Meeting> ()
                .ForMember (dest => dest.Id, opt => opt.MapFrom (src => ObjectId.Parse (src.Id)))
                .ForMember (dest => dest.Name, opt => opt.MapFrom (src => src.Name))
                .ForMember (dest => dest.TeamId, opt => opt.MapFrom (src => ObjectId.Parse (src.TeamId)));

            //mapping for Team

            CreateMap<DBModel.Team, DomainModel.Team> ()
                .ForMember (dest => dest.TeamId, opt => opt.MapFrom (src => src.Id))
                .ForMember (dest => dest.Name, opt => opt.MapFrom (src => src.Name))
                .ForMember (dest => dest.Invited, opt => opt.MapFrom (src => src.Invited))
                .ForMember (dest => dest.Members, opt => opt.MapFrom (src => src.Members));

            CreateMap<DomainModel.Team, DBModel.Team> ()
                .ForMember (dest => dest.Id, opt => opt.MapFrom (src => ObjectId.Parse (src.TeamId)))
                .ForMember (dest => dest.Name, opt => opt.MapFrom (src => src.Name))
                .ForMember (dest => dest.Invited, opt => opt.MapFrom (src => src.Invited))
                .ForMember (dest => dest.Members, opt => opt.MapFrom (src => src.Members));

            CreateMap<DBModel.User, DomainModel.User> ()
                .ForMember (dest => dest.UserId, opt => opt.MapFrom (src => src.Id))
                .ForMember (dest => dest.Name, opt => opt.MapFrom (src => src.Name))
                .ForMember (dest => dest.Email, opt => opt.MapFrom (src => src.Email))
                .ForMember (dest => dest.LastLoggedIn, opt => opt.MapFrom( src => src.LastLoggedIn));

            CreateMap<DomainModel.User, DBModel.User> ()
                .ForMember (dest => dest.Id, opt => opt.MapFrom (src => src.UserId))
                .ForMember (dest => dest.Name, opt => opt.MapFrom (src => src.Name))
                .ForMember (dest => dest.Email, opt => opt.MapFrom (src => src.Email))
                .ForMember (dest => dest.LastLoggedIn, opt => opt.MapFrom( src => src.LastLoggedIn))
                .ForMember (dest => dest.SubscriptionEnd, opt => opt.Ignore());


            CreateMap<DBModel.TeamMember, DomainModel.TeamMember> ()
            .ForMember (dest => dest.UserId, opt => opt.MapFrom (src => src.UserId));


            CreateMap<DomainModel.TeamMember, DBModel.TeamMember> ()
            .ForMember (dest => dest.UserId, opt => opt.MapFrom (src => ObjectId.Parse(src.UserId)));
   


            CreateMap<DBModel.Invitation, DomainModel.Invitation> ()
                .ReverseMap();

        }

    }

}