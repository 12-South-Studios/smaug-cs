using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Data;

namespace SmaugCS.Managers
{
    public interface IGameManager
    {
        TimeInfoData GameTime { get; }
        SystemData SystemData { get; }
        void SetGameTime(TimeInfoData gameTime);
    }
}
