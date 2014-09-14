using System;
using System.Collections.Generic;
using SmaugCS.Data;

namespace SmaugCS.SpecFuns
{
    public interface ISpecFunHandler
    {
        bool IsValidSpecFun(string name);
        SpecialFunction GetSpecFun(string name);
        SkillData PickSpell(Dictionary<int, Tuple<int, string>> lookupTable, int characterLevel);
    }
}