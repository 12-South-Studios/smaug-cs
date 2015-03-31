using System;
using SmaugCS.Data.Instances;

namespace SmaugCS.Data
{
    public class UseHistory
    {
        public int TimesUsed { get; private set; }
        public double TotalTimeUsed { get; private set; }
        public long MinimumTime { get; set; }
        public long MaximumTime { get; set; }
        public DateTime LastUsed { get; private set; }
        public string LastUsedBy { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void Use(CharacterInstance ch, TimeSpan time)
        {
            TimesUsed++;
            TotalTimeUsed += time.TotalMilliseconds;
            LastUsed = DateTime.Now;
            LastUsedBy = ch.Name;
        }
    }
}
