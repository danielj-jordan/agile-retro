using AutoMapper;
using ViewModel =app.Model;
using DBModel=Retrospective.Data.Model;
using Microsoft.Extensions.Logging;


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
            CreateMap<DBModel.Comment, ViewModel.Comment>()
                .ForMember(dest=>dest.CommentId, opt=>opt.MapFrom(src=>src.Id))
                .ForMember(dest=>dest.CategoryId, opt=>opt.MapFrom(src=>src.CategoryNumber))
                .ForMember(dest=>dest.UpdateUser, opt=>opt.MapFrom(src=>src.LastUpdateUser));
               

        }

        public MapperConfiguration Initialize()
        {
           MapperConfiguration config = new MapperConfiguration(
               cfg=>{cfg.CreateMap<DBModel.Comment, ViewModel.Comment>()
                .ForMember(dest=>dest.CommentId, opt=>opt.MapFrom(src=>src.Id))
                .ForMember(dest=>dest.CategoryId, opt=>opt.MapFrom(src=>src.CategoryNumber))
                .ForMember(dest=>dest.UpdateUser, opt=>opt.MapFrom(src=>src.LastUpdateUser));
               });

               return config;
        }
    }
       
    
}