﻿using Realm.Library.Common.Objects;

namespace SmaugCS.Data
{
    public class SocialData : Entity
    {
        public SocialData(long id, string name) : base(id, name)
        {
        }

        public string CharNoArg { get; set; }
        public string OthersNoArg { get; set; }
        public string CharFound { get; set; }
        public string OthersFound { get; set; }
        public string VictFound { get; set; }
        public string CharAuto { get; set; }
        public string OthersAuto { get; set; }
    }
}
