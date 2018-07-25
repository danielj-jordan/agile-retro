using AutoMapper;
using ViewModel =app.Model;
using DBModel=Retrospective.Data.Model;
using Microsoft.Extensions.Logging;


namespace app.Domain
{
    public class Mapper: Profile
    {
        ILogger _logger;

        public Mapper(ILogger<Mapper> logger)
        {
            _logger=logger;
            //Initialize();

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
                
            
            _logger.LogDebug("mapper initialize");

            return config;
        }
    }
       
    
}