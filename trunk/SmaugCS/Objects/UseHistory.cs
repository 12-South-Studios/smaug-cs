using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Objects
{
    /// <summary>
    /// 
    /// </summary>
    public class UseHistory
    {
        public int TimesUsed { get; private set; }
        public double TotalTimeUsed { get; private set; }
        public long MinimumTime { get; set; }
        public long MaximumTime { get; set; }
        public DateTime LastUsed { get; private set; }
        public string LastUsedBy { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="time"></param>
        public void Use(CharacterInstance ch, TimeSpan time)
        {
            TimesUsed++;
            TotalTimeUsed += time.TotalMilliseconds;
            LastUsed = DateTime.Now;
            LastUsedBy = ch.Name;
        }
    }
}
