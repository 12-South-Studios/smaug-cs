using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Managers
{
    public interface ILogManager
    {
        void InitializeManager(string path);

        void Bug(string str, params object[] args);
        void BootLog(string str, params object[] args);
        void BootLog(Exception ex);
        void Log(LogTypes logType, int level, string fmt, params object[] args);
        void Log(string fmt, params object[] args);
        void Log(string str, LogTypes logType, int level);
    }
}
