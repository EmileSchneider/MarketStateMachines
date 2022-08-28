using MarketStateMachines.Trend;
using TradingEngine.Loggers;

namespace MarketStateMachines.DV_Mode
{
    internal class NormalModeUptrend : IDecreasingVolatilityMode
    {
        private ITrend trend;
        private decimal lastAtr;
        private decimal highestClose;

        decimal highestAtr;
        public NormalModeUptrend(ITrend trend, decimal lastAtr, decimal highestClose)
        {
            this.trend = trend;
            this.lastAtr = lastAtr;
            this.highestClose = highestClose;
            this.highestAtr = lastAtr;
        }

        public IDecreasingVolatilityMode Transition(Candle candle, Indicators indicators)
        {
            trend = trend.CandleTransition(candle, indicators);

             if (indicators.Atr14 > highestAtr)
                highestAtr = indicators.Atr14;

            if (!(trend is Uptrend))
                return new NormalMode(trend, indicators.Atr14);
            if (candle.Close > highestClose && indicators.Atr14 < lastAtr)
                return new DecreasingVolatilityModeUptrend(trend, highestAtr,decimal.MaxValue,decimal.MaxValue, candle.Close, decimal.MaxValue);
            if (candle.Close > highestClose)
                return new NormalModeUptrend(trend, indicators.Atr14, candle.Close);
            return new NormalModeUptrend(trend, indicators.Atr14,highestClose );
        }
    }
}