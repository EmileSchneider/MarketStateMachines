using MarketStateMachines.Trend;
using TradingEngine.Loggers;

namespace MarketStateMachines.DV_Mode
{
    public class DecreasingVolatilityModeUptrend : IDecreasingVolatilityMode
    {
        private ITrend trend;
        private decimal highestAtr14;

        private decimal lastPivotTwoHigh;
        private decimal previousAtr;
        private decimal highestClose;
        private decimal previousPreviousAtr;


        public DecreasingVolatilityModeUptrend(ITrend trend, decimal highestAtr14, decimal lastPivotTwoHigh, decimal previousAtr, decimal highestClose, decimal previousPreviousAtr)
        {
            this.trend = trend;
            this.highestAtr14 = highestAtr14;
            this.lastPivotTwoHigh = lastPivotTwoHigh;
            this.previousAtr = previousAtr;
            this.highestClose = highestClose;
            this.previousPreviousAtr = previousPreviousAtr;
        }

   
        public IDecreasingVolatilityMode Transition(Candle candle, Indicators indicators)
        {
            trend = trend.CandleTransition(candle, indicators);


            if (candle.Close > highestClose)
                highestClose = candle.Close;

            if (indicators.Atr14 > highestAtr14)
            {
                if (trend is Uptrend)
                    return new NormalModeUptrend(trend, indicators.Atr14, highestClose);
                if (trend is Downtrend)
                    return new NormalModeDowntrend(trend, indicators.Atr14, candle.Close);
                return new NormalMode(trend, indicators.Atr14);

            }

            if (indicators.Atr14 > lastPivotTwoHigh)
            {
                if (trend is Uptrend)
                    return new NormalModeUptrend(trend, indicators.Atr14, highestClose);
                if (trend is Downtrend)
                    return new NormalModeDowntrend(trend, indicators.Atr14, candle.Close);
                return new NormalMode(trend, indicators.Atr14);

            }

            if (previousAtr > indicators.Atr14 && previousPreviousAtr < indicators.Atr14)
            {
                lastPivotTwoHigh = previousAtr;
            }

            previousPreviousAtr = previousAtr;
            previousAtr = indicators.Atr14;
            return new DecreasingVolatilityModeUptrend(trend, highestAtr14, lastPivotTwoHigh, previousAtr, highestClose, previousPreviousAtr);
        }
    }
}