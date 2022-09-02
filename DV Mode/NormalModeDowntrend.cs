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

        private ITrend fiveMinuteTrend;
        private ITrend tenMinuteTrend;
        public NormalModeDowntrend(ITrend trend, decimal lastAtr, decimal lowestClose, ITrend fiveMinuteTrend, ITrend tenMinuteTrend)
        {
            this.lastAtr = lastAtr;
            this.trend = trend;
            this.lowestClose = lowestClose;
            this.highestAtr = lastAtr;
            this.fiveMinuteTrend = fiveMinuteTrend;
            this.tenMinuteTrend = tenMinuteTrend;
        }

        public void NewFiveMinute(Candle candle, Indicators indicators)
        {
            fiveMinuteTrend = fiveMinuteTrend.CandleTransition(candle, indicators);
        }

        public void NewTenMinute(Candle candle, Indicators indicators)
        {
            tenMinuteTrend = tenMinuteTrend.CandleTransition(candle, indicators);
        }

        private bool UptrendMode()
        {
            return fiveMinuteTrend is Uptrend && tenMinuteTrend is Uptrend;
        } 

        private bool DowntrendMode()
        {
            return fiveMinuteTrend is Downtrend && tenMinuteTrend is Downtrend;
        }

        public IDecreasingVolatilityMode Transition(Candle candle, Indicators indicators)
        {
            trend = trend.CandleTransition(candle, indicators);

            if (indicators.Atr14 > highestAtr)
                highestAtr = indicators.Atr14;
            if (!(trend is Downtrend))
            {
                return new NormalMode(trend, indicators.Atr14, fiveMinuteTrend, tenMinuteTrend);
            }
            if (candle.Close < lowestClose && indicators.Atr14 < lastAtr && !UptrendMode())
            {
                return new DecreasingVolatilityModeDowntrend(trend, highestAtr, decimal.MaxValue, decimal.MaxValue, decimal.MaxValue, candle.Close, fiveMinuteTrend, tenMinuteTrend);
            }
            if (candle.Close < lowestClose)
            {
                return new NormalModeDowntrend(trend, indicators.Atr14, candle.Close, fiveMinuteTrend, tenMinuteTrend);
            }
            return new NormalModeDowntrend(trend, indicators.Atr14, lowestClose, fiveMinuteTrend, tenMinuteTrend);
        }
    }
}