using System;

namespace SmaugCS.Weather
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class WeatherMessageAttribute : Attribute
    {
        public WeatherThresholdTypes Threshold { get; private set; }
        public string Message { get; private set; }

        public WeatherMessageAttribute(WeatherThresholdTypes threshold, string message)
        {
            Threshold = threshold;
            Message = message;
        }
    }
}
