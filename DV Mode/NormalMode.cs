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

        private ITrend fiveMinuteTrend;
        private ITrend tenMinuteTrend;
        public NormalMode(ITrend trend, decimal lastAtr, ITrend fiveMinuteTrend, ITrend tenMinuteTrend) 
        {
            this.trend = trend;
            this.lastAtr = lastAtr;
            this.fiveMinuteTrend = fiveMinuteTrend;
            this.tenMinuteTrend = tenMinuteTrend;
        }

        public NormalMode()
        {
            trend = new MarketStateMachines.Trend.Range();
            fiveMinuteTrend = new MarketStateMachines.Trend.Range();
            tenMinuteTrend = new MarketStateMachines.Trend.Range();
        }

        public IDecreasingVolatilityMode Transition(Candle candle, Indicators indicators)
        {
            trend = trend.CandleTransition(candle, indicators);

            if (trend is Uptrend)
            {
                if (indicators.Atr14 < lastAtr && !DowntrendMode())
                    return new DecreasingVolatilityModeUptrend(trend, indicators.Atr14, decimal.MaxValue,decimal.MaxValue, candle.Close, decimal.MaxValue, fiveMinuteTrend, tenMinuteTrend);
                return new NormalModeUptrend(trend, lastAtr, candle.Close, fiveMinuteTrend, tenMinuteTrend);
            }
            if (trend is Downtrend)
            {
                if(indicators.Atr14 < lastAtr && !UptrendMode())
                {
                    return new DecreasingVolatilityModeDowntrend(trend, indicators.Atr14, decimal.MaxValue,decimal.MaxValue,decimal.MaxValue, candle.Close, fiveMinuteTrend, tenMinuteTrend);
                }
                return new NormalModeDowntrend(trend, lastAtr, candle.Close, fiveMinuteTrend, tenMinuteTrend);
            }
            lastAtr = indicators.Atr14;
            return new NormalMode(trend, lastAtr, fiveMinuteTrend, tenMinuteTrend);
        }

        private bool UptrendMode()
        {
            return fiveMinuteTrend is Uptrend && tenMinuteTrend is Uptrend;
        } 

        private bool DowntrendMode()
        {
            return fiveMinuteTrend is Downtrend && tenMinuteTrend is Downtrend;
        }
        public void NewFiveMinute(Candle candle, Indicators indicators)
        {
            fiveMinuteTrend = fiveMinuteTrend.CandleTransition(candle, indicators); 
        }

        public void NewTenMinute(Candle candle, Indicators indicators)
        {
            tenMinuteTrend = tenMinuteTrend.CandleTransition(candle, indicators);
        }
    }
}
