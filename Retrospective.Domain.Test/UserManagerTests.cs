using AutoMapper;
using Xunit;

namespace Retrospective.Domain.Test.obj
{
    [Collection ("Domain Test collection")]
    public class UserManagerTests
    {
        AutoMapper.Mapper mapper = null;
        TestFixture fixture;

        public UserManagerTests (TestFixture fixture) {
            this.fixture = fixture;
            var config = new MapperConfiguration (c => {
                c.AddProfile<Retrospective.Domain.DomainProfile> ();
            });

            mapper = new Mapper (config);

        }

        [Fact]
        public void GetUserFromEmail_ForExistingUser_HasUserID()
        {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.UserManager> ();
            var userManager= new UserManager(logger,this.mapper,this.fixture.Database);
            var user= userManager.GetUserFromEmail(this.fixture.SampleUser.Email);
            System.Console.WriteLine(user.UserId);
            Assert.DoesNotContain(user.UserId, "00000000");
        }

        [Fact]
        public void GetUserFromEmail_ForExistingUser_UpdatedLastLoggedIn()
        {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.UserManager> ();
            var userManager= new UserManager(logger,this.mapper,this.fixture.Database);
            var user= userManager.GetUserFromEmail(this.fixture.SampleUser.Email);

            var updatTime= System.DateTime.UtcNow;
            user.LastLoggedIn=updatTime;
            var newUSer = userManager.UpdateUser(user);
            

            Assert.Equal(user.LastLoggedIn, updatTime);
        }
    }
}