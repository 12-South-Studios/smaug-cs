using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Weather
{
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = true)]
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
