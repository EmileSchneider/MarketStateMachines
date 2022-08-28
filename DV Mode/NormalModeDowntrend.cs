using MarketStateMachines.Trend;
using TradingEngine.Loggers;

namespace MarketStateMachines.DV_Mode
{
    public class NormalModeDowntrend : IDecreasingVolatilityMode
    {
        private decimal lastAtr;
        private ITrend trend;
        private decimal lowestClose;
        private decimal highestAtr;
        public NormalModeDowntrend(ITrend trend, decimal lastAtr, decimal lowestClose)
        {
            this.lastAtr = lastAtr;
            this.trend = trend;
            this.lowestClose = lowestClose;
            this.highestAtr = lastAtr;
        }

        public IDecreasingVolatilityMode Transition(Candle candle, Indicators indicators)
        {
            trend = trend.CandleTransition(candle, indicators);

            if (indicators.Atr14 > highestAtr)
                highestAtr = indicators.Atr14;
            if (!(trend is Downtrend))
            {
                return new NormalMode(trend, indicators.Atr14);
            }
            if (candle.Close < lowestClose && indicators.Atr14 < lastAtr)
            {
                return new DecreasingVolatilityModeDowntrend(trend, highestAtr, decimal.MaxValue, decimal.MaxValue, decimal.MaxValue, candle.Close);
            }
            if (candle.Close < lowestClose)
            {
                return new NormalModeDowntrend(trend, indicators.Atr14, candle.Close);
            }
            return new NormalModeDowntrend(trend, indicators.Atr14, lowestClose);
        }
    }
}