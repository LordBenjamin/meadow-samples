using BenjaminOwen.Meadow.Samples.DigitalThermometer.Web;
using Meadow.Foundation.Web.Maple.Server;
using Meadow.Foundation.Web.Maple.Server.Routing;
using System;

namespace DigitalThermometer.Web
{
    public class TemperatureHttpHandler : RequestHandlerBase
    {
        // TODO - this is nasty! Maple doesn't support constructor parameters as far as I can tell :(
        public static MeadowApp App { get; } = Program.AppInstance;

        [HttpGet]
        public void Temperature()
        {
            Console.WriteLine("GET::Temperature");

            this.Context.Response.ContentType = ContentTypes.Application_Json;
            this.Context.Response.StatusCode = 200;
            this.Send(new TemperatureResponse(App.CurrentTemperature.Celsius)).Wait();
        }

        private class TemperatureResponse
        {

            public TemperatureResponse(double temperatureCelsius)
            {
                TemperatureCelsius = temperatureCelsius;
            }

            public double TemperatureCelsius { get; set; }
        }
    }
}
