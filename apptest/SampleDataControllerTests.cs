using System;
using Xunit;

namespace apptest
{
    public class SampleDataControllerTests
    {
        

        [Fact]
        public void GetWeatherForecsts()
        {
                var controller = new app.Controllers.SampleDataController();

                var forecasts= controller.WeatherForecasts();
                var count=0;
                foreach( var forecast in forecasts)
                {
                        count++;
                }
                Console.WriteLine("There are {0} forecasts.",count);
                Assert.True(count>0, "There should be some weather forecasts.");
        }

    }
}
