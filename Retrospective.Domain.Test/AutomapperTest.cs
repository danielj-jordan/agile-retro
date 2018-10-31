using Xunit;
using AutoMapper;

namespace Retrospective.Domain.Test
{
    public class AutomapperTest
    {
        private IMapper mapper; 
        private MapperConfiguration config;

        public AutomapperTest()
        {
            config = new MapperConfiguration(c =>
            {
                c.AddProfile<Retrospective.Domain.DomainProfile>();
            });
        
        }

        [Fact]
        public void AutomapperConfiguration()
        {

            config.AssertConfigurationIsValid();
        
        }


    }
}