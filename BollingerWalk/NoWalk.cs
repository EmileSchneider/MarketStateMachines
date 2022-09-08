using MarketStateMachines.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketStateMachines.BollingerWalk
{
    public class NoWalk : IBollingerWalk
    {
        public IBollingerWalk Transition(Candle candle, Indicators indicators)
        {
            if (candle.Close >= indicators.BollingerBandTop)
                return new BollingerWalkUp();
            if (candle.Close <= indicators.BollingerBandBottom)
                return new BollingerWalkDown();
            return new NoWalk();
        }
    }
}
