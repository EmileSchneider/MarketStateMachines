﻿using MarketStateMachines.Common;
using MarketStateMachines.Trend;

namespace MarketStateMachines.DV_Mode
{
    public class DecreasingVolatilityModeUptrend : IDecreasingVolatilityMode
    {
        ITrend trend;
        decimal highestAtr14;

        decimal lastPivotTwoHigh;
        decimal previousAtr;
        decimal highestClose;
        decimal previousPreviousAtr;

        ITrend fiveMinuteTrend;
        ITrend tenMinuteTrend;

        PivotTwoHighRecognizer pivotTwoHighRecognizer;
        public DecreasingVolatilityModeUptrend(ITrend trend, decimal highestAtr14, decimal lastPivotTwoHigh, decimal previousAtr, decimal highestClose, decimal previousPreviousAtr, ITrend fiveMinuteTrend, ITrend tenMinuteTrend)
        {
            this.trend = trend;
            this.highestAtr14 = highestAtr14;
            this.lastPivotTwoHigh = lastPivotTwoHigh;
            this.previousAtr = previousAtr;
            this.highestClose = highestClose;
            this.previousPreviousAtr = previousPreviousAtr;
            this.fiveMinuteTrend = fiveMinuteTrend;
            this.tenMinuteTrend = tenMinuteTrend;
            this.pivotTwoHighRecognizer = new PivotTwoHighRecognizer();
        }
        public DecreasingVolatilityModeUptrend(ITrend trend, decimal highestAtr14, decimal lastPivotTwoHigh, decimal previousAtr, decimal highestClose, decimal previousPreviousAtr, ITrend fiveMinuteTrend, ITrend tenMinuteTrend, PivotTwoHighRecognizer pivotTwoHighRecognizer)
        {
            this.trend = trend;
            this.highestAtr14 = highestAtr14;
            this.lastPivotTwoHigh = lastPivotTwoHigh;
            this.previousAtr = previousAtr;
            this.highestClose = highestClose;
            this.previousPreviousAtr = previousPreviousAtr;
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


            if (candle.Close > highestClose)
                highestClose = candle.Close;

            if (indicators.Atr14 > highestAtr14)
            {
                if (trend is Uptrend)
                    return new NormalModeUptrend(trend, indicators.Atr14, highestClose, fiveMinuteTrend, tenMinuteTrend);

                if (trend is Downtrend)
                    return new NormalModeDowntrend(trend, indicators.Atr14, candle.Close, fiveMinuteTrend, tenMinuteTrend);

                return new NormalMode(trend, indicators.Atr14, fiveMinuteTrend, tenMinuteTrend);
            }

            if (indicators.Atr14 > lastPivotTwoHigh)
            {
                if (trend is Uptrend)
                    return new NormalModeUptrend(trend, indicators.Atr14, highestClose, fiveMinuteTrend,tenMinuteTrend);

                if (trend is Downtrend)
                    return new NormalModeDowntrend(trend, indicators.Atr14, candle.Close, fiveMinuteTrend, tenMinuteTrend);

                return new NormalMode(trend, indicators.Atr14, fiveMinuteTrend, tenMinuteTrend);
            }

            pivotTwoHighRecognizer.NewAtr(indicators.Atr14);
            lastPivotTwoHigh = pivotTwoHighRecognizer.CurrentPivotTwoHigh(); 

            previousPreviousAtr = previousAtr;
            previousAtr = indicators.Atr14;

            return new DecreasingVolatilityModeUptrend(trend, highestAtr14, lastPivotTwoHigh, previousAtr, highestClose, previousPreviousAtr, fiveMinuteTrend, tenMinuteTrend, pivotTwoHighRecognizer);
        }
    }
}