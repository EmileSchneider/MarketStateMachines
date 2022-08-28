using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngine.Loggers
{
    public interface IDVModeLogger
    {
        public void LogNormalMode(string message);
        public void LogUptrend(string message);
        public void LogDowntrend(string message);
        public void LogDvUp(string message);
        public void LogDvDown(string message);
    }
}
