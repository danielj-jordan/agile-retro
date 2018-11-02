using AutoMapper;
using Xunit;

namespace Retrospective.Domain.Test {
    public class AutomapperTest {
        private MapperConfiguration config;

        public AutomapperTest () {
            config = new MapperConfiguration (c => {
                c.AddProfile<Retrospective.Domain.DomainProfile> ();
            });

        }

        [Fact]
        public void AutomapperConfiguration () {
            config.AssertConfigurationIsValid ();
        }
    }
}