using MarketStateMachines.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketStateMachines.DV_Mode
{
    public interface IDecreasingVolatilityMode
    {
        public IDecreasingVolatilityMode Transition(Candle candle, Indicators indicators);

        public void NewFiveMinute(Candle candle, Indicators indicators);
        public void NewTenMinute(Candle candle, Indicators indicators);
    }
}
