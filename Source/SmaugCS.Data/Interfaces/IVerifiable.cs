using SmaugCS.Constants.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Data.Interfaces
{
    public interface IVerifiable
    {
        bool IsNpc();
        bool IsFloating();
        bool IsAffected(AffectedByTypes affectedBy);
        bool IsImmortal(int level = 51);
        bool IsHero(int level = 50);
    }
}
