using MarketStateMachines.Common;

namespace MarketStateMachines.MeanReversion
{
    public class LongSetupRTC : IMeanReversion
    {
        public decimal Low { get; set; }
        public decimal High { get; set; }

        public LongSetupRTC(decimal low, decimal high)
        {
            Low = low;
            High = high;
        }

        public IMeanReversion CandleTransition(Candle candle, Indicators indicators)
        {
            return new NoSetup().CandleTransition(candle, indicators);
        }

        public IMeanReversion TickTransition(MarketTick tick)
        {
            return this;
        }
    }
}