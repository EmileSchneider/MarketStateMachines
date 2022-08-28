using MarketStateMachines.Trend;
using TradingEngine.Loggers;

namespace MarketStateMachines.DV_Mode
{
    public class DecreasingVolatilityModeDowntrend : IDecreasingVolatilityMode
    {
        private ITrend trend;
        private decimal highestAtr14;
        private decimal lowestClose;
        private decimal lastPivotTwoHigh;
        private decimal previousAtr;
        private decimal previousPreviousAtr;
     
         public DecreasingVolatilityModeDowntrend(ITrend trend, decimal highestAtr14, decimal previousAtr14, decimal previousPreviousAtr14, decimal lastPivotTwoHigh, decimal lowestClosel)
        {
            this.trend = trend;
            this.highestAtr14 = highestAtr14;
            previousAtr = previousAtr14;
            previousPreviousAtr = previousPreviousAtr14; 
            this.lastPivotTwoHigh = lastPivotTwoHigh;
            this.lowestClose = lowestClose;
        }



        public IDecreasingVolatilityMode Transition(Candle candle, Indicators indicators)
        {
            trend = trend.CandleTransition(candle, indicators);
            if (candle.Close < lowestClose)
                lowestClose = candle.Close;

            if (indicators.Atr14 > highestAtr14)
            {
                if (trend is Downtrend)
                    return new NormalModeDowntrend(trend, indicators.Atr14, lowestClose);
                if (trend is Uptrend)
                    return new NormalModeUptrend(trend, indicators.Atr14, lowestClose);
                return new NormalMode(trend, indicators.Atr14);
            }
            if (indicators.Atr14 > lastPivotTwoHigh)
            {
                if (trend is Downtrend)
                    return new NormalModeDowntrend(trend, indicators.Atr14, lowestClose);
                if (trend is Uptrend)
                    return new NormalModeUptrend(trend, indicators.Atr14, lowestClose);
                return new NormalMode(trend, indicators.Atr14);
            }

            if (previousAtr > indicators.Atr14 && previousPreviousAtr < previousAtr)
            {
                lastPivotTwoHigh = previousAtr;
            }
            previousPreviousAtr = previousAtr;
            previousAtr = indicators.Atr14;
            return new DecreasingVolatilityModeDowntrend(trend, highestAtr14, previousAtr, previousPreviousAtr, lastPivotTwoHigh, lowestClose);
        }
    }
}
