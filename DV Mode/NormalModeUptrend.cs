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

        private ITrend fiveMinuteTrend;
        private ITrend tenMinuteTrend;
        public NormalModeUptrend(ITrend trend, decimal lastAtr, decimal highestClose, ITrend fiveMinuteTrend, ITrend tenMinuteTrend)
        {
            this.trend = trend;
            this.lastAtr = lastAtr;
            this.highestClose = highestClose;
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

        private bool DowntrendMode()
        {
            return fiveMinuteTrend is Downtrend && tenMinuteTrend is Downtrend;
        }
        public IDecreasingVolatilityMode Transition(Candle candle, Indicators indicators)
        {
            trend = trend.CandleTransition(candle, indicators);

             if (indicators.Atr14 > highestAtr)
                highestAtr = indicators.Atr14;

            if (!(trend is Uptrend))
                return new NormalMode(trend, indicators.Atr14, fiveMinuteTrend, tenMinuteTrend);
            if (candle.Close > highestClose && indicators.Atr14 < lastAtr && !DowntrendMode())
                return new DecreasingVolatilityModeUptrend(trend, highestAtr,decimal.MaxValue,decimal.MaxValue, candle.Close, decimal.MaxValue, fiveMinuteTrend, tenMinuteTrend);
            if (candle.Close > highestClose)
                return new NormalModeUptrend(trend, indicators.Atr14, candle.Close, fiveMinuteTrend, tenMinuteTrend);
            return new NormalModeUptrend(trend, indicators.Atr14,highestClose, fiveMinuteTrend, tenMinuteTrend);
        }
    }
}