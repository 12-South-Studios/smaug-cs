using System;
using SmaugCS.Weather.Enums;

namespace SmaugCS.Weather
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class WeatherMessageAttribute : Attribute
    {
        public WeatherThresholdTypes Threshold { get; set; }
        public string Message { get; set; }
    }
}
