﻿using System;
using Realm.Library.Common;

namespace SmaugCS.Data
{
    public class SpecialFunction : Entity
    {
        public SpecialFunction(long id, string name) : base(id, name)
        {
        }

        public Func<CharacterInstance, bool> Value { get; set; }
    }
}