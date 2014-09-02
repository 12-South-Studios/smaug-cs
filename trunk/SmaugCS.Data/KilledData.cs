using System;

namespace SmaugCS.Data
{
    public class KilledData
    {
        public long ID { get; private set; }
        public int Count { get; private set; }
        public DateTime Added { get; private set; }
        public DateTime Updated { get; private set; }

        public KilledData(long id)
        {
            ID = id;
            Added = DateTime.Now;
            Updated = Added;
            Count = 1;
        }

        public void Increment(int number)
        {
            Count += number;
            Updated = DateTime.Now;
        }
    }
}
