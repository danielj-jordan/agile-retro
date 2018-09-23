using Xunit;
using AutoMapper;

namespace apptest
{
    public class AutomapperTest
    {
        private IMapper mapper; 
        private MapperConfiguration config;

        public AutomapperTest()
        {
            config = new MapperConfiguration(c =>
            {
                c.AddProfile<app.Domain.DomainProfile>();
            });
        
        }

        [Fact]
        public void AutomapperConfiguration()
        {

            config.AssertConfigurationIsValid();
        
        }


    }
}