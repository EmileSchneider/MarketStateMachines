using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketStateMachines.BollingerWalk
{
    public interface IBollingerWalk
    {
        public IBollingerWalk Transition(Candle candle, Indicators indicators);
    }
}
