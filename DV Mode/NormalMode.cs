using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketStateMachines.Trend;
using TradingEngine.Loggers;

namespace MarketStateMachines.DV_Mode
{
    public class NormalMode : IDecreasingVolatilityMode
    {

        private ITrend trend;
        private decimal lastAtr;
        public NormalMode(ITrend trend, decimal lastAtr) 
        {
            this.trend = trend;
            this.lastAtr = lastAtr;
        }

        public NormalMode()
        {
            trend = new MarketStateMachines.Trend.Range();
        }

        public IDecreasingVolatilityMode Transition(Candle candle, Indicators indicators)
        {
            trend = trend.CandleTransition(candle, indicators);

            if (trend is Uptrend)
            {
                if (indicators.Atr14 < lastAtr)
                    return new DecreasingVolatilityModeUptrend(trend, indicators.Atr14, decimal.MaxValue,decimal.MaxValue, candle.Close, decimal.MaxValue);
                return new NormalModeUptrend(trend, lastAtr, candle.Close);
            }
            if (trend is Downtrend)
            {
                if(indicators.Atr14 < lastAtr)
                {
                    return new DecreasingVolatilityModeDowntrend(trend, indicators.Atr14, decimal.MaxValue,decimal.MaxValue,decimal.MaxValue, candle.Close);
                }
                return new NormalModeDowntrend(trend, lastAtr, candle.Close);
            }
            lastAtr = indicators.Atr14;
            return new NormalMode(trend, lastAtr);
        }
    }
}
