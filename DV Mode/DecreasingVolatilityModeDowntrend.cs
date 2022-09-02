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

        private ITrend fiveMinuteTrend;
        private ITrend tenMinuteTrend;

        private PivotTwoHighRecognizer pivotTwoHighRecognizer;
         public DecreasingVolatilityModeDowntrend(ITrend trend, decimal highestAtr14, decimal previousAtr14, decimal previousPreviousAtr14, decimal lastPivotTwoHigh, decimal lowestClosel, ITrend fiveMinuteTrend, ITrend tenMinuteTrend)
        {
            this.trend = trend;
            this.highestAtr14 = highestAtr14;
            previousAtr = previousAtr14;
            previousPreviousAtr = previousPreviousAtr14;
            this.lastPivotTwoHigh = lastPivotTwoHigh;
            this.lowestClose = lowestClose;
            this.fiveMinuteTrend = fiveMinuteTrend;
            this.tenMinuteTrend = tenMinuteTrend;

            this.pivotTwoHighRecognizer = new PivotTwoHighRecognizer();
        }
       public DecreasingVolatilityModeDowntrend(ITrend trend, decimal highestAtr14, decimal previousAtr14, decimal previousPreviousAtr14, decimal lastPivotTwoHigh, decimal lowestClosel, ITrend fiveMinuteTrend, ITrend tenMinuteTrend, PivotTwoHighRecognizer pivotTwoHighRecognizer)
        {
            this.trend = trend;
            this.highestAtr14 = highestAtr14;
            previousAtr = previousAtr14;
            previousPreviousAtr = previousPreviousAtr14;
            this.lastPivotTwoHigh = lastPivotTwoHigh;
            this.lowestClose = lowestClose;
            this.fiveMinuteTrend = fiveMinuteTrend;
            this.tenMinuteTrend = tenMinuteTrend;

            this.pivotTwoHighRecognizer = pivotTwoHighRecognizer; 
        }


        public void NewFiveMinute(Candle candle, Indicators indicators)
        {
            fiveMinuteTrend = fiveMinuteTrend.CandleTransition(candle, indicators);
        }

        public void NewTenMinute(Candle candle, Indicators indicators)
        {
            tenMinuteTrend = tenMinuteTrend.CandleTransition(candle, indicators);
        }

        public IDecreasingVolatilityMode Transition(Candle candle, Indicators indicators)
        {
            trend = trend.CandleTransition(candle, indicators);
            if (candle.Close < lowestClose)
                lowestClose = candle.Close;

            if (indicators.Atr14 > highestAtr14)
            {
                if (trend is Downtrend)
                    return new NormalModeDowntrend(trend, indicators.Atr14, lowestClose, fiveMinuteTrend, tenMinuteTrend);
                if (trend is Uptrend)
                    return new NormalModeUptrend(trend, indicators.Atr14, lowestClose, fiveMinuteTrend, tenMinuteTrend);
                return new NormalMode(trend, indicators.Atr14, fiveMinuteTrend, tenMinuteTrend);
            }
            if (indicators.Atr14 > lastPivotTwoHigh)
            {
                if (trend is Downtrend)
                    return new NormalModeDowntrend(trend, indicators.Atr14, lowestClose, fiveMinuteTrend, tenMinuteTrend);
                if (trend is Uptrend)
                    return new NormalModeUptrend(trend, indicators.Atr14, lowestClose, fiveMinuteTrend, tenMinuteTrend);
                return new NormalMode(trend, indicators.Atr14, fiveMinuteTrend, tenMinuteTrend);
            }

            pivotTwoHighRecognizer.NewAtr(indicators.Atr14);
            lastPivotTwoHigh = pivotTwoHighRecognizer.CurrentPivotTwoHigh();

            previousPreviousAtr = previousAtr;
            previousAtr = indicators.Atr14;
            return new DecreasingVolatilityModeDowntrend(trend, highestAtr14, previousAtr, previousPreviousAtr, lastPivotTwoHigh, lowestClose, fiveMinuteTrend,tenMinuteTrend, pivotTwoHighRecognizer);
        }
    }
}
