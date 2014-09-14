using System;
using System.Linq;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;

namespace SmaugCS.Extensions
{
    public static class Timer
    {
        public static bool AddTimer(this CharacterInstance ch, TimerTypes type, int count,
            Action<CharacterInstance, string> fun, int value)
        {
            if (ch.Timers.Any(x => x.Type == type))
                return false;

            TimerData timer = new TimerData
            {
                Count = count,
                Type = type,
                Value = value,
                Action = new DoFunction {Value = fun}
            };
            ch.Timers.ToList().Add(timer);
            return true;
        }

        public static TimerData GetTimer(this CharacterInstance ch, TimerTypes type)
        {
            return ch.Timers.FirstOrDefault(x => x.Type == type);
        }

        public static bool RemoveTimer(this CharacterInstance ch, TimerTypes type)
        {
            TimerData timer = ch.GetTimer(type);
            return timer != null && ch.Timers.ToList().Remove(timer);
        }

        public static bool RemoveTimer(this CharacterInstance ch, TimerData timer)
        {
            return ch.Timers.Any(x => x == timer) && ch.Timers.ToList().Remove(timer);
        }

        public static bool HasTimer(this CharacterInstance ch, TimerTypes type)
        {
            return ch.Timers.Any(x => x.Type == type);
        }
    }
}
